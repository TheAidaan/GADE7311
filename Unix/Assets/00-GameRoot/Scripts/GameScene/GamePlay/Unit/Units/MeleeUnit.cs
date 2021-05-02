using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        base.Setup(TeamColor, unitColor, unitManager);

        movement = new Vector3Int(1, 1, 1);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Melee");
        gameObject.AddComponent<BoxCollider>();
    }
}
