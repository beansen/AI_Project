
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Layer
{
	private float[] neurons;

	private float[,] weights;

	private float bias;

	private ActivationFunction.ActivationFct ActivationFunction;

	public float[] Neurons
	{
		get { return neurons; }
	}

	public float[,] Weights
	{
		get { return weights; }
		set { weights = value; }
	}

	public Layer(uint neuronsAmount, float bias)
	{
		neurons = new float[neuronsAmount];
		this.bias = bias;
	}

	public void ProcessInput(float[] input)
	{
		for (int y = -1; y < weights.GetLength(1); y++)
		{
			float inputValue = y != -1 ? input[y] : 0;
			
			for (int x = 0; x < weights.GetLength(0); x++)
			{
				float weight = y != -1 ? weights[x, y] : 0;

				if (y == -1)
				{
					neurons[x] = bias;
				}
				else
				{
					neurons[x] += inputValue * weight;
				}
				
				if (y == weights.GetLength(1) - 1)
				{
					neurons[x] = ActivationFunction(neurons[x]);
				}
			}
		}
	}

	public void SetInput(float[] input)
	{
		neurons = input;
	}

	public void SetRandomWeights(uint inputLayer, float minWeight, float maxWeight)
	{
		weights = new float[neurons.Length, inputLayer];
		
		for (int y = 0; y < weights.GetLength(1); y++)
		{
			for (int x = 0; x < weights.GetLength(0); x++)
			{
				weights[x, y] = Random.Range(minWeight, maxWeight);
			}
		}
	}

	public void SetActivationFunction(ActivationFunction.ActivationFct activationFct)
	{
		this.ActivationFunction = activationFct;
	}
}