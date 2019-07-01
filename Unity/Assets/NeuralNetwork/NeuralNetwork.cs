namespace AI.NeuralNetwork
{
	public class NeuralNetwork
	{

		private uint[] topology;
		private Layer[] layers;

		private float fitness;

		private float bias;

		private ActivationFunction.FunctionType functionType;

		private NeuralNetworkConfig config;
	
		public float Fitness
		{
			get { return fitness; }
			set { fitness = value; }
		}

		public float[] Output
		{
			get { return layers[layers.Length - 1].Neurons; }
		}

		public Layer[] Layers
		{
			get { return layers; }
		}

		public NeuralNetwork(ActivationFunction.FunctionType functionType, float bias, params uint[] topology)
		{
			this.functionType = functionType;
			this.bias = bias;
			this.topology = topology;
			layers = new Layer[topology.Length];
			InitSimple();
		}

		public NeuralNetwork(NeuralNetworkConfig config)
		{
			this.config = config;
			layers = new Layer[config.neurons.Length];
			InitWithConfig();
		}

		public void ProcessInput(float[] input)
		{
			layers[0].SetInput(input);
		
			for (int i = 1; i < layers.Length; i++)
			{
				layers[i].ProcessInput(layers[i - 1].Neurons);
			}
		}

		private void InitSimple()
		{
			for (int i = 0; i < topology.Length; i++)
			{
				layers[i] = new Layer(topology[i], bias);

				if (i != 0)
				{
					layers[i].SetRandomWeights(topology[i - 1], -0.8f, 0.8f);
					layers[i].SetActivationFunction(ActivationFunction.GetActivationFct(functionType));
				}
			}
		}

		private void InitWithConfig()
		{
			for (int i = 0; i < config.neurons.Length; i++)
			{
				layers[i] = new Layer((uint) config.neurons[i], config.bias);

				if (i != 0)
				{
					layers[i].SetRandomWeights((uint) config.neurons[i - 1], -0.8f, 0.8f);
					if (config.activationFunctions.Length > 1)
					{
						layers[i].SetActivationFunction(ActivationFunction.GetActivationFct(config.activationFunctions[i]));
					}
					else
					{
						layers[i].SetActivationFunction(ActivationFunction.GetActivationFct(config.activationFunctions[0]));
					}
				}
			}
		}
	}
}
