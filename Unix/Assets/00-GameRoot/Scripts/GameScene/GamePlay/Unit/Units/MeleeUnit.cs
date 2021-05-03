using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : BaseUnit
{
    BaseUnit _target;
    public override void Setup(Color TeamColor, Color32 unitColor)
    {
        maxHealth = 20;
        base.Setup(TeamColor, unitColor);

        movement = new Vector3Int(1, 1, 1);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Melee");
        gameObject.AddComponent<BoxCollider>();
    }

    public override void CheckForEnemies()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 15f, Vector3.down);
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
        if(!_target.gameObject.activeSelf) //if gameObject has been set to deactive
            TransitionToState(idleState); //go back to idle
        else
            _target.TakeDamage(2); //attack
    }
}
