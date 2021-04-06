using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    Unit _squareUnit = new Unit();
    // Start is called before the first frame update
    void Start()
    {
        _squareUnit.unitName = "cirlce";
        _squareUnit.unitLevel = 1;
        _squareUnit.currentHP = _squareUnit.maxHP = 20;
        _squareUnit.damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
