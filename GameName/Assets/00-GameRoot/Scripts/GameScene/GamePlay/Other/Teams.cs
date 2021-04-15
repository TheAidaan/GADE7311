using System.Collections.Generic;

public class BaseUnit
{
    public int teamID;
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;
}

public class BaseTeams
{
    public string teamName;
    public Unit headUnit = null;
    public List<Unit> unitMembers = new List<Unit>();

}