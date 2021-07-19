using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLearning : AI
{
    // Start is called before the first frame update
    int generation = 0;
    private int[] _layers = new int[] { 1, 10, 10, 1 };
    List<NeuralNetwork> _neuralNetwork;

    private void Start()
    {
        //InitialiseNeuralNetwork();
    }
    public override void AssignUnits()
    {
        

        if (GameData.minMaxColor == Color.white)
        {
            base.AssignUnits();
        }
        else
        {
            aiUnits = GameData.minMaxColor == Color.red ?  GameData.blueUnits : GameData.redUnits;
            teamColor = GameData.minMaxColor == Color.red ? Color.blue : Color.red;
        }

        GameData.STATIC_SetGeneticAIColor(teamColor);
    }
    public override void Play()
    {
        //_neuralNetwork.Sort();
        RandomMover();



        base.Play();

    }

    void RandomMover()
    {

        BaseUnit unit = aiUnits[Random.Range(0, aiUnits.Count)];

        if (!unit.gameObject.activeSelf)
            RandomMover();
        else
        {
            unit.CheckPath();
            if (unit.highlightedTiles.Count == 0)
                RandomMover();
            else
                unit.Move(unit.highlightedTiles[Random.Range(0, unit.highlightedTiles.Count)]);
        }

    }

    void InitialiseNeuralNetwork()
    {
        for (int i = 0; i < aiUnits.Count; i++)
        {
            NeuralNetwork network = new NeuralNetwork(_layers);
            network.Mutate();
            _neuralNetwork.Add(network);


        }
    }
}

