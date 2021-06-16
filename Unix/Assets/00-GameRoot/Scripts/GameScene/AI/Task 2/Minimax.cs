using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    List<BaseUnit> _playerUnits;
    List<BaseUnit> _aiUnits;
    
    // Start is called before the first frame update
    void Start()
    {
        AssignUnits();
        
    }

    public void Play()
    {
        RandomMover();
    }

    void RandomMover()
    {
        BaseUnit unit = _aiUnits[Random.Range(0, _aiUnits.Count)];

        if (!unit.gameObject.activeSelf)
            RandomMover();
        else
        {
            unit.CheckPath();
            if (unit.highlightedTiles.Count == 0)
                RandomMover();
            else
                unit.Move(unit.highlightedTiles[Random.Range(0, unit.highlightedTiles.Count)]);
        }

    }
    void AssignUnits()
    {
        int rand = Random.Range(0, 1);

        _playerUnits = rand == 0 ?  UnitManager.Static_GetBlueTeamUnits() : _playerUnits = UnitManager.Static_GetRedTeamUnits();
        _aiUnits = rand == 0 ? UnitManager.Static_GetRedTeamUnits() : _playerUnits = UnitManager.Static_GetBlueTeamUnits();
        Color aiColor = rand == 0 ? Color.red : Color.blue;
        UnitManager.Static_SetMinMax(this,aiColor);
    }
}
