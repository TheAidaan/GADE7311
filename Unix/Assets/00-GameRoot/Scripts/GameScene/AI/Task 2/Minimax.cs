using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    List<BaseUnit> _playerUnits;
    List<BaseUnit> _aiUnits;

    Color _aiTeamColor;

    Dictionary<char, double> _evaluationScoreLibrary = new Dictionary<char, double>()
    {
        {'M', 1 },  
        {'R', 2 },  
        {'W', 3 },  
        
    };

    // Start is called before the first frame update
    void Start()
    {
        AssignUnits();
        
    }
    void AssignUnits()
    {
        int rand = Random.Range(0, 1);

        //_playerUnits = rand == 0 ? UnitManager.Static_GetBlueTeamUnits() : _playerUnits = UnitManager.Static_GetRedTeamUnits();
        //_aiUnits = rand == 0 ? UnitManager.Static_GetRedTeamUnits() : _playerUnits = UnitManager.Static_GetBlueTeamUnits();
        //_aiTeamColor = rand == 0 ? Color.red : Color.blue;


        _aiTeamColor = Color.red;
        _aiUnits = UnitManager.Static_GetRedTeamUnits();
        _playerUnits = UnitManager.Static_GetBlueTeamUnits();

        UnitManager.Static_SetMinMax(this, _aiTeamColor);
    }

    IEnumerator Evaluate(BaseUnit unit)
    {
        BaseUnit singletarget = null;
        List<BaseUnit> targets = new List<BaseUnit>();
        bool testattacked=false;

        unit.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        double evaluation = 0;
        bool simpleEval = unit.characterCode == 'M' || unit.characterCode == 'R' ? true : false;

       
        if (simpleEval)
        {
            singletarget = unit.CheckForEnemy();
            if (singletarget != null)
            {
                singletarget.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

                evaluation += _evaluationScoreLibrary[singletarget.characterCode];
                testattacked = true;
            }   
            
        }else
        {
            targets = unit.CheckForEnemies(true);

            foreach(BaseUnit target in targets)
            {
                testattacked = true;
                target.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                evaluation += _evaluationScoreLibrary[target.characterCode];
            }            
        }

        int perspective = unit.teamColor == _aiTeamColor ? 1 : -1; //the AI wants a high evaluation for itself and a low evaluation fot the player;
        evaluation *= perspective;

        if (testattacked)
        {
            Debug.Log(evaluation);
            Time.timeScale = 0;
        }
        yield return new WaitForSeconds(1f);

        unit.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        if (testattacked)
        {
            if (singletarget!=null)
                singletarget.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            foreach (BaseUnit target in targets)
            {
                target.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

            }
        }

    }
    public void Play()
    {

        StartCoroutine(AI(1));
        //RandomMover();
    }

    IEnumerator AI(int depth)
    {
        GameManager.STATIC_SetAIEvaluationStatus(true);

        foreach(BaseUnit AIUnit in _aiUnits)
        {
            bool AIUnitCanMove = CheckValidMoves(AIUnit);

            if (AIUnitCanMove)
            {
                AIUnit.Move(AIUnit.highlightedTiles[0]);
                yield return StartCoroutine(Evaluate(AIUnit)); ;

                foreach (BaseUnit playerUnit in _playerUnits)
                {
                    
                    bool playerUnitCanMove = CheckValidMoves(playerUnit);

                    if (playerUnitCanMove)
                    {
                        playerUnit.Move(playerUnit.highlightedTiles[0]);
                        //yield return StartCoroutine(Evaluate(playerUnit)); ;

                        playerUnit.UndoMove();
                    }
                    else
                        continue;
                    

                }
                AIUnit.UndoMove();
            }
            else
            {
                continue;
            }

        }
        
        GameManager.STATIC_SetAIEvaluationStatus(false);
    }

    void RandomMover()
    {
        BaseUnit unit = _aiUnits[Random.Range(0, _aiUnits.Count)];

        bool unitCanMove = CheckValidMoves(unit);

        if (unitCanMove)
            unit.Move(unit.highlightedTiles[Random.Range(0, unit.highlightedTiles.Count)]);
        else
            RandomMover();

    }

    bool CheckValidMoves(BaseUnit unit)
    {
        if (!unit.gameObject.activeSelf)
            return false;

        unit.CheckPath();

        if (unit.highlightedTiles.Count == 0)
            return false;

        return true;
    }
   
}
