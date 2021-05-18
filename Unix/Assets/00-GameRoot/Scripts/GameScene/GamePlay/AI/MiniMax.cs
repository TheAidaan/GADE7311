using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Moves //stores every available move that a unit can make 
{
    public BaseUnit Unit { get; } // stores the specific unit that has a move
    public List<Tile> Tiles { get; } // stores the all tiles that a unit can move to

    public Moves(BaseUnit unit, List<Tile> tile)
    {
        Unit = unit;
        Tiles = tile;
    }
}

struct BestMove //stores the best move the algorithm should make
{
    public BaseUnit Unit { get; } // stores the specific unit that has a move
    public Tile Tile { get; } // stores the all tiles that a unit can move to

    public BestMove(BaseUnit unit, Tile tile)
    {
        Unit = unit;
        Tile = tile;
    }
}

public class MiniMax : MonoBehaviour
{
    Color teamColor;
    List<BaseUnit> _aiUnits, _playerUnits;

    BestMove _bestMove;

    public void AIAttributes(List<BaseUnit> AIUnits, Color aiColor, List<BaseUnit> PlayerUnits)
    {
        teamColor = aiColor;
        _aiUnits = AIUnits;
        _playerUnits = PlayerUnits;
        //Debug.Log(_aiUnits.Count);
    }

    public void Play()
    {
        MoveAIUnits(1);
        //MiniMaxAlgorithm(1, true, teamColor);
        //_bestMove.Unit.Move(_bestMove.Tile);

    }

    void RandomMover()
    {
        int index;
        BaseUnit unit = null;

        while (unit == null)
        {
            index = Random.Range(0, _aiUnits.Count);
            unit = _aiUnits[index];

            if (!unit.gameObject.activeSelf)
            {
                unit = null;
            }
            else
            {
                unit.CheckPath();
                if (unit.highlightedTiles.Count == 0)
                    unit = null;

            }
        }
        unit.CheckPath();
        unit.ShowHighlightedTiles();

        index = Random.Range(0, unit.highlightedTiles.Count);
        unit.Move(unit.highlightedTiles[index]);

    }

    void MoveAIUnits(int depth)
    {
        Tile currentTile;

        List<Moves> moves = GenerateMoves(_aiUnits);

        foreach(Moves move in moves)
        {
            currentTile = move.Unit.currentTile;
            
            foreach (Tile tile in move.Tiles)
            {
                move.Unit.Move(tile);
                //GeneratedMover(depth - 1);
                StartCoroutine(MovePlayerUnits(depth));
                move.Unit.Move(currentTile);
            }
        }

        
    } //"generation"

    IEnumerator MovePlayerUnits(int depth)
    {
        Tile currentTile;

        Debug.Log("here");
        List<Moves> moves = GenerateMoves(_playerUnits);

        foreach (Moves move in moves)
        {
            currentTile = move.Unit.currentTile;

            foreach (Tile tile in move.Tiles)
            {
                move.Unit.Move(tile);
                //GeneratedMover(depth - 1);
                yield return new WaitForSeconds(1);
                move.Unit.Move(currentTile);
            }
        }
    } //"generation"

    List<Moves> GenerateMoves(List<BaseUnit> units)
    {
        List<Moves> moves = new List<Moves>();

        foreach (BaseUnit unit in units)
        {
            if (!unit.gameObject.activeSelf)
            {
                continue;
            }
            else
            {
                unit.CheckPath();
                if (unit.highlightedTiles.Count == 0)
                    continue;
            }

            Moves move = new Moves(unit, unit.highlightedTiles);
            moves.Add(move);
        }
        
        return moves;
     
    }

    int Evaluate(Color color)
    {
        return color == Color.red ? GameManager.redScore - GameManager.blueScore : GameManager.blueScore - GameManager.blueScore;
    }

    //double MiniMaxAlgorithm(int depth,bool maximizing, Color color)
    //{
    //    if ((depth == 0) || (GameManager.gameOver))
    //        Evaluate(color);

    //    List<Moves> moves = GenerateMoves();

    //    if (maximizing)
    //    {
            
    //        double maxEval = double.NegativeInfinity;
    //        foreach (Moves move in moves)
    //        {
    //            Tile currentTile = move.Unit.currentTile;
                
    //            foreach (Tile tile in move.Tiles)
    //            {
    //                move.Unit.Move(tile);
    //                //double currentEvaluation = MiniMaxAlgorithm(depth - 1, false,color);
    //                //move.Unit.Move(currentTile);

    //                //Debug.Log(currentEvaluation);

    //                //if (currentEvaluation > maxEval)
    //                //{
    //                //    Debug.Log("here");
    //                //    maxEval = currentEvaluation;
    //                //    _bestMove = new BestMove(move.Unit, tile);
    //                //}
    //            }
    //        }

    //        return maxEval;
    //    }
    //    else
    //    {
    //        double minEval = double.PositiveInfinity;

    //        foreach (Moves move in moves)
    //        {
    //            Tile currentTile = move.Unit.currentTile;

    //            foreach (Tile tile in move.Tiles)
    //            {
    //                move.Unit.Move(tile);
    //                double currentEvaluation = MiniMaxAlgorithm(depth - 1, false, color);
    //                move.Unit.Move(currentTile);

    //                if (currentEvaluation < minEval)
    //                {
    //                    minEval = currentEvaluation;
    //                    _bestMove = new BestMove(move.Unit, tile);
    //                }
    //            }
    //        }

    //        return minEval;

    //    }

    //}
}
