using Random = UnityEngine.Random;


namespace AI.NeuralNetwork
{
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
			for (int y = 0; y < weights.GetLength(1); y++)
			{
				float inputValue = y != 0 ? input[y - 1] : bias;
			
				for (int x = 0; x < weights.GetLength(0); x++)
				{
					float weight = weights[x, y];

					neurons[x] += inputValue * weight;
				
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
			weights = new float[neurons.Length, inputLayer + 1];
		
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
}
