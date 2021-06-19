using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    bool _gameOver, _activeMinMax;

    public GameObject unitPrefab;

    Color _aiColor;
    Minimax _minimax = null;

    List<BaseUnit> _redTeamUnits = null;
    List<BaseUnit> _blueTeamUnits = null;

    int _blueTeamScore, _redTeamScore;

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
        instance = this;
    }

    #region Unit setup

    public static void Static_Setup(Board board)
    {
        instance.Setup(board);
        
    }
    void Setup(Board board)
    {
        //Create Player1 units
        _redTeamUnits = CreateUnits(Color.red, new Color32(210, 95, 64, 255), board);

        //Create Player2 units
        _blueTeamUnits = CreateUnits(Color.blue, new Color32(80, 124, 159, 255) , board);

        PlaceUnits(1, 0, _redTeamUnits, board);
        PlaceUnits(6, 7, _blueTeamUnits, board);

        SwitchSides(Color.red);

        _activeMinMax = false;
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
            newUnit.Setup(teamColor, unitColor,key);
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
    public static void Static_SwitchSides(Color color)
    {

            instance.SwitchSides(color);
        
    }
    void SwitchSides(Color color)
    {

        bool isRedTurn = color == Color.red ? true : false;

        //set the interactivity
        SetInteractive(_redTeamUnits, !isRedTurn);
        SetInteractive(_blueTeamUnits, isRedTurn);

        if (_aiColor != color) // the player just went and it is the players turn
        {
            if(_minimax != null) { _minimax.Play(); }

        }
    }
    void CheckGameState()
    {
        if (_gameOver)
        {
            ResetGame();
        }
    }
    void ResetGame()
    {
        ResetUnits();//reset pieces to go back to where they cam from and keep board the same

        _gameOver = false;//game is not over anymore
        _redTeamScore = _blueTeamScore = 0;
    }

    void ResetUnits()
    {
        foreach (BaseUnit unit in _redTeamUnits) //reset player 1 units
            unit.Reset();

        foreach (BaseUnit unit in _blueTeamUnits)//reset player 2 units
            unit.Reset();
    }
    public static void Static_UnitDeath(Color color)
    {
        instance.UnitDeath(color);
    }
    void UnitDeath(Color color)
    {
        if (color == Color.red)
            _blueTeamScore += 1;
        else
            _redTeamScore += 1;

        if (_redTeamScore == 12 || _blueTeamScore == 12)
            _gameOver = true;

        CheckGameState();
    }
    #endregion

    #region AI
    public void SetMinMax(Minimax minimax, Color aiColor)
    {
        _activeMinMax = true;
        _minimax = minimax;
        _aiColor = aiColor;
    }

    public List<BaseUnit> GetRedTeamUnits()
    {
        return _redTeamUnits;
    }
    public List<BaseUnit> GetBlueTeamUnits()
    {
        return _blueTeamUnits;
    }
    public static List<BaseUnit> Static_GetRedTeamUnits()
    {
        return instance.GetRedTeamUnits();
    }
    public static List<BaseUnit> Static_GetBlueTeamUnits()
    {
        return instance.GetBlueTeamUnits();
    }
    public static void Static_SetMinMax(Minimax minimax,Color aiColor) //is there an AI in this scene?
    {
        instance.SetMinMax(minimax,aiColor);
    }
    #endregion
}
