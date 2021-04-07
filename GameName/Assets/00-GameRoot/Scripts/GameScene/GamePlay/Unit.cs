using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseUnit
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;
}
public class Unit : MonoBehaviour
{
    BaseUnit _characterUnit = new BaseUnit();

    public void AssignCharacterUnit(BaseUnit unit)
    {
        _characterUnit = unit;
    }

    public BaseUnit GetCharacterUnit()
    {
        return _characterUnit;
    }
    #region Heal

    public void Heal()
    {
        _characterUnit.currentHP += 2;
        if (_characterUnit.currentHP > _characterUnit.maxHP)
            _characterUnit.currentHP = _characterUnit.maxHP;

        Debug.Log(_characterUnit.currentHP);
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
            Destroy(gameObject);

        Debug.Log(_characterUnit.unitName + " took " + damage + " damage");
        Debug.Log(_characterUnit.currentHP);
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
