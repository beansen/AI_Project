
using UnityEngine;

namespace AI.NeuralNetwork
{
	public class ActivationFunction
	{
		public delegate float ActivationFct(float input);

		public static ActivationFct GetActivationFct(FunctionType functionType)
		{
			switch (functionType)
			{
				case FunctionType.Binary:
					return BinaryFunction;
				
				case FunctionType.Linear:
					return LinearFunction;
				
				case FunctionType.Logistic:
					return LogisticFunction;
				
				case FunctionType.Sigmoid:
					return SigmoidFunction;
			}

			return null;
		}

		private static float BinaryFunction(float input)
		{
			return input < 0 ? 0 : 1;
		}
	
		private static float LinearFunction(float input)
		{
			return input;
		}
	
		private static float LogisticFunction(float input)
		{
			return 1 / (1 + Mathf.Exp(-input));
		}
	
		private static float SigmoidFunction(float input)
		{
			return 2 / (1 + Mathf.Exp(-2 * input)) - 1;
		}

		public enum FunctionType
		{
			Binary,
			Linear,
			Logistic,
			Sigmoid
		}
	}

}
