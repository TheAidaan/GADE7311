using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static Test instance;


    int i=0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Debug.Log(i);
        i++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(true);
        }

            if (Input.GetKeyDown(KeyCode.K))
            {
                gameObject.SetActive(false);
            }

    }
}
