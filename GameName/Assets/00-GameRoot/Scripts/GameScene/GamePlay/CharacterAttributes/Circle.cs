using UnityEngine;
public class Circle : MonoBehaviour
{
    void Awake()
    {
        BaseUnit circleUnit = new BaseUnit();
        circleUnit.unitName = "cirlce";
        circleUnit.unitLevel = 1;
        circleUnit.currentHP = circleUnit.maxHP = 20;
        circleUnit.damage = 2;

        GetComponentInParent<Unit>().AssignCharacterUnit(circleUnit);
    }
}
