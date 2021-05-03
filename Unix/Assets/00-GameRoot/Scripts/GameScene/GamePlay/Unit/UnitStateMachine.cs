using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseState : BaseSate<BaseUnit>
{
    public override void EnterState(BaseUnit script)
    {
        Debug.Log("Unit State machine enter");
    }
    public override void Update(BaseUnit script)
    {
        Debug.Log("Unit State machine update");
    }
}

public class UnitAttackState : UnitBaseState
{
    public override void Update(BaseUnit script)
    {
        script.Attack(); // attack the enemy unit 
    }
}
public class UnitHoverState : UnitBaseState
{
    public override void EnterState(BaseUnit script)
    {
        script.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public override void Update(BaseUnit script)
    {
        script.Drag(); //drag the unit
    }
}
public class UnitIdleState : UnitBaseState
{
    public override void EnterState(BaseUnit script)
    {
        script.transform.position = script.currentTile.transform.position; // go back to where you came from
        script.HideHighlightedTiles();
        script.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    public override void Update(BaseUnit script)
    {
        script.CheckForEnemies();
    }
}
