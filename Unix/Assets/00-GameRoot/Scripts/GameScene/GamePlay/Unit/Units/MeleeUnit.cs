using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : BaseUnit
{
    BaseUnit _target;

    public override void Setup(Color TeamColor, Color32 unitColor, char ChararcterCode)
    {
        maxHealth = 20;
        coolDown = 3f;
        base.Setup(TeamColor, unitColor, ChararcterCode);

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
                    transform.LookAt(target.transform);
                    TransitionToState(attackState);
                }
            }
        }
    }

    public override IEnumerator CoolDown()
    {
        if(!_target.gameObject.activeSelf) //if gameObject has been set to deactive
            TransitionToState(idleState); //go back to idle
        else
        {

            StartCoroutine(_target.TakeDamage(2));             //attack
        }
            

        yield return base.CoolDown();
    }
}
