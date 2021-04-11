using UnityEngine;
public class Circle : MonoBehaviour
{
    void Start()
    {
        BaseUnit circleUnit = new BaseUnit();
        circleUnit.unitName = "cirlce";
        circleUnit.unitLevel = 1;
        circleUnit.currentHP = circleUnit.maxHP = 2;
        circleUnit.damage = 2;

        circleUnit.teamID = 1;

        GetComponentInParent<Unit>().AssignCharacterUnit(circleUnit);
        GetComponentInParent<Unit>().AddToTeam();
    }
}
