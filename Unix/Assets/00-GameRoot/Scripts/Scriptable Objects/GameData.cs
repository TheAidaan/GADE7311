using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : SingletonScriptableObject<GameData>
{
    static int _minMaxDepth;
    public static int minMaxDepth { get { return _minMaxDepth; } }

    static bool _loadMinMaxScript;
    public static bool loadMinMaxScript { get { return _loadMinMaxScript; } }

    List<BaseUnit> _redTeamUnits = null;
    List<BaseUnit> _blueTeamUnits = null;

    void SetMinMaxDepth(int depth)
    {
        _minMaxDepth = depth;
    }
    void LoadMinMaxScript(bool load)
    {
        _loadMinMaxScript = load;
    }


    /*                  STATICS                  */

    public static void STATIC_SetMinMaxDepth(int depth)
    {
        instance.SetMinMaxDepth(depth);
    }
    public static void STATIC_LoadMinMaxScript(bool load)
    {
        instance.LoadMinMaxScript(load);
    }
}
