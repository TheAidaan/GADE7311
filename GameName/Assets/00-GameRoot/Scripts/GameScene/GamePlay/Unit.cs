using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    bool _turnCompleted,_teamHead;
    BaseUnit _characterUnit = new BaseUnit();
    GameObject _uiCanvas;

    private void Awake()
    {
        _uiCanvas = GetComponentInChildren<Canvas>().gameObject;
        _uiCanvas.SetActive(false);
    }

    public void AssignCharacterUnit(BaseUnit unit)
    {
        _characterUnit = unit;
    }

    public BaseUnit GetCharacterUnit()
    {
        return _characterUnit;
    }

    public bool Decativated()
    {
        if (_turnCompleted)
        {
            _uiCanvas.SetActive(false);
            return true;
        }
        else
            return false;     
    }

    public bool HeadUnitAlive()
    {
        SpriteRenderer render = gameObject.GetComponentInChildren<SpriteRenderer>();
        return render.enabled; // if renderer is enabled, headUnit is alive and vice versa
    }

    public void ShowHeadUnit()
    {
        SpriteRenderer render = gameObject.GetComponentInChildren<SpriteRenderer>();
        render.enabled = true;
    }

    public void Activate()
    {
        _turnCompleted = false;
        _uiCanvas.SetActive(true);
    }

    public void AddToTeam()
    {
        GameManager.Static_AddToTeam(_characterUnit.teamID, this);
    }
    public void SetAsHeadUnit()
    {
        _teamHead = true;
    }

    #region Heal

    public void Heal()
    {
        _characterUnit.currentHP += 2;
        if (_characterUnit.currentHP > _characterUnit.maxHP)
            _characterUnit.currentHP = _characterUnit.maxHP;

        _turnCompleted = true;
    }
    #endregion

    #region Attack
    public void GetTarget()
    {      
        InfoText.Static_SetOnScreenText("Choose the enemy you wish to attack");
        StartCoroutine(WaitForInput());
    }

    void TakeDamage(int damage)
    {
        _characterUnit.currentHP -= damage;
        if (_characterUnit.currentHP <= 0)
        {
            if (_teamHead)
            {
                SpriteRenderer render = gameObject.GetComponentInChildren<SpriteRenderer>();
                render.enabled = false;                                                     //hide until battle is done
            }
            else
            {
                Destroy(gameObject);
                GameManager.Static_RemoveFromTeam(_characterUnit.teamID, this);
            }     
        }
    }

    IEnumerator WaitForInput ()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit = new RaycastHit();

                Physics.Raycast(ray, out hit);

               if (hit.collider != null) // check if clicked on collider
               {
                    if (hit.collider.gameObject.GetComponent<Unit>() != null) // check if collider is a unit
                    {
                        hit.collider.gameObject.GetComponent<Unit>().TakeDamage(_characterUnit.damage);
                        _turnCompleted = true;
                        InfoText.Static_ClearOnScreenText();
                        done = true; // breaks the loop
                    }
               }               
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

    #endregion 
}
