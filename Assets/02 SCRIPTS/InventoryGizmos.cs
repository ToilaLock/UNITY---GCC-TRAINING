using UnityEngine;

public class InventoryGizmos : MonoBehaviour
{
    [Header("1. Cell Setting")]
    [SerializeField] private int col = 9;
    [SerializeField] private int row = 1;
    [SerializeField] private float cellWidth = 1f;
    [SerializeField] private float cellHeight = 1f;
    [SerializeField] private float cellSpacing = 1f;

    [Header("2. Other")]
    [SerializeField] private Color cellColor = Color.white;
    [SerializeField] private float posX = 0f;
    [SerializeField] private float posY = 0f;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = cellColor;  
        var totalW = col * cellWidth + (col - 1) * cellSpacing;
        var totalH = row * cellHeight + (row - 1) * cellSpacing;
        var centerX = posX - totalW / 2f + cellWidth / 2f;
        var centerY = posY - totalH / 2f + cellHeight / 2f;

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                Vector2 position = new Vector2(centerX + j * (cellWidth + cellSpacing), centerY + i * (cellHeight + cellSpacing));
                Gizmos.DrawWireCube(position, new Vector2(cellWidth, cellHeight));
            }
        }
    }
}
