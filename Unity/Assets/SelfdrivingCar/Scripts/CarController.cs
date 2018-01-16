using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public EvolutionManager evolutionManager;
	// Left, right, middle
	public Transform[] sensors;

	public WheelCollider[] frontWheels;

	public UiController uiController;

	public float minTorque;
	public float maxTorque;

	private Ray[] sensorRays;
	
	private float[] input = new float[3];

	private float timer;

	private Vector3 startingPosition;

	private float distanceTraveled;
	private float maxDistance;

	private Vector3 lastPosition;

	private int distZeroCounter;
	
	// Use this for initialization
	void Start ()
	{
		sensorRays = new Ray[3];
		sensorRays[0] = new Ray();
		sensorRays[1] = new Ray();
		sensorRays[2] = new Ray();

		startingPosition = transform.position;
		lastPosition = transform.position;
		uiController.UpdateNetworkText(evolutionManager.CurrenGeneration, evolutionManager.CurrenGenome + 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;


			for (int i = 0; i < sensorRays.Length; i++)
			{
				sensorRays[i].origin = sensors[i].position;
				float angle = 0;
			
				switch (i)
				{
					case 0:
						angle = -45 * Mathf.Deg2Rad;
						break;
					
					case 1:
						angle = 45 * Mathf.Deg2Rad;
						break;
				}

				sensorRays[i].direction = sensors[i].TransformDirection(Mathf.Sin(angle), 0, Mathf.Cos(angle));

				RaycastHit hit;
			
				if (Physics.Raycast(sensorRays[i], out hit, 2.5f))
				{
					input[i] = hit.distance;
				}
				else
				{
					input[i] = 2.5f;
				}
			}

			if (HasCrashed())
			{
				ResetCar();
			}
			else
			{
				float[] output = evolutionManager.GetOutput(input);
				ApplyOutput(output);
				distanceTraveled += Vector3.Distance(transform.position, lastPosition);
				lastPosition = transform.position;
				uiController.UpdateCurrentDistance(distanceTraveled);

				if (distanceTraveled > maxDistance)
				{
					maxDistance = distanceTraveled;
					uiController.UpdateMaxDistance(maxDistance);
				}
			}
	}

	private void ApplyOutput(float[] output)
	{
		float torque = output[0] * maxTorque;
		float steering = output[1] * 45;

		frontWheels[0].motorTorque = Mathf.Clamp(torque, minTorque, maxTorque);
		frontWheels[1].motorTorque = Mathf.Clamp(torque, minTorque, maxTorque);

		frontWheels[0].steerAngle = steering;
		frontWheels[1].steerAngle = steering;
	}

	private void ResetCar()
	{
		evolutionManager.NextNetwork(distanceTraveled);
		frontWheels[0].motorTorque = 0;
		frontWheels[1].motorTorque = 0;
		frontWheels[0].steerAngle = 0;
		frontWheels[1].steerAngle = 0;

		transform.position = startingPosition;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		distanceTraveled = 0;
		lastPosition = transform.position;
		uiController.UpdateNetworkText(evolutionManager.CurrenGeneration, evolutionManager.CurrenGenome + 1);
		distZeroCounter = 0;
	}

	private bool HasCrashed()
	{
		return input[0] < 0.1f || input[1] < 0.1f || input[2] < 0.1f;
	}
}
