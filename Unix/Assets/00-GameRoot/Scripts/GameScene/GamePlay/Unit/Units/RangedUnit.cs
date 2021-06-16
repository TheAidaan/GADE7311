using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, char CharacterCode)
    {
        maxHealth = 18;
        coolDown = 3f;

        base.Setup(TeamColor, unitColor,CharacterCode);


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
                BaseUnit Target = Hit.transform.gameObject.GetComponent<BaseUnit>();
                if (Target != null)
                {
                    target = Target;
                    targetPos = Target.transform.position;
                    TransitionToState(attackState);
                    break;
                }
            }
        }
    }



    public override void Attack()
    {
        bool canAttack = CheckAttackValidity();

        if (canAttack)
        {
            StartCoroutine(target.TakeDamage(2));             //attack 
        }
        else
        {
            TransitionToState(idleState); //go back to idle
        }

    }
}
