using System.Collections.Generic;
using UnityEngine;

namespace AI.Evolution
{
	using NeuralNetwork;

	public class EvolutionManager : MonoBehaviour
	{
		public uint[] topology;
	
		[Range(0f, 1f)]
		public float mutationPossibility;
		[Range(0.001f, 0.2f)]
		public float mutationRate;
	
		public int maxGenerations;
		public int population;
	
		[Range(-1f, 1f)]
		public float bias;
	
		public ActivationFunction.FunctionType activationFunction;

		public NeuralNetworkConfig config;
	
		private int generationCounter = 1;
		private int populationIndex;
	
		private List<NeuralNetwork> currentPopulation;
	
		public int CurrenGeneration
		{
			get { return generationCounter; }
		}
		
		public int CurrenGenome
		{
			get { return populationIndex; }
		}
	
		void Start()
		{
			currentPopulation = new List<NeuralNetwork>(population);
	
			for (int i = 0; i < population; i++)
			{
				if (config)
				{
					currentPopulation.Add(new NeuralNetwork(config));
				}
				else
				{
					currentPopulation.Add(new NeuralNetwork(activationFunction, bias, topology));
				}
			}
		}
	
		public float[] GetOutput(float[] input)
		{
			currentPopulation[populationIndex].ProcessInput(input);
			return currentPopulation[populationIndex].Output;
		}
	
		public void NextNetwork(float distanceTraveled)
		{
			currentPopulation[populationIndex].Fitness = distanceTraveled;
			populationIndex++;
	
			if (populationIndex == population)
			{
				populationIndex = 0;
				EvolveCurrentPopulation();
			}
		}
	
		private void EvolveCurrentPopulation()
		{
			currentPopulation.Sort((networkA, networkB) => networkA.Fitness.CompareTo(networkB.Fitness));
			currentPopulation.RemoveRange(4, population - 4);
			
			BreedNewGeneration();
			MutateNewGeneration();
			generationCounter++;
		}
	
		private void BreedNewGeneration()
		{
			while (currentPopulation.Count < population)
			{
				int parentOneIndex = Random.Range(0, 4);
				int parentTwoIndex = Random.Range(0, 4);
	
				if (parentOneIndex != parentTwoIndex)
				{
					currentPopulation.Add(BreedNewNetwork(currentPopulation[parentOneIndex], currentPopulation[parentTwoIndex]));
				}
			}
		}
	
		private NeuralNetwork BreedNewNetwork(NeuralNetwork parentOne, NeuralNetwork parentTwo)
		{
			NeuralNetwork child;

			if (config)
			{
				child = new NeuralNetwork(config);
			}
			else
			{
				child = new NeuralNetwork(activationFunction, bias, topology);
			}
	
			for (int i = 1; i < child.Layers.Length; i++)
			{
				child.Layers[i].Weights = Random.Range(0, 2) == 0 ? parentOne.Layers[i].Weights : parentTwo.Layers[i].Weights;
			}
			
			return child;
		}
	
		private void MutateNewGeneration()
		{
			for (int i = 4; i < currentPopulation.Count; i++)
			{
				NeuralNetwork child = currentPopulation[i];
	
				for (int j = 1; j < child.Layers.Length; j++)
				{
					MutateLayer(child.Layers[j].Weights);
				}
			}
		}
	
		private void MutateLayer(float[] layerWeights)
		{
			for (int i = 0; i < layerWeights.Length; i++)
			{
				if (Random.Range(0f, 1f) < mutationPossibility)
				{
					layerWeights[i] += Random.Range(-mutationRate, mutationRate);
				}
			}
		}
	}
}
