using System;
using System.Collections.Generic;
using UnityEngine;

struct Move
{
    public BaseUnit unit;
    public Tile target;

    public Move(BaseUnit Unit, Tile Target)
    {
        unit = Unit;
        target = Target;
    }
}
public class MiniMax : MonoBehaviour
{
    List<BaseUnit> _playerUnits;
    List<BaseUnit> _aiUnits;

    Color _aiTeamColor;

    Move _bestMove;

    Dictionary<char, double> _evaluationScoreLibrary = new Dictionary<char, double>()
    {
        {'M', 1 },  
        {'R', 2 },  
        {'W', 3 },  
        
    };

    public void AssignUnits()
    {
        int rand = UnityEngine.Random.Range(0, 1);

        _playerUnits = rand == 0 ? GameData.blueUnits : GameData.redUnits;
        _aiUnits = rand == 0 ?  GameData.redUnits : GameData.blueUnits;
        _aiTeamColor = rand == 0 ? Color.red : Color.blue;

        GameData.STATIC_SetAIColor(_aiTeamColor);
    }

    public void Play()
    {
        GameManager.STATIC_SetAIEvaluationStatus(true);

        Algorithm(GameData.minMaxDepth,double.NegativeInfinity,double.PositiveInfinity, true, null);

        GameManager.STATIC_SetAIEvaluationStatus(false);

        _bestMove.unit.Move(_bestMove.target);
    }

    double Evaluate(BaseUnit unit)
    {
        BaseUnit singletarget = null;
        List<BaseUnit> targets = new List<BaseUnit>();

        double evaluation = 0;
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

        return evaluation;

    }

    double Algorithm(int depth,double alpha, double beta, bool maximising, BaseUnit EvaluationUnit)
    {
        double evaluation;

        List<Tile> moves = new List<Tile>();

        if (depth == 0)
            return Evaluate(EvaluationUnit);

        if (maximising) // working with the AI units because they need the highest evaluation
        {
            double maxEval = double.NegativeInfinity; //this is the lowest possible evaluation 

            foreach (BaseUnit unit in _aiUnits)
            {
                List<Tile> ValidMmoves = CheckValidMoves(unit); //every unit gets their moves checked to see if they have valid moves

                if (ValidMmoves != null) //if they have valid moves
                {
                    foreach (Tile tile in ValidMmoves)
                        moves.Add(tile);            // add them to an exterior list so that even though the unit will move and get a new set of avalable moves, the moves that this loop will use are not effected

                    if (moves.Count != 0) //just to check if the list isnt empty(MIGHT REMOVE)
                    {
                        Tile currentTile = unit.currentTile; //save the current tile that the unit is on at the moment

                        foreach (Tile targetTile in moves)
                        {

                            unit.Move(targetTile); //for every available move this unit has, move it.

                            evaluation = Algorithm(depth - 1,alpha,beta, false, unit); //loop through itself 

                            unit.Move(currentTile);

                            if (evaluation > maxEval)
                            {
                                maxEval = evaluation;

                                _bestMove.unit = unit;
                                _bestMove.target = targetTile;
                            }

                            alpha = Math.Max(alpha, evaluation);

                            if (beta <= alpha)
                                break;
                        }
                    }
                }
            }

            return maxEval;

        }else 
        {
            double minEval = double.PositiveInfinity; // the worst possible outcome for player units

            foreach (BaseUnit unit in _playerUnits)
            {
                List<Tile> ValidMmoves = CheckValidMoves(unit); //working with the fact that the player has just gone; 

                if (ValidMmoves != null)
                {

                    foreach (Tile tile in ValidMmoves)
                        moves.Add(tile);

                    if (moves.Count != 0)
                    {
                        Tile currentTile = unit.currentTile;

                        foreach (Tile targetTile in moves)
                        {
                            unit.Move(targetTile);

                            evaluation = Algorithm(depth - 1, alpha, beta, true, unit); //loop through itself 

                            minEval = Math.Min(evaluation, minEval);

                            unit.Move(currentTile);

                            beta = Math.Min(beta, evaluation);

                            if (beta <= alpha)
                                break;

                        }
                    }
                }
            }

            return minEval;
        }

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
