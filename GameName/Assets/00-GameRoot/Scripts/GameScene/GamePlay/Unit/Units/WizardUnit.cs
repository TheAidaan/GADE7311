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
}
