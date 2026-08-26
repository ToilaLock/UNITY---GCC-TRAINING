using UnityEngine;

public class VectorTest : MonoBehaviour
{
    Vector2 position;
    [SerializeField] private float xPos = 0f;
    [SerializeField] private float yPos = 0f;
    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private int gridWidth = 1;
    [SerializeField] private int row = 10;
    [SerializeField] private int collum = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = new Vector2(xPos, yPos);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        var x = transform.position.x;
        var y = transform.position.y;

        for(var i = 0; i <= row; i+=gridWidth)
        {
            for(var j = 0; j <= collum; j+=gridWidth)
            {
                Gizmos.DrawWireCube(new Vector2(x+i, y+j), new Vector2(gridWidth, gridWidth));;
            }
        }
    }
}
