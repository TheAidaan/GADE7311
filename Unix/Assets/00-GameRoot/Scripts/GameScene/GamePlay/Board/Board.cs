using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;

    public Tile[,] allTiles = new Tile[8, 8];
    
    public void Create()
    {
        for(int x=0; x <8;x++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);

                Transform mtransform = newTile.GetComponent<Transform>();
                mtransform.position = new Vector3Int((x * 10) + 50,0, (z * 10) + 50);

                allTiles[x, z] = newTile.GetComponent<Tile>();
                allTiles[x, z].Setup(this, new Vector3Int(x,0,z));
            }
        }

        for (int x = 0; x < 8; x+=2)
        {
            for (int y = 0; y < 8; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x  + offset;

                allTiles[finalX, y].GetComponent<Renderer>().material.color = new Color32(230, 220, 187, 255);
            }
        }

        foreach (Tile tile in allTiles)
            tile.GetColor(tile.gameObject.GetComponent<Renderer>().material.color);
    }

    public TileState ValidateTile(int targetX, int targetY, BaseUnit checkingUnit)
    {
        //is Target on the board (Bounds Check)
        if (targetX < 0 || targetX > 7)                                                           
            return TileState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return TileState.OutOfBounds;

        Tile targetTile = allTiles[targetX, targetY]; // get the specific tile

        if (targetTile.currentUnit != null)
        {
            if (checkingUnit.teamColor == targetTile.currentUnit.teamColor)
                return TileState.Friendly;
            else
                return TileState.Enemy;
        }

        return TileState.Free;

    }
}
