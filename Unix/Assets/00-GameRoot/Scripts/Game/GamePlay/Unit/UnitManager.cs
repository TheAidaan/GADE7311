using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    GameObject _unitPrefab;

    MiniMax _minMax = null;

    char[] _unitOrder = new char[12]
    {
                'R','M','W','M','R','M',
                'M','R','W','R','M','M'
    };

    Dictionary<char, Type> _unitLibrary = new Dictionary<char, Type>()
    {
        {'R', typeof(RangedUnit) },
        {'M', typeof(MeleeUnit) },
        {'W', typeof(WizardUnit) },

    };


    private void Awake()
    {
        _unitPrefab = Resources.Load<GameObject>("Prefabs/BaseUnit");
    }

    #region Unit setup

    public void Setup(Board board)
    {
        GameData.STATIC_SetRedUnits( CreateUnits(Color.red, new Color32(210, 95, 64, 255), board));

        GameData.STATIC_SetBlueUnits(CreateUnits(Color.blue, new Color32(80, 124, 159, 255) , board));


        PlaceUnits(1, 0, GameData.redUnits, board);
        PlaceUnits(GameData.boardLength - 2, GameData.boardLength - 1, GameData.blueUnits, board);


        if (GameData.loadMinMaxScript)
        {
            _minMax = GetComponent<MiniMax>();
            _minMax.AssignUnits();
        }

        SwitchSides(Color.red);
    }

    List<BaseUnit> CreateUnits(Color teamColor, Color32 unitColor,Board board)
    {
        List<BaseUnit> newUnits = new List<BaseUnit>();

        for(int i = 0; i< _unitOrder.Length; i++)
        {
            Vector3 rotation = teamColor == Color.red ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

            // instantiate the new unit
            GameObject newUnitObject = Instantiate(_unitPrefab, transform);
            newUnitObject.transform.eulerAngles = rotation;

            //identify the new unit
            char key = _unitOrder[i];
            Type unitType = _unitLibrary[key];

            //store the new unit
            BaseUnit newUnit = (BaseUnit)newUnitObject.AddComponent(unitType);
            newUnits.Add(newUnit);

            //setup peice
            newUnit.Setup(teamColor, unitColor,key);
        }

        return newUnits;
    }

    void PlaceUnits(int secondRow,int firstRow,List<BaseUnit> units, Board board)
    {
        int offset = GameData.generateBoard ? 2 : 1;

        for (int i = 0; i < 6; i++)
        {
            units[i].Place(board.allTiles[i+ offset, secondRow]);

            units[i+6].Place(board.allTiles[i+ offset, firstRow]);
        }
    }

    #endregion

    #region Unit controls

    void SetInteractive(List<BaseUnit> allUnits, bool value)
    {
        string tag = value ? "Interactive" : "Untagged";

        foreach (BaseUnit unit in allUnits)
            if (unit.gameObject.activeSelf)
                unit.gameObject.tag = tag;         
    }

    public void SwitchSides(Color color)
    {

        bool isRedTurn = color == Color.red ? true : false;
        
        //set the interactivity
        SetInteractive(GameData.redUnits, !isRedTurn);
        SetInteractive(GameData.blueUnits, isRedTurn);

        if (GameData.aiColor != color) // the player just went and it is the players turn    
            if(_minMax != null)
            {
                if(GameData.aiColor == Color.red) 
                  SetInteractive(GameData.redUnits, false);
                else 
                  SetInteractive(GameData.blueUnits, false);


                _minMax.Play();
            }
                
    }

   
    
    #endregion

}
