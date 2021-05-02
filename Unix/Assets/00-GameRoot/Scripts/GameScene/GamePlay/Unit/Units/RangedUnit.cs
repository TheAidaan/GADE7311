using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        base.Setup(TeamColor, unitColor, unitManager);


        movement = new Vector3Int(0, 7, 0);
        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Ranged");
        gameObject.AddComponent<BoxCollider>();
    }
}
