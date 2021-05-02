using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        base.Setup(TeamColor, unitColor, unitManager);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Wizard");
        gameObject.AddComponent<BoxCollider>();
    }
    void CreateTilePath(int flipper) //different to the BaseUnits createTilePath
    {
        int currentX = currentTile.boardPosition.x;
        int currentY = currentTile.boardPosition.z;// z represents the world point, but it also represents the y point in the 2D array. 

        //flipper represents the point where the movement option turns to make the L shape in the various directions
        MatchesStates(currentX - 2, currentY + (1 * flipper));//left
        MatchesStates(currentX - 1, currentY + (2 * flipper));//Upper left
        MatchesStates(currentX + 1, currentY + (2 * flipper));//Upper right
        MatchesStates(currentX + 2, currentY + (1 * flipper));//right
    }

    void MatchesStates(int targetX, int targetY)
    {
        TileState tileState = TileState.None;
        tileState = currentTile.board.ValidateTile(targetX, targetY, this);
        
        if (tileState != TileState.Friendly && tileState != TileState.OutOfBounds)
            highlightedTiles.Add(currentTile.board.allTiles[targetX, targetY]);

    }

    protected override void CheckPath()
    {
        CreateTilePath(1); // top half
        CreateTilePath(-1);//bottom half
    }
}
