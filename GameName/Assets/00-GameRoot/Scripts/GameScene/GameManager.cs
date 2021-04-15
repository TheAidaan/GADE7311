using UnityEngine;
using System.Collections;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool battleOver;

    BaseTeams[] _teams = new BaseTeams[2]; // will  hold the necessary information for the two teams


    
    void Awake()
    {
        _teams[0] = new BaseTeams();
        _teams[1] = new BaseTeams();
        instance = this;
    }

    void Start()
    {
        _teams[0].teamName = "bubu";
        _teams[1].teamName = "sheba"; //must get the team name from somebody 
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WaitForDeactivation(0));
        }
    }

    public IEnumerator WaitForDeactivation(int teamID)
    {
        bool done, hasActiveTeamMembers;
        done = hasActiveTeamMembers = false;
        if (_teams[teamID].unitMembers.Count != 0)
        {
            foreach (Unit unit in _teams[teamID].unitMembers)
            {
                
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
            hasActiveTeamMembers = true;
        }
        
        if (_teams[teamID].headUnit.HeadUnitAlive()) //if false then the team has lost
        {
            _teams[teamID].headUnit.Activate();
            done = false;
            while (!done) // essentially a "while true", but with a bool to break out naturally
            {
                if (_teams[teamID].headUnit.Decativated())
                {
                    done = true; // breaks the loop     
                }
                yield return null; // wait until next frame, then continue execution from here (loop continues)
            }
        }
        
        if (hasActiveTeamMembers || _teams[teamID].headUnit.HeadUnitAlive()) //does this team have any members that can attack and does it still have a head unit?
            StartCoroutine(WaitForDeactivation(Mathf.Abs(teamID - 1)));//if yes, then let the next team have their turn
        else
            EndBattle(Mathf.Abs(teamID - 1));// if no, declare the next team the winner
    }

    void EndBattle(int WinningTeamID)
    {
        InfoText.Static_SetOnScreenText(_teams[WinningTeamID].teamName + " has won");
    }

    void AddToTeam(int teamID, Unit unit)
    {
        if ( _teams[teamID].headUnit == null)
        {
            _teams[teamID].headUnit = unit;
            unit.SetAsHeadUnit();
        }     
        else
            _teams[teamID].unitMembers.Add(unit);
    }
    public void RemoveFromTeam(int id, Unit unit)
    {
        _teams[id].unitMembers.Remove(unit); //unit has been killed in the game
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
