using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : BaseUnit
{
    BaseUnit _target;
    public override void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        maxHealth = 18;

        base.Setup(TeamColor, unitColor, unitManager);


        movement = new Vector3Int(0, 7, 0);
        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Ranged");
        gameObject.AddComponent<BoxCollider>();
    }

    public override void CheckForEnemies()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 25f , Vector3.down);
        foreach (RaycastHit Hit in hit)
        {
            if (Hit.transform.gameObject.layer != transform.gameObject.layer)
            {
                BaseUnit target = Hit.transform.gameObject.GetComponent<BaseUnit>();
                if (target != null)
                {
                    _target = target;
                    TransitionToState(attackState);
                }
            }
        }
    }

    public override void Attack()
    {
        if (!_target.gameObject.activeSelf) //if gameObject has been set to deactive
            TransitionToState(idleState); //go back to idle
        else
            _target.TakeDamage(3); //attack
    }
}
