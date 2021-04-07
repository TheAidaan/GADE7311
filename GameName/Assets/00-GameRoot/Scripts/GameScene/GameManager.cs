using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    void Awake()
    {
        instance = this;
    }
    void SetCurrentTurn()
    {

    }

    public static void Static_SetCurrentTurn()
    {
        instance.SetCurrentTurn();
    }
}
