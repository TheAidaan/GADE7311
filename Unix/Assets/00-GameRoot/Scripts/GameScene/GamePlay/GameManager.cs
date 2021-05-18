using UnityEngine;
using System;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; //single...

    UnitManager _unitManager;
    MiniMax _miniMaxAI;

    static int _redScore, _blueScore;
    public static int redScore { get { return _redScore; } }
    public static int blueScore { get { return _blueScore; } }

    static bool _gameOver;
    public static bool gameOver { get { return _gameOver; } }

    public static bool singlePlayer;
    Color _aiColor;

    Dictionary<char, int> _scoreLibrary = new Dictionary<char, int>()
    {
        {'M', 1 },  //SIX ranged units per team     (SIX points)
        {'R', 2 },  //FOUR ranged units per team    (EIGHT points)
        {'W', 3 },  //TWO ranged units per team     (SIX points)
        
        //Need 20 points to win
    };

    private void Awake()
    {
        instance = this; //..ton

        if (singlePlayer)
            _miniMaxAI = gameObject.AddComponent<MiniMax>();

        SetUpBoard();

        _redScore = _blueScore = 20;

        _gameOver = false;
    }

    void SetUpBoard()
    {
        Board board = GetComponent<Board>();
        board.Create();
        _unitManager = GetComponent<UnitManager>();
        _unitManager.Setup(board);
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


    public static void Static_UnitDeath(Color color, char chararcterCode)
    {
        instance.UnitDeath(color, chararcterCode);
    }

    void UnitDeath(Color color, char chararcterCode)
    {
        if (color == Color.red)
            _blueScore -= _scoreLibrary[chararcterCode];
        else
            _redScore -= _scoreLibrary[chararcterCode];

        if (redScore == 0 || blueScore == 0)
            _gameOver = true;

        if (_gameOver)
        {
            Debug.Log("gameOver");
        }
    }
    
    public static void Static_SendAIUnits(List<BaseUnit> allUnits, Color aiColor, List<BaseUnit> playerUnits)
    {
        instance.SendAIUnits(allUnits, aiColor, playerUnits);
    }
    void SendAIUnits(List<BaseUnit> aiUnits, Color aiColor, List<BaseUnit> playerUnits)
    {
        _aiColor = aiColor;
        GetComponent<MiniMax>().AIAttributes(aiUnits, aiColor, playerUnits);
    }

    public static void Static_SwitchSides(Color color)
    {
        instance.SwitchSides(color);
    }

    void SwitchSides(Color color) // takes in the color that just went
    {
        _unitManager.SwitchSides(color);

        if (singlePlayer)
        {
            if (color != _aiColor)  // if the color tht just went isnt the ai color then the ai must surely go now
            {
                _miniMaxAI.Play();
            }
        }
    }
}
