using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUnit : MonoBehaviour
{
    #region Unit setup
    public Color teamColor = Color.clear;

    Tile _originalTile = null;
    Tile _currentTile = null;

    UnitManager _unitManager;

    public virtual void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        _unitManager = unitManager;
        teamColor = TeamColor;

        GetComponent<Renderer>().material.color = unitColor;
    }

    public void Place(Tile newTile)
    {
        //tile
        _currentTile = newTile;
        _originalTile = newTile;

        _currentTile.currentUnit = this;

        //object
        transform.position = newTile.transform.position;
        gameObject.SetActive(true);
    }
    #endregion

    #region Unit Movement
    //Movement Variables
    public Vector3Int movement = Vector3Int.one;
    List<Tile> _highlightedTiles = new List<Tile>();

    Tile _targetTile = null;
    bool _selected;
    void CreateTilePath(int xDirection, int zDirection, int movement)
    {
        //TargetPosition
        int currentX = _currentTile.boardPosition.x;
        int currentZ = _currentTile.boardPosition.z; // z as in world point. But is the Y point in the boards 2D array 

        //Check each tile
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentZ += zDirection;

            TileState tileState = TileState.None;
            tileState = _currentTile.board.ValidateTile(currentX, currentZ, this);

            if (tileState == TileState.Enemy)
            {
                _highlightedTiles.Add(_currentTile.board.allTiles[currentX, currentZ]); // add tiles with an enemy on them as a moveable place on target on the board.
                break;
            }

            if (tileState != TileState.Free) //if the tile is out of bounds or has a friendly on it, then break the look and don't add anything to the available target tiles.
                break;

            _highlightedTiles.Add(_currentTile.board.allTiles[currentX, currentZ]); //add the tile if it is free

        }
    }

    protected virtual void CheckPath()
    {
        //horizantal 
        CreateTilePath(1, 0, movement.x);
        CreateTilePath(-1, 0, movement.x);

        //horizantal 
        CreateTilePath(0, 1, movement.z);
        CreateTilePath(0, -1, movement.z);

        //horizantal 
        CreateTilePath(1, 1, movement.y);
        CreateTilePath(-1, 1, movement.y);

        //horizantal 
        CreateTilePath(-1, -1, movement.y);
        CreateTilePath(1, -1, movement.y);
    }

    void ShowTiles()
    {
        foreach (Tile tile in _highlightedTiles)
            tile.HighLight();
            
    }

    void ClearTiles()
    {
        foreach (Tile tile in _highlightedTiles)
            tile.StopHighLight();

        _highlightedTiles.Clear();
    }

    public void Reset() //Resets peices, keeps board the same
    {
        Kill();
        Place(_originalTile);
    }

    public void Kill() // CHANGE
    {
        _currentTile.currentUnit = null;

        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        _targetTile.RemovePiece(); //CHANGE (CHECK IF UNIT IS ALREADY THERE)

        //clear the current tile
        _currentTile.currentUnit = null;

        //change current tile
        _currentTile = _targetTile;
        _currentTile.currentUnit = this;

        //Move on board
        transform.position = _currentTile.transform.position;
        _targetTile = null;


    }

    #region Mouse events
    public void OnMouseOver()
    {
        if (gameObject.CompareTag("Interactive"))
        {
            // Test for cells
            CheckPath();

            ShowTiles(); //show tiles
        }
        
    }

    public void Clicked()
    {
        StartCoroutine(Drag(transform.position));    
    }

    IEnumerator Drag(Vector3 originalPosition)
    {
        _selected = true;
        bool followMouse = true;

        while (followMouse)
        {
            GetComponent<BoxCollider>().enabled = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) // did the ray hit anything?
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);// follow the ray(the cursor)

                foreach (Tile tile in _highlightedTiles) 
                {
                    if (hit.transform.gameObject.GetComponent<Tile>() == tile) // is the cursor over one of he highlighted tiles?
                    {
                        _targetTile = tile;//make that tile the target tile 
                        break;// only one can be the target 
                    }

                    _targetTile = null; 
                }

            }
                

            if (Input.GetMouseButtonDown(1)) // right button clicked
            {
                followMouse = false; //stop following the mouse
                if (_targetTile != null) // got somewhere to go?
                {
                    Move(); // go to target location;
                }
                else // nowhere to go? just go home
                {
                    transform.position = originalPosition; // go back to where you came from
                }

                
                _selected = false;
                ClearTiles(); //hide
                GetComponent<BoxCollider>().enabled = true;

                _unitManager.SwitchSides(teamColor);
            }

            yield return null;
        }


    }

    public void OnMouseExit() //EndDrag
    {
        if (!_selected)
            ClearTiles();//hide

    }

    #endregion

    #endregion

}
