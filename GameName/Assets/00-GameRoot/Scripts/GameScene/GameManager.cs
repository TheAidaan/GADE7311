using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    LineRenderer _line;
    public ActiveMarble _activeMarbles;
    protected InactiveMarble[] _inactiveMarbles;

    // Start is called before the first frame update
    void Awake()
    {
        _line = GetComponent<LineRenderer>();
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
}
