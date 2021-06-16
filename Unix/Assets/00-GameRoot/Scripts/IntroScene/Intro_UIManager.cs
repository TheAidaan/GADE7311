using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_UIManager : UIManager
{
    bool _showTitleScreen = true;
    bool _showSettingScreen, _showPlayOptions;

    public override void SetUI()
    { /*
        _childPanels[0] : Title Screen
        _childPanels[1] : Settings Screen
        _childPanels[2] : Settings Screen

      */

        _childPanels[0].SetActive(_showTitleScreen);
        _childPanels[1].SetActive(_showSettingScreen);
        _childPanels[2].SetActive(_showPlayOptions);
    }
    public void Leave()
    {
        _showTitleScreen = true;
        _showSettingScreen = false;
        _showPlayOptions = false;
        SetUI();
    }

    public void ShowSettingsMenu()
    {
        _showSettingScreen = true;
        _showTitleScreen = false;
        SetUI();
    }
    public void ShowPlayOptions()
    {
        _showPlayOptions = true;
        _showTitleScreen = false;
        SetUI();
    }

    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(2);
    } 
    
    public void LoadMultiPlayer()
    {
        SceneManager.LoadScene(1);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
