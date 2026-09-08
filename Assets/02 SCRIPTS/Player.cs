using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float castDist;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;
    private float moveInput;
    private float jumpInput;
    private int coinCount = 0;
   

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("MovementPlatformer");
        jumpAction = InputSystem.actions.FindAction("MovementPlatformerJump");
    }

    private void Update() {
        moveInput = moveAction.ReadValue<float>();
        jumpInput = jumpAction.ReadValue<float>();
    }

    private void LateUpdate() {
        rb.linearVelocityX = moveInput * speed;
        if (jumpAction.WasPressedThisFrame() && isGrounded())
        rb.linearVelocityY = jumpInput * jumpForce;
    }

    public bool isGrounded() {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, castDist, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.down * castDist, boxSize);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            Debug.Log($"Da nhat {++coinCount}");
        }
    }
}
