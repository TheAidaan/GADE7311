using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public int maxHealth;
    public float coolDown;
    int _health;
    public Tile currentTile = null;

    #region Unit setup
    public Color teamColor = Color.clear;

    Tile _originalTile = null;

    public Vector3 rotation;

    char chararcterCode;


    public virtual void Setup(Color TeamColor, Color32 unitColor, char ChararcterCode)
    {
        chararcterCode = ChararcterCode;
        teamColor = TeamColor;

        int layer = teamColor == Color.red ? 3:6;
        gameObject.layer = layer;
        GetComponent<Renderer>().material.color = unitColor;

        _health = maxHealth;

        rotation = transform.rotation.eulerAngles;
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
    public List<Tile> highlightedTiles = new List<Tile>(); // all possibile moves list


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

    public virtual void CheckPath()
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
    public virtual void Move(Tile targetTile)
    {
        //clear the current tile
        currentTile.currentUnit = null;

        //change current tile
        currentTile = targetTile;
        currentTile.currentUnit = this;

        //Move on board
        transform.position = currentTile.transform.position;
        targetTile = null;

        GameManager.Static_SwitchSides(teamColor);

        //HideHighlightedTiles();
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
        gameObject.SetActive(true);
        Place(_originalTile);
    }

    

    #region Mouse events
    public void OnMouseOver()
    {
        if (gameObject.CompareTag("Interactive"))
        {
            // Test for cells
            CheckPath();
            ShowHighlightedTiles(); //show highlighted tile
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
        Tile targetTile = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) // did the ray hit anything?
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);// follow the ray(the cursor)

            foreach (Tile tile in highlightedTiles)
            {
                if (hit.transform.gameObject.GetComponent<Tile>() == tile) // is the cursor over one of he highlighted tiles?
                {
                    targetTile = tile;//make that tile the target tile 
                    break;// only one can be the target 
                }

                targetTile = null;
            }

            if (Input.GetMouseButtonDown(0) && targetTile != null) // right button clicked
            {

                Move(targetTile); // go to target location;
                TransitionToState(idleState);

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

    public IEnumerator TakeDamage(int damage)
    {
        _health -= damage;
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        if (_health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Static_UnitDeath(teamColor, chararcterCode);
        }
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

    }
    #endregion

    #region abstract Methods
    public virtual void Attack() { StartCoroutine(CoolDown()); }
    public virtual IEnumerator CoolDown() 
    { 
        yield return new WaitForSeconds(coolDown); 
        StartCoroutine(CoolDown()); 
    }
    public virtual void CheckForEnemies() { }
    #endregion
}
