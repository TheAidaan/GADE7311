using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public int maxHealth;
    int _health;
    public Tile currentTile = null;

    #region Unit setup
    public Color teamColor = Color.clear;

    Tile _originalTile = null;
   

    UnitManager _unitManager;

    public virtual void Setup(Color TeamColor, Color32 unitColor, UnitManager unitManager)
    {
        _unitManager = unitManager;
        teamColor = TeamColor;

        int layer = teamColor == Color.red ? 3:6;
        gameObject.layer = layer;
        GetComponent<Renderer>().material.color = unitColor;

        _health = maxHealth;

    }

    public void Place(Tile newTile)
    {
        //tile
        currentTile = newTile;
        _originalTile = newTile;

        currentTile.currentUnit = this;

        //object
        transform.position = newTile.transform.position;
        gameObject.SetActive(true);
    }
    #endregion

    #region Unit Movement
    //Movement Variables
    public Vector3Int movement = Vector3Int.one;
    public List<Tile> highlightedTiles = new List<Tile>();

    Tile _targetTile = null;
    public bool selected;
    void CreateTilePath(int xDirection, int yDirection, int movement)
    {
        //TargetPosition
        int currentX = currentTile.boardPosition.x;
        int currentY = currentTile.boardPosition.z; // z represents the world point, but it also represents the y point in the 2D array. 

        //Check each tile
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            TileState tileState = TileState.None;
            tileState = currentTile.board.ValidateTile(currentX, currentY, this);

            if (tileState != TileState.Free) //if the tile is out of bounds or has a friendly on it, then break the look and don't add anything to the available target tiles.
                break;

            highlightedTiles.Add(currentTile.board.allTiles[currentX, currentY]); //add the tile if it is free

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

    public void ShowHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
            tile.HighLight();
            
    }

    public void HideHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
            tile.StopHighLight();

        highlightedTiles.Clear();
    }

    public void Reset() //Resets peices, keeps board the same
    {
        Kill();
        Place(_originalTile);
    }

    public void Kill() // CHANGE
    {
        currentTile.currentUnit = null;

        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        _targetTile.RemovePiece(); //CHANGE (CHECK IF UNIT IS ALREADY THERE)

        //clear the current tile
        currentTile.currentUnit = null;

        //change current tile
        currentTile = _targetTile;
        currentTile.currentUnit = this;

        //Move on board
        transform.position = currentTile.transform.position;
        _targetTile = null;


    }

    #region Mouse events
    public void OnMouseOver()
    {
        if (gameObject.CompareTag("Interactive"))
        {
            // Test for cells
            CheckPath();
            ShowHighlightedTiles(); //show highlighted tiles
        }   
    }

    private void OnMouseExit()
    {
        if(_currentState != hoverState) //don't clear the highlighted tiles while the player is draging a unit
            HideHighlightedTiles();
    }

    public void Clicked()
    {
        TransitionToState(hoverState);  
    }

    public void Drag()
    {
        

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) // did the ray hit anything?
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);// follow the ray(the cursor)

                foreach (Tile tile in highlightedTiles) 
                {
                    if (hit.transform.gameObject.GetComponent<Tile>() == tile) // is the cursor over one of he highlighted tiles?
                    {
                        _targetTile = tile;//make that tile the target tile 
                        break;// only one can be the target 
                    }

                    _targetTile = null; 
                }

            if (Input.GetMouseButtonDown(0) && _targetTile != null) // right button clicked
            {

                Move(); // go to target location;
                TransitionToState(idleState);
                _unitManager.SwitchSides(teamColor);
            }


            if (Input.GetMouseButtonDown(1)) // right button clicked
            { 
                TransitionToState(idleState);
            }
        }


    }

    #endregion

    #endregion

    #region States and Interactions
    UnitBaseState _currentState;
    public UnitBaseState currentState { get { return _currentState; } }


    public readonly UnitAttackState attackState = new UnitAttackState(); // attack enemies
    public readonly UnitHoverState hoverState = new UnitHoverState();  // hover over enemies
    public readonly UnitIdleState idleState = new UnitIdleState(); // scan for enemies

    void Start()
    {
        TransitionToState(idleState);
    }
    private void Update()
    {
        currentState.Update(this);
    }
    public void TransitionToState(UnitBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            gameObject.SetActive(false);

        Debug.Log(_health);
    }
    #endregion

    #region abstract Methods
    public virtual void Attack() { }
    public virtual void CheckForEnemies() { }
    #endregion
}
