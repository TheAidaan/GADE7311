using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player[] _players = new Player[2];
    public static GameManager instance;
    LineRenderer _line;
    public ActiveMarble _activeMarbles;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        _line = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _players[0] = new Player();
        _players[0].playerName = "player1";

        _players[1] = new Player();
        _players[1].playerName = "player2";

        int startingplayer;
        startingplayer = Random.Range(1, 2);
        InfoText.Static_SetOnScreenText("Select where you would like to start player" + startingplayer.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var direction = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            var ballPos = new Vector3(_activeMarbles.transform.position.x, 0.1f, _activeMarbles.transform.position.z);
            var mousePos = new Vector3(hit.point.x, 0.1f, hit.point.z);
            _line.SetPosition(0, mousePos);
            _line.SetPosition(1, ballPos);
            direction = (mousePos - ballPos).normalized;
        }
        if (Input.GetMouseButtonDown(0) && _line.gameObject.activeSelf)
        {
            _line.enabled = false;
            _activeMarbles.GetComponent<Rigidbody>().velocity = direction * 1f;
        }

        if (_activeMarbles.GetComponent<Rigidbody>().velocity.magnitude < 0.3f)
        {
            _line.enabled = true;
        }
    }
    void ActiveMarbleOutsideRing()
    {
        Debug.Log("ActiveMarble Left");
    }
    void InactiveMarbleOutsideRing()
    {
        Debug.Log("InactiveMarble Left");
    }

    public static void Static_ActiveMarbleOutsideRing()
    {
        instance.ActiveMarbleOutsideRing();

    }
    public static void Static_InactiveMarbleOutsideRing()
    {
        instance.InactiveMarbleOutsideRing();
    }
}
