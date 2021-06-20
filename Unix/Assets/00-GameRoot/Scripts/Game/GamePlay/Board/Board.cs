using UnityEngine;

public enum TileState
{
    None,
    Taken,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{

    public Tile[,] allTiles = new Tile[8, 8]; //stores all tiles

    private void Awake()
    {
        
    }
    public void Create()
    {
        GameObject _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        for (int x=0; x <8;x++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject newTile = Instantiate(_tilePrefab, transform);

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
    }

    public TileState ValidateTile(int targetX, int targetY, BaseUnit checkingUnit)
    {
        //is Target on the board (Bounds Check)
        if (targetX < 0 || targetX > 7)                                                           
            return TileState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return TileState.OutOfBounds;

        Tile targetTile = allTiles[targetX, targetY]; // get the specific tile

        if (targetTile.currentUnit != null) // is there a unit on the target tile?
        {
            if (targetTile.currentUnit.gameObject.activeSelf) // is that unit active
            {
                    return TileState.Taken;
            }  
        }

        return TileState.Free;

    }
}
