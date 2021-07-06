using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : SingletonScriptableObject<GameData>
{
    static int _minMaxDepth, _boardLength;
    public static int minMaxDepth { get { return _minMaxDepth; } }
    public static int boardLength { get { return _boardLength; } }

    static bool _loadMinMaxScript;
    public static bool loadMinMaxScript { get { return _loadMinMaxScript; } }

    static bool _generateBoard;
    public static bool generateBoard { get { return _generateBoard; } }

    static List<BaseUnit> _redUnits;
    public static List<BaseUnit> redUnits { get { return _redUnits; } }

    static List<BaseUnit> _blueUnits;
    public static List<BaseUnit> blueUnits { get { return _blueUnits; } }

    static Color _aiColor;
    public static Color aiColor { get { return _aiColor; } }

    void SetMinMaxDepth(int depth)
    {
        _minMaxDepth = depth;
    }

    void LoadMinMaxScript(bool load)
    {
        _loadMinMaxScript = load;
    }
    void SetBlueUnits(List<BaseUnit> units)
    {
        _blueUnits = units;
    }
    void SetRedUnits(List<BaseUnit> units)
    {
        _redUnits = units;
    }
    void SetAIColor(Color color)
    {
        _aiColor = color;
    }

    void SetBoardLength(int Length)
    {
        _boardLength = Length;
    }
    void GenerateBoard(bool generate)
    {
        _generateBoard = generate;

        _boardLength = _generateBoard ? 10:8;
    }


    /*                  STATICS                  */
    public static void STATIC_SetAIColor(Color color)
    {
        instance.SetAIColor(color);
    }
    public static void STATIC_SetBlueUnits(List<BaseUnit> units)
    {
        instance.SetBlueUnits(units);
    }
    public static void STATIC_SetRedUnits(List<BaseUnit> units)
    {
        instance.SetRedUnits(units);
    }
    public static void STATIC_SetMinMaxDepth(int depth)
    {
        instance.SetMinMaxDepth(depth);
    } 

    public static void STATIC_LoadMinMaxScript(bool load)
    {
        instance.LoadMinMaxScript(load);
    }

    public static void STATIC_GenerateBoard(bool generate)
    {
        instance.GenerateBoard(generate);
    }

    public static void STATIC_SetBoardLength(int length)
    {
        instance.SetBoardLength(length);
    }
}
