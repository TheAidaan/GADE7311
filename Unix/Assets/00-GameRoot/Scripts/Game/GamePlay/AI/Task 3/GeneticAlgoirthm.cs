using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgoirthm : AI
{
    // Start is called before the first frame update

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
        Debug.Log("Genetic");

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
}

