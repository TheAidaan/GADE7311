using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    Unit _circleUnit = new Unit();
    // Start is called before the first frame update
    void Start()
    {
        _circleUnit.unitName = "cirlce";
        _circleUnit.unitLevel = 1;
        _circleUnit.currentHP = _circleUnit.maxHP = 20;
        _circleUnit.damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
