using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

	public Text generation;
	public Text genome;
	public Text currentDistance;
	public Text maxDistance;

	public void UpdateNetworkText(int generation, int genome)
	{
		this.generation.text = String.Format("Generation: {0}", generation);
		this.genome.text = String.Format("Genome: {0}", genome);
	}

	public void UpdateCurrentDistance(float distance)
	{
		string dist = distance.ToString("0.00");
		currentDistance.text = String.Format("Current distance: {0}", dist);
	}

	public void UpdateMaxDistance(float distance)
	{
		string dist = distance.ToString("0.00");
		maxDistance.text = String.Format("Max distance: {0}", dist);
	}
}
