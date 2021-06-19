using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Move
{
    public double evaluation;
    public BaseUnit unit;
    public Tile target;

    public Move(double Evaluation, BaseUnit Unit, Tile Target)
    {
        evaluation = Evaluation;
        unit = Unit;
        target = Target;
    }
}
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

    double Evaluate(BaseUnit unit)
    {
        BaseUnit singletarget = null;
        List<BaseUnit> targets = new List<BaseUnit>();
    
        double evaluation  = double.PositiveInfinity;
        bool simpleEval = unit.characterCode == 'M' || unit.characterCode == 'R' ? true : false;

       
        if (simpleEval)
        {
            singletarget = unit.CheckForEnemy();
            if (singletarget != null)
                evaluation += _evaluationScoreLibrary[singletarget.characterCode];
             
            
        }else
        {
            targets = unit.CheckForEnemies(true);

            foreach(BaseUnit target in targets)
                evaluation += _evaluationScoreLibrary[target.characterCode];
            
        }

        int perspective = unit.teamColor == _aiTeamColor ? 1 : -1; //the AI wants a high evaluation for itself and a low evaluation fot the player;
        evaluation *= perspective;

        return evaluation;

    }
    public void Play()
    {
        GameManager.STATIC_SetAIEvaluationStatus(true);


        Move move = AISix(3, _aiUnits,null) ;

        Debug.Log(move.evaluation);

         move.target.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
         move.unit.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");



        //Debug.Log(AIFour(2, _aiUnits));
        GameManager.STATIC_SetAIEvaluationStatus(false);

       //move.unit.Move(move.target);

    }


    int AIFour(int depth, List<BaseUnit> units)
    {
        List<Tile> moves = new List<Tile>();

        if (depth == 0)
            return 0;

        int counter = 0;


        foreach (BaseUnit unit in units)
        {
            List<Tile> ValidMmoves = CheckValidMoves(unit);
            

            if (ValidMmoves != null)
            {
                
                foreach (Tile tile in ValidMmoves)
                {
                    moves.Add(tile);
                }

                if (moves.Count != 0)
                {
                    Tile CurrentTile = unit.currentTile;

                    foreach (Tile TargetTile in moves)
                    {

                        unit.Move(TargetTile);

                        counter++;

                        Evaluate(unit);

                        if (units == _aiUnits)
                            counter += AIFour(depth - 1, _playerUnits);
                        else
                            counter += AIFour(depth - 1, _aiUnits);

                        unit.Move(CurrentTile);
                    }
                }
            }

            moves.Clear();
           
        }

        return counter;
    }

    Move EvaluateToo(BaseUnit unit)
    {
        BaseUnit singletarget = null;
        List<BaseUnit> targets = new List<BaseUnit>();

        double evaluation = double.PositiveInfinity;
        bool simpleEval = unit.characterCode == 'M' || unit.characterCode == 'R' ? true : false;


        if (simpleEval)
        {
            singletarget = unit.CheckForEnemy();
            if (singletarget != null)
                evaluation += _evaluationScoreLibrary[singletarget.characterCode];


        }
        else
        {
            targets = unit.CheckForEnemies(true);

            foreach (BaseUnit target in targets)
                evaluation += _evaluationScoreLibrary[target.characterCode];

        }

        int perspective = unit.teamColor == _aiTeamColor ? 1 : -1; //the AI wants a high evaluation for itself and a low evaluation fot the player;
        evaluation *= perspective;

        Move move = new Move(evaluation, unit, unit.currentTile);
        
        return move;

    }

    Move AISix(int depth, List<BaseUnit> units, BaseUnit EvaluationUnit)
    {
        Move BestMove = new Move(double.NegativeInfinity, null, null);
        Move EvaluatedMove;

        List<Tile> moves = new List<Tile>();

        if (depth == 0)
            return EvaluateToo(EvaluationUnit);


        foreach (BaseUnit unit in units)
        {
            List<Tile> ValidMmoves = CheckValidMoves(unit);


            if (ValidMmoves != null)
            {

                foreach (Tile tile in ValidMmoves)
                {
                    moves.Add(tile);
                }

                if (moves.Count != 0)
                {
                    Tile CurrentTile = unit.currentTile;

                    foreach (Tile TargetTile in moves)
                    {
                        unit.Move(TargetTile);

                        if (units == _aiUnits)
                            EvaluatedMove = AISix(depth - 1, _playerUnits, unit);
                        else
                            EvaluatedMove = AISix(depth - 1, _aiUnits, unit);


                        if(EvaluatedMove.evaluation > BestMove.evaluation)
                        {
                            BestMove = EvaluatedMove;
                        }

                        unit.Move(CurrentTile);
                    }
                }
            }

            moves.Clear();

        }

        return BestMove;
    }

    List<Tile>  CheckValidMoves(BaseUnit unit)
    {

        if (!unit.gameObject.activeSelf)
            return null;

        unit.CheckPath();

        if (unit.highlightedTiles.Count == 0)
            return null;

        return unit.highlightedTiles;
    }
   
}
