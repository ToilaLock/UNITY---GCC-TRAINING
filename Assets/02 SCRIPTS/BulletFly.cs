using UnityEngine;
using UnityEngine.InputSystem;

public class BulletFly : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeSpan = 3f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = (Vector2)mousePosInWorld - (Vector2)transform.position;

        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hitted Enemy");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
