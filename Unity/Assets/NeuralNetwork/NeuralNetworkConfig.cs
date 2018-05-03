using UnityEngine;

namespace AI.NeuralNetwork
{
	public class NeuralNetworkConfig : ScriptableObject
	{

		public ActivationFunction.FunctionType[] activationFunctions;
		public int[] neurons;
		public float bias;
	}

}
