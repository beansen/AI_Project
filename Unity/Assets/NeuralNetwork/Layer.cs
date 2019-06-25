using Random = UnityEngine.Random;


namespace AI.NeuralNetwork
{
	public class Layer
	{
		private float[] neurons;

		private float[] weights;

		private float bias;

		private ActivationFunction.ActivationFct ActivationFunction;

		public float[] Neurons
		{
			get { return neurons; }
		}

		public float[] Weights
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
			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < neurons.Length; j++)
				{
					if (i == 0)
					{
						neurons[j] = weights[j] * input[i];
					}
					else
					{
						neurons[j] += weights[i * neurons.Length + j] * input[i];
					}
				}
			}
			
			for (int i = 0; i < neurons.Length; i++)
			{
				neurons[i] += bias * weights[weights.Length - 1];
				neurons[i] = ActivationFunction(neurons[i]);
			}
		}

		public void SetInput(float[] input)
		{
			neurons = input;
		}
 
		public void SetRandomWeights(uint inputLayer, float minWeight, float maxWeight)
		{
			weights = new float[inputLayer * neurons.Length + 1];
		
			for (int i = 0; i < weights.Length; i++)
			{
				weights[i] = Random.Range(minWeight, maxWeight);
			}
		}

		public void SetActivationFunction(ActivationFunction.ActivationFct activationFct)
		{
			this.ActivationFunction = activationFct;
		}
	}
}
