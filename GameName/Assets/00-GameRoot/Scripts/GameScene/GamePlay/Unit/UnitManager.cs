using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public bool gameOver;

    public GameObject unitPrefab;

    List<BaseUnit> Player1Units = null;
    List<BaseUnit> Player2Units = null;

    string[] _unitOrder = new string[12]
    {
                "R","M","W","M","R",
            "M","M","R","W","R","M","M"
    };

    Dictionary<string, Type> unitLibrary = new Dictionary<string, Type>()
    {
        {"R", typeof(RangedUnit) },
        {"M", typeof(MeleeUnit) },
        {"W", typeof(WizardUnit) },

    };

    #region Unit setup
    public void Setup(Board board)
    {
        //Create Player1 units
        Player1Units = CreateUnits(Color.red, new Color32(80, 124, 159, 255), board);

        //Create Player2 units
        Player2Units = CreateUnits(Color.blue, new Color32(210, 95, 64, 255), board);

        PlaceUnits(1, 0, Player1Units, board);
        PlaceUnits(6, 7, Player2Units, board);

        SwitchSides(Color.red);
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
            string key = _unitOrder[i];
            Type unitType = unitLibrary[key];

            //store the new unit
            BaseUnit newUnit = (BaseUnit)newUnitObject.AddComponent(unitType);
            newUnits.Add(newUnit);

            //setup peice
            newUnit.Setup(teamColor, unitColor, this);
        }

        return newUnits;
    }

    void PlaceUnits(int pawnRow,int royaltyRow,List<BaseUnit> units, Board board)
    {
        for (int i = 0; i < 6; i++)
        {
            units[i].Place(board.allTiles[i, pawnRow]);

            units[i+6].Place(board.allTiles[i, royaltyRow]);
        }
    }

    #endregion

    #region Unit controls

    void SetInteractive(List<BaseUnit> allUnits, bool value)
    {
        string tag = value ? "Interactive" : "Untagged";

        foreach (BaseUnit unit in allUnits)
            unit.gameObject.tag = tag;
    }
    public void SwitchSides(Color color)
    {
        if (gameOver)
        {

            ResetUnits();//reset pieces to go back to where they cam from and keep board the same

            
            gameOver = false;//game is not over anymore

            color = Color.red; //let the whiteteam go first

        }

        bool isRedTurn = color == Color.red ? true : false;

        //set the interactivity
        SetInteractive(Player1Units, !isRedTurn);
        SetInteractive(Player2Units, isRedTurn);
    }

    public void ResetUnits()
    {
        foreach (BaseUnit unit in Player1Units) //reset player 1 units
            unit.Reset();

        foreach (BaseUnit unit in Player2Units)//reset player 1 units
            unit.Reset();
    }
    #endregion
}
