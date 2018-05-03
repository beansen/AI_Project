using System;
using AI.NeuralNetwork;
using UnityEditor;
using UnityEngine;

public class NNConfigWindow : EditorWindow
{

	private bool showLayers;
	private bool useMultipleFunctions;

	private int layers;
	private ActivationFunction.FunctionType singleActivationFunction;
	private int[] neurons;
	private ActivationFunction.FunctionType[] activationFunctions;
	private float bias;
	
	private String configName;
	
	[MenuItem("AI/NeuralNetwork/Config")]
	public static void ShowWindow()
	{
		GetWindow<NNConfigWindow>();
	}
	
	private void OnGUI()
	{
		if (showLayers)
		{
			ShowConfig();
		}
		else
		{
			layers = EditorGUILayout.IntField("Layers", layers);
			if (GUILayout.Button("Create Layers"))
			{
				neurons = new int[layers];
				activationFunctions = new ActivationFunction.FunctionType[layers];
				showLayers = true;
			}
		}
		
	}

	private void ShowConfig()
	{
		bias = EditorGUILayout.Slider("Bias", bias, -1, 1);
		useMultipleFunctions = EditorGUILayout.Toggle("Multiple activation functions", useMultipleFunctions, GUILayout.Width(500));

		if (!useMultipleFunctions)
		{
			singleActivationFunction = (ActivationFunction.FunctionType) EditorGUILayout.EnumMaskField("Activation function", singleActivationFunction);
		}
		
		for (int i = 0; i < layers; i++)
		{
			String layerDesc;
			
			if (i == 0)
			{
				layerDesc = "Input layer";

			}
			else if (i == layers - 1)
			{
				layerDesc = "Output layer";
			}
			else
			{
				layerDesc = String.Format("Hidden layer {0}", i);
			}
			EditorGUILayout.LabelField(layerDesc);
			
			neurons[i] = EditorGUILayout.IntField("Neurons", neurons[i]);
			if (useMultipleFunctions)
			{
				activationFunctions[i] = (ActivationFunction.FunctionType) EditorGUILayout.EnumMaskField("Activation function", activationFunctions[i]);
			}
			EditorGUILayout.Space();
		}

		EditorGUILayout.Space();
		configName = EditorGUILayout.TextField("Config name", configName);
		EditorGUILayout.Space();

		if (GUILayout.Button("Save"))
		{
			NeuralNetworkConfig config = CreateInstance<NeuralNetworkConfig>();
			config.neurons = neurons;
			config.bias = bias;
			if (useMultipleFunctions)
			{
				config.activationFunctions = activationFunctions;
			}
			else
			{
				config.activationFunctions = new[] {singleActivationFunction};
			}
			
			AssetDatabase.CreateAsset(config, String.Format("Assets/{0}.asset", configName));
			AssetDatabase.SaveAssets();
			Close();
		}
	}
}
