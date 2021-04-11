using UnityEngine;
using System.Collections;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool battleOver;

    Teams[] _currentTeams = new Teams[2]; // will  hold the necessary information for the two teams


    
    void Awake()
    {
        _currentTeams[0] = new Teams();
        _currentTeams[1] = new Teams();
        instance = this;
    }

    void Start()
    {
        _currentTeams[0].teamName = "bubu";
        _currentTeams[1].teamName = "sheba"; //must get the team name from somebody 
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle();
        }
    }

    void StartBattle()
    {

        StartCoroutine(WaitForDeactivation(0));
    }

    public IEnumerator WaitForDeactivation(int teamID)
    {
        if (_currentTeams[teamID].unitMembers.Count != 0)
        {
            Debug.Log("Unit Members Count: " + _currentTeams[teamID].unitMembers.Count);
            foreach (Unit unit in _currentTeams[teamID].unitMembers)
            {
                bool done = false;
                unit.Activate();

                while (!done) // essentially a "while true", but with a bool to break out naturally
                {
                    if (unit.Decativated())
                    {
                        done = true; // breaks the loop     
                    }
                    yield return null; // wait until next frame, then continue execution from here (loop continues)
                }
            }

            StartCoroutine(WaitForDeactivation(Mathf.Abs(teamID - 1)));
        }
        else
        {
            EndBattle(Mathf.Abs(teamID - 1));
        }
    }

    void EndBattle(int WinningTeamID)
    {
        InfoText.Static_SetOnScreenText(_currentTeams[WinningTeamID].teamName + " has won");
    }

    void AddToTeam(int id, Unit unit)
    {
        _currentTeams[id].unitMembers.Add(unit);
    }
    public void RemoveFromTeam(int id, Unit unit)
    {
        _currentTeams[id].unitMembers.Remove(unit); //unit has been killed in the game
    }


        /*              PUBLIC STATICS              */

    public static void Static_AddToTeam(int id, Unit unit)
    {
        instance.AddToTeam(id, unit);
    }
    public static void Static_RemoveFromTeam(int id, Unit unit)
    {
        instance.RemoveFromTeam(id, unit);
    }
}
