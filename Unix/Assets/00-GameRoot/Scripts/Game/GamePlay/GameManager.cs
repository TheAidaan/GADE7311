using UnityEngine;
using System;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    UnitManager _unitManager;

    static int _blueTeamScore, _redTeamScore;
    public static int blueTeamScore { get { return _blueTeamScore; } }
    public static int redTeamScore { get { return _redTeamScore; } }

    static bool _redTeamWon;
    public static bool redTeamWon { get { return _redTeamWon; } }

    static bool _aiEvaluationInProgress;
    public static bool aiEvaluationInProgress { get { return _aiEvaluationInProgress; } }

    bool _gameOver;

    public static event Action updateUI;
    public static event Action endGame;

    private void Awake()
    {
        instance = this;

        _blueTeamScore = _redTeamScore = 0;

        if (GameData.loadMinMaxScript)
            gameObject.AddComponent<MiniMax>();
        
        Board board = GetComponent<Board>();
       _unitManager = GetComponent<UnitManager>();

        board.Create();
        _unitManager.Setup(board);



        SetAIEvaluationStatus(false);

       

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)//did the mouse hit anything?
                {
                    if (hit.transform.gameObject.CompareTag("Interactive")) // only interact with interactive gameObjects
                    {
                        BaseUnit clickedUnit = hit.transform.gameObject.GetComponent<BaseUnit>();
                        if (clickedUnit != null)
                        {
                            clickedUnit.Clicked();
                        }
                    }
                }
            }
        }
    }
    public void SwitchSides(Color color)
    {
        _unitManager.SwitchSides(color);
    }

    public void UnitDeath(Color color)
    {
        if (color == Color.red)
            _blueTeamScore += 1;
        else
            _redTeamScore += 1;

        if (_redTeamScore == 12)
        {
            _redTeamWon = true;
            _gameOver = true;
        }
        if (_blueTeamScore == 12)
        {
            _redTeamWon = false;
            _gameOver = true;
        }

        if (updateUI != null)
        {
            updateUI();
        }

        if (_gameOver)
        {
            if (endGame != null)
            {
                endGame();
            }
        }

    }

    /*                  STATICS                  */

    public void SetAIEvaluationStatus(bool status)
    {
        _aiEvaluationInProgress = status;
    }
    public static void STATIC_SetAIEvaluationStatus(bool status)
    {
        instance.SetAIEvaluationStatus(status);
    }

    public static void Static_SwitchSides(Color color)
    {
        instance.SwitchSides(color);
    }
    public static void Static_UnitDeath(Color color)
    {
        instance.UnitDeath(color);
    }

}
