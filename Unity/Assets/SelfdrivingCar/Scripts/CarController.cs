using System;
using System.IO;
using AI.Evolution;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public EvolutionManager evolutionManager;
	// Left, right, middle
	public Transform[] sensors;

	public WheelCollider[] wheels;

	public UiController uiController;

	private Ray[] sensorRays;
	
	private float[] input = new float[3];

	private Vector3 startingPosition;

	private float distanceTraveled;
	private float maxDistance;

	private Vector3 lastPosition;

	private int distZeroCounter;

	private bool play;
	
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
		if (play)
		{
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
			
				if (Physics.Raycast(sensorRays[i], out hit))
				{
					input[i] = hit.distance;
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

		if (Input.GetKeyDown(KeyCode.A))
		{
			play = true;
			wheels[0].motorTorque = 30;
			wheels[1].motorTorque = 30;
			wheels[2].motorTorque = 30;
			wheels[3].motorTorque = 30;
		}
	}

	private void ApplyOutput(float[] output)
	{
		float steering = output[0] * 45;

		wheels[0].steerAngle = steering;
		wheels[1].steerAngle = steering;
	}

	private void ResetCar()
	{
		evolutionManager.NextNetwork(distanceTraveled);
		wheels[0].brakeTorque = 100;
		wheels[1].brakeTorque = 100;
		wheels[2].brakeTorque = 100;
		wheels[3].brakeTorque = 100;
		wheels[0].steerAngle = 0;
		wheels[1].steerAngle = 0;

		transform.position = startingPosition;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		distanceTraveled = 0;
		lastPosition = transform.position;
		uiController.UpdateNetworkText(evolutionManager.CurrenGeneration, evolutionManager.CurrenGenome + 1);
		distZeroCounter = 0;
		
		wheels[0].brakeTorque = 0;
		wheels[1].brakeTorque = 0;
		wheels[2].brakeTorque = 0;
		wheels[3].brakeTorque = 0;
	}

	private bool HasCrashed()
	{
		return input[0] < 0.1f || input[1] < 0.1f || input[2] < 0.1f;
	}
}
