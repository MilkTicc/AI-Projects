using System;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class NeuroNetWork
	{
		//float [] weight1 = new float [7];
		//float [] weight2 = new float [7];
		//float [] weight3 = new float [7];
		//float [] weight4 = new float [7];
		//float [] weight5 = new float [7];
		//float [] weight6 = new float [7];
		//float [] weight7 = new float [7];
		//float [] weight9 = new float [7];
		//float [] weight8 = new float [7];
		//Neuro h1;
		//Neuro h2;
		//Neuro h3;
		//Neuro h4;
		//Neuro h5;
		//Neuro h6;
		//Neuro q1;
		//Neuro q2;
		//Neuro q3;
		//Neuro q4;
		//Neuro q5;
		//Neuro q6;
		////Neuro h7;
		//Neuro o1;
		//Neuro o2;

		public float[] NueroOutput (float [] weights, float [] inputs, int layerCount, int neuroCount)
		{
			int cut = 0;
			float [] tempInput = new float [neuroCount + 1];
			tempInput= LayerOutput (SubArray (weights, 0, inputs.Length * neuroCount), inputs, neuroCount);
			cut += inputs.Length * neuroCount;
			//for (int i = 0; i < layerCount; i++) {
			//	float [] tempInput = new float [neuroCount + 1];
			//	tempInput = LayerOutput(SubArray(weights,cut,layer1Input))
			//}
			
			while(layerCount > 1){
				layerCount--;
				tempInput = LayerOutput (SubArray (weights, cut, tempInput.Length * neuroCount), tempInput, neuroCount);
				cut += tempInput.Length * neuroCount;
			}

			float[] output = new float [3];
			output = LayerOutput (SubArray (weights, cut, tempInput.Length * 2), tempInput, 2);
			return output;
		}

		public float [] LayerOutput (float [] layerWeights, float [] inputs, int neuroCount)
		{
			//float [] layerOutput = new float [inputs.Length + 1];
			float [] layerOutput = new float [neuroCount+1];
			for (int i = 0; i < neuroCount; i++) {
				layerOutput [i] = new Neuro (inputs, SubArray (layerWeights, i * inputs.Length, inputs.Length)).NeuroResult ();
			}
			layerOutput [neuroCount] = -1f;
			return layerOutput;
		}
		//	float[] weight1 = SubArray (weights, 0, 7);
		//	float[] weight2 = SubArray (weights, 7, 7);
		//	float[] weight3 = SubArray (weights, 14, 7);
		//	float[] weight4 = SubArray (weights, 21, 7);
		//	float[] weight5 = SubArray (weights, 28, 7);
		//	float[] weight6 = SubArray (weights, 35, 7);
		//	float[] weight7 = SubArray (weights, 42, 7);
		//	float [] weight8 = SubArray (weights, 49, 7);
		//	float [] weight9 = SubArray (weights, 56, 7);
		//	float [] weight10 = SubArray (weights, 63, 7);
		//	float [] weight11= SubArray (weights, 70, 7);
		//	float [] weight12 = SubArray (weights, 77, 7);
		//	float [] weight13 = SubArray (weights, 84, 7);
		//	float [] weight14 = SubArray (weights, 91, 7);
		//	h1 = new Neuro (inputs, weight1);
		//	h2 = new Neuro (inputs, weight2);
		//	h3 = new Neuro (inputs, weight3);
		//	h4 = new Neuro (inputs, weight4);
		//	h5 = new Neuro (inputs, weight5);
		//	h6 = new Neuro (inputs, weight6);


		//	float [] hiddenLayer1Outputs = { h1.NeuroResult (), h2.NeuroResult (), h3.NeuroResult (), h4.NeuroResult (), h5.NeuroResult (), h6.NeuroResult (), -1f };
		//	q1 = new Neuro (hiddenLayer1Outputs, weight7);
		//	q2 = new Neuro (hiddenLayer1Outputs, weight8);
		//	q3 = new Neuro (hiddenLayer1Outputs, weight9);
		//	q4 = new Neuro (hiddenLayer1Outputs, weight10);
		//	q5 = new Neuro (hiddenLayer1Outputs, weight11);
		//	q6 = new Neuro (hiddenLayer1Outputs, weight12);

		//	float [] hiddenLayer2Outputs = { q1.NeuroResult (), q2.NeuroResult (), q3.NeuroResult (), q4.NeuroResult (), q5.NeuroResult (), q6.NeuroResult (), -
		//		1f };
		//	o1 = new Neuro (hiddenLayer2Outputs, weight13);
		//	o2 = new Neuro (hiddenLayer2Outputs, weight14);

		//	return new Vector2 (o1.NeuroResult(), o2.NeuroResult());
		//}

		//public Vector2 CalcResult (int inputCount, int layerCount, int neuroPerLayer, float[] weights, float[] inputs ){
		//	List<NeuroLayer> layers = new List<NeuroLayer> ();
		//	for (int i =0 ; i < layerCount; i++)
		//	{
		//		layers [i] = new NeuroLayer (neuroPerLayer);
		//	}
			 
		//	float [] tempInput = new float [neuroPerLayer];
		//	for (int n = 0; n < neuroPerLayer; n++)
		//	{
		//		tempInput[n] = layers [0].Neuros [n].NeuroResult (inputs, SubArray (weights, inputCount * n, inputCount));
		//	}
		//}


		float TanH (float x)
		{
			return (1 + Mathf.Exp (2 * x) / 1 - Mathf.Exp (2 * x));
		}

		public float[] SubArray(float[] array, int index, int length){
			float [] result = new float [length];
			Array.Copy (array, index, result, 0, length);
			return result;
		}

	}

	public class NeuroLayer{
		//int neuroCount;
		//Neuro [] neuros;
		//public Neuro[] Neuros { get { return neuros;} set { neuros = value;}}

		//public NeuroLayer (int neuroC, float[] weights, float[] inputs) {
		//	neuroCount = neuroC;
		//	neuros = new Neuro [neuroCount];
		//}

		public float [] SubArray (float [] array, int index, int length)
		{
			float [] result = new float [length];
			Array.Copy (array, index, result, 0, length);
			return result;
		}

		public float[] Output (float [] weights, float [] inputs) { 
			float [] layerOutput = new float [inputs.Length + 1];
			for (int i = 0; i < inputs.Length - 1; i++) {
				layerOutput [i] = new Neuro (inputs, SubArray (weights, i * inputs.Length, inputs.Length)).NeuroResult ();
			}
			layerOutput [inputs.Length] = -1f;
			return layerOutput;
		}

	}

	public class Neuro{

		float [] inputs;
		float [] weights;

		public Neuro(float [] input, float [] weight){
			inputs = input;
			weights = weight;
		}

		float TanH (float x)
		{
			return ((1 - Mathf.Exp (-2 * x)) / (1 + Mathf.Exp (-2 * x)));
		}

		float Sigmoid(float x){
			return 1f / (1f + Mathf.Exp (-1*x));
		}

		public float NeuroResult(){
			
			float sum = 0;
			for (int i = 0; i < inputs.Length;i++)
			{
				sum += inputs [i] * weights [i];
			}
			//return (Sigmoid (sum));
			return (TanH  (sum));
		}
	}


}