using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ActiveMarble")
        {
            GameManager.Static_ActiveMarbleOutsideRing();
        }

        if (other.gameObject.tag == "InactiveMarble")
        {
            GameManager.Static_InactiveMarbleOutsideRing();
        }
    }
}
