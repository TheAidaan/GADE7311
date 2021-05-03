using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : BaseUnit
{ 
    List<BaseUnit> _targets = new List<BaseUnit>();
    public override void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        maxHealth = 15;

        base.Setup(TeamColor, unitColor, unitManager);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Wizard");
        gameObject.AddComponent<BoxCollider>();
    }

    #region Movement
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
        
        if (tileState != TileState.Taken && tileState != TileState.OutOfBounds)
            highlightedTiles.Add(currentTile.board.allTiles[targetX, targetY]);

    }

    protected override void CheckPath()
    {
        CreateTilePath(1); // top half
        CreateTilePath(-1);//bottom half
    }
    #endregion

    public override void CheckForEnemies()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 15f, Vector3.down);
        foreach (RaycastHit Hit in hit)
        {
            if(Hit.transform.gameObject.layer != transform.gameObject.layer)
            {
                BaseUnit target = Hit.transform.gameObject.GetComponent<BaseUnit>();
                if (target != null)
                {
                    _targets.Add(target);
                    TransitionToState(attackState);
                }
            }         
        } 
    }

    public override void Attack()
    {
        List<BaseUnit> newTargets = new List<BaseUnit>(); 

        foreach (BaseUnit target in _targets)
        {
            if (target.gameObject.activeSelf) //if gameObject is still active
            {
                newTargets.Add(target);
                target.TakeDamage(4);       
            }     
        }

        _targets = newTargets; // i wanted to remove allthe inactive fromt the main _targets list in the foreach loop but, i got an error
        
        if (_targets.Count == 0) // if all targets have been removed
        {
            TransitionToState(idleState);
        }
    }
}
