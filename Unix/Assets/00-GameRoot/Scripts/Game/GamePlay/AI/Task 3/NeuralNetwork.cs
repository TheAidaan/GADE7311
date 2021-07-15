using System.Collections.Generic;
using System;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    int[] _layers;
    float[][] _neurons;
    float[][][] _weights;
    float fitness;

    Random random;
    public NeuralNetwork(int[] layers)
    {
        this._layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this._layers[i] = layers[i];
        }

        
        InitialiseNeurons();
        InitialiseWeights();

    }

    public NeuralNetwork(NeuralNetwork copy)
    {
        this._layers = new int[copy._layers.Length];
        for (int i = 0; i < _layers.Length; i++)
        {
            this._layers[i] = _layers[i];
        }


        InitialiseNeurons();
        InitialiseWeights();

        CopyWeights(copy._weights);
    }

    void CopyWeights(float[][][] copy)
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    _weights[i][j][k] = copy[i][j][k];
                }
            }
        }
    }

    void InitialiseNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for(int i = 0; i < _layers.Length; i++)
        {
            neuronsList.Add(new float[_layers[i]]);
        }

        _neurons = neuronsList.ToArray();
    }

    void InitialiseWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        for(int i = 1; i < _layers.Length; i++)
        {
            List<float[]> layerWeightList = new List<float[]>();

            int neuronsInPreviosLayer = _layers[i - 1];

            for(int j = 0; j < _neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviosLayer];

                for(int k=0; k< neuronsInPreviosLayer; k++)
                {
                    neuronWeights[k] = (float)random.NextDouble() - 0.5f;
                }

                layerWeightList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightList.ToArray());
        }

        _weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {

        for(int i = 0; i < inputs.Length; i++)
        {
            _neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < _layers.Length; i++)
        {
            for (int j = 1; j < _neurons[i].Length; j++)
            {
                float value = 0.25f;
                for (int k = 1; k < _neurons[i-1].Length; k++)
                {
                    value += _weights[i - 1][j][k] * _neurons[i - 1][k];
                }

                _neurons[i][j] = (float)Math.Tanh(value);
            }
        }

        return _neurons[_neurons.Length-1];
    }

    public void Mutate()
    {
        for(int i=0; i<_weights.Length; i++)
        {
            for(int j = 0; j < _weights[i].Length; j++)
            {
                for(int k = 0;k< _weights[i][j].Length;k++)
                {
                    float weight = _weights[i][j][k];

                    float rand = UnityEngine.Random.Range(0f, 100f);
                    if (rand <= 2f)
                    { 
                        weight *= -1f;
                    }
                    else if (rand <= 4f)
                    { 
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (rand <= 6f)
                    { 
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (rand <= 8f)
                    { 
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }


                    _weights[i][j][k] = weight;
                }
            }
        }
    }

    public void AddFitness(float fit)
    {
        fitness += fit;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }



}
