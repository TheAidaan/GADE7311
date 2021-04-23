using UnityEngine;

public class Square : MonoBehaviour
{
    void Awake()
    {
        BaseUnit squareUnit = new BaseUnit();
        squareUnit.unitName = "cirlce";
        squareUnit.unitLevel = 1;
        squareUnit.currentHP = squareUnit.maxHP = 20;

        squareUnit.damage = 2; 
        
        GetComponentInParent<Unit>().AssignCharacterUnit(squareUnit);
    }
}
