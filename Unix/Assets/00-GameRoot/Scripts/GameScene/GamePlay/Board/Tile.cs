using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int boardPosition = Vector3Int.zero;
    public Board board = null;
    public Transform _transform = null;

    public BaseUnit currentUnit = null;

    Color32 _myColor;

    
    public void Setup(Board newBoard, Vector3Int newBoardPosition)
    {
        board = newBoard;
        boardPosition = newBoardPosition;

        _transform = GetComponent<Transform>();
    }
    #region Tile highlight when unit is hovered over and selected
    public void GetColor(Color32 myColor)
    {
        _myColor = myColor;
    }
    public void HighLight()
    {
        GetComponent<Renderer>().material.color = new Color32(34, 36, 43, 255); ;
    }
    public void StopHighLight()
    {

        GetComponent<Renderer>().material.color = _myColor;
    }
    #endregion


    public void RemovePiece()
    {
        if (currentUnit != null)
            currentUnit.Kill();
    }
}
