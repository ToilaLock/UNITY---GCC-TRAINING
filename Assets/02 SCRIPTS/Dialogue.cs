using UnityEngine;

public class Dialogue : MonoBehaviour
{ 
    
    [SerializeField] private GameObject npc;
    [SerializeField] private float rad = 3f;
    [SerializeField] private GameObject dia;
    [SerializeField] private float timer;
    private LayerMask playerCheck;
    private Vector2 point;
    private float timeCnt;
    
    void Start()
    {
        point = npc.transform.position;
        playerCheck = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timeCnt -= Time.deltaTime;

        if (isGay())
        {
            // Debug.Log("Vao vung Do gay tren 9000");
            dia.gameObject.SetActive(true);
            timeCnt = timer;
        }
        else if (timeCnt <= 0)
        {
            // Debug.Log("Ngoai vung Gay");
            dia.gameObject.SetActive(false);
        }
    }

    public bool isGay()
    {
        return Physics2D.OverlapCircle(point, rad, playerCheck);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(point, rad);
    }
}
