using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int boardPosition = Vector3Int.zero;
    public Board board = null;

    public BaseUnit currentUnit = null;

    
    public void Setup(Board newBoard, Vector3Int newBoardPosition)
    {
        board = newBoard;
        boardPosition = newBoardPosition;

    }

}
