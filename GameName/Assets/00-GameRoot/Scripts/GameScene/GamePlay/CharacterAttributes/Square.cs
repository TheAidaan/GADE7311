using UnityEngine;

public class Square : MonoBehaviour
{
    void Start()
    {
        BaseUnit squareUnit = new BaseUnit();
        squareUnit.unitName = "cirlce";
        squareUnit.unitLevel = 1;
        squareUnit.currentHP = squareUnit.maxHP = 20;

        squareUnit.teamID = 0;

        squareUnit.damage = 2; 
        
        GetComponentInParent<Unit>().AssignCharacterUnit(squareUnit);
        GetComponentInParent<Unit>().AddToTeam();
    }
}
