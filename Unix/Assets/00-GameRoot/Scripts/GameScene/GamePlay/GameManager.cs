using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    static bool _aiEvaluationInProgress;
    public static bool aiEvaluationInProgress { get { return _aiEvaluationInProgress; } }

    private void Awake()
    {
        instance = this;

        SetAIEvaluationStatus(false);

        Board board = GetComponent<Board>();
        board.Create();
        UnitManager.Static_Setup(board);
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

    public void SetAIEvaluationStatus(bool status)
    {
        _aiEvaluationInProgress = status;
    }
    public static void STATIC_SetAIEvaluationStatus(bool status)
    {
        instance.SetAIEvaluationStatus(status);
    }

}
