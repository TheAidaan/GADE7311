using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_UIManager : UIManager
{
    bool _showPauseScreen, _showSettingsMenu, _showGameOverScreen;
    [SerializeField]
    TextMeshProUGUI _txtRedTeamScore, _txtBlueTeamScore, txtWinner;

    private void Start()
    {
        UnitManager.updateUI += UpdateScores;
        UnitManager.endGame += EndGame;


    }

    void UpdateScores()
    {
        _txtBlueTeamScore.text = UnitManager.blueTeamScore.ToString();
        _txtRedTeamScore.text = UnitManager.redTeamScore.ToString();
    }

    

    public override void SetUI()
    { /*
        _childPanels[0] : Pause menu
        _childPanels[1] : Settings menu
        _childPanels[2] : Game Over Screen

      */
        _childPanels[0].SetActive(_showPauseScreen);
        _childPanels[1].SetActive(_showSettingsMenu);
        _childPanels[2].SetActive(_showGameOverScreen);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        _showSettingsMenu = false;
        _showPauseScreen = true;
        Time.timeScale = 0;
        SetUI();
    }
    public void Play()
    {
        _showPauseScreen = false;
        Time.timeScale = 1;
        SetUI();
    }

    public void ShowSettingsMenu()
    {
        _showPauseScreen = false;
        _showSettingsMenu = true;
        SetUI();
    }

    void EndGame()
    {
        _showGameOverScreen = true;
        _showPauseScreen = false;
        _showSettingsMenu = false;
        SetUI();

        string winningTeam = UnitManager.redTeamWon ? "red" : "blue";
        Color vertexColor = UnitManager.redTeamWon ? new Color32(210, 95, 64, 255) : new Color32(80, 124, 159, 255);
        txtWinner.text = winningTeam + "team Won!";
        txtWinner.color = vertexColor;

        UnitManager.endGame -= EndGame;
    }

}
