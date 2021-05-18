using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public GameObject unitPrefab;

    List<BaseUnit> _redTeamUnits = null;
    List<BaseUnit> _blueTeamUnits = null;

    char[] _unitOrder = new char[12]
    {
                'R','M','W','M','R','M',
                'M','R','W','R','M','M'
    };

    Dictionary<char, Type> _unitLibrary = new Dictionary<char, Type>()
    {
        {'M', typeof(MeleeUnit)  },
        {'R', typeof(RangedUnit) },
        {'W', typeof(WizardUnit) },

    };

    #region Unit setup

    public void Setup(Board board)
    {
        //Create Player1 units
        _redTeamUnits = CreateUnits(Color.red, new Color32(210, 95, 64, 255), board);

        //Create Player2 units
        _blueTeamUnits = CreateUnits(Color.blue, new Color32(80, 124, 159, 255) , board);

        PlaceUnits(1, 0, _redTeamUnits, board);
        PlaceUnits(6, 7, _blueTeamUnits, board);

        if (GameManager.singlePlayer)
        {
            //List<BaseUnit> allUnits;
            //allUnits = _blueTeamUnits;
            //allUnits.AddRange(_redTeamUnits);
            GameManager.Static_SendAIUnits(_redTeamUnits, Color.red,_blueTeamUnits);
        }

        SwitchSides(Color.red); //blue always starts the game
    }

    List<BaseUnit> CreateUnits(Color teamColor, Color32 unitColor,Board board)
    {
        List<BaseUnit> newUnits = new List<BaseUnit>();

        for(int i = 0; i< _unitOrder.Length; i++)
        {
            Vector3 rotation = teamColor == Color.red ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

            // instantiate the new unit
            GameObject newUnitObject = Instantiate(unitPrefab, transform);
            newUnitObject.transform.eulerAngles = rotation;

            //identify the new unit
            char key = _unitOrder[i];
            Type unitType = _unitLibrary[key];

            //store the new unit
            BaseUnit newUnit = (BaseUnit)newUnitObject.AddComponent(unitType);
            newUnits.Add(newUnit);

            //setup peice
            newUnit.Setup(teamColor, unitColor, key);
        }

        return newUnits;
    }

    void PlaceUnits(int secondRow,int firstRow,List<BaseUnit> units, Board board)
    {
        for (int i = 0; i < 6; i++)
        {
            units[i].Place(board.allTiles[i, secondRow]);

            units[i+6].Place(board.allTiles[i, firstRow]);
        }
    }

    #endregion

    #region Unit controls

    void SetInteractive(List<BaseUnit> allUnits, bool value)
    {
        string tag = value ? "Interactive" : "Untagged";

        foreach (BaseUnit unit in allUnits)
        {
            if (unit.gameObject.activeSelf)
            {
                unit.gameObject.tag = tag;
            }
        }
            
    }

    public void SwitchSides(Color color)
    {
        bool isRedTurn = color == Color.red ? true : false;

        //set the interactivity
        SetInteractive(_redTeamUnits, !isRedTurn);
        SetInteractive(_blueTeamUnits, isRedTurn);
    }

    #endregion

    //List<BaseUnit> AvailableMoves()
    //{
    //    List<BaseUnit> availableMoves = new List<BaseUnit>();

    //    foreach (BaseUnit unit in _redTeamUnits)
    //    {
    //        if (!unit.gameObject.activeSelf)
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            unit.CheckPath();
    //            if (unit.highlightedTiles.Count == 0)
    //                continue;
    //        }
    //        availableMoves.Add(unit);
    //    }
    //}
}
