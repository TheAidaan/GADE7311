using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public BaseUnit unit;
    public Tile target;

    public Move(BaseUnit Unit, Tile Target)
    {
        unit = Unit;
        target = Target;
    }
}

public abstract class AI : MonoBehaviour
{
    protected List<BaseUnit> otherUnits, aiUnits;
    protected Color teamColor;
    public Move bestMove;

    public virtual void AssignUnits()
    {
        int rand = Random.Range(0, 1);

        otherUnits = rand == 0 ? GameData.blueUnits : GameData.redUnits;
        aiUnits = rand == 0 ? GameData.redUnits : GameData.blueUnits;
        teamColor = rand == 0 ? Color.red : Color.blue;

        if (!GameData.aiBattle)
        {
            Color playerColor = teamColor == Color.red ? Color.blue : Color.red;
            GameData.STATIC_SetPlayerColor(playerColor);

        }
    }

    protected List<Tile> CheckValidMoves(BaseUnit unit)
    {

        if (!unit.gameObject.activeSelf)
            return null;

        unit.CheckPath();

        if (unit.highlightedTiles.Count == 0)
            return null;

        return unit.highlightedTiles;
    }

    public virtual void Play() { GameManager.play -= Play; }
}
