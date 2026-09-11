using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float knockBackForce = 1f;
    [SerializeField] private float timeKnockBack = 0.05f;

    [Header("BoxCast")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float castDist;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;
    private float moveInput;
    private float jumpInput;
    private int coinCount = 0;
    private bool isKnockBack = false;
   
    // Movement Basic
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
        if (isKnockBack) return;
        rb.linearVelocityX = moveInput * speed;
        if (jumpAction.IsPressed() && isGrounded())
        rb.linearVelocityY = jumpInput * jumpForce;
    }

    // Ground Check
    public bool isGrounded() {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, castDist, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.down * castDist, boxSize);
    }

    // Coin Collect
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            Debug.Log($"Da nhat {++coinCount}");
        }
    }

    // Player knock back when shooting
    public void shootKnockBack()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 knockBackDirection = (Vector2)mousePosInWorld - (Vector2)transform.position;

        StopCoroutine(nameof(PCoroutine));
        StartCoroutine(PCoroutine(knockBackDirection));
    }

    private IEnumerator PCoroutine(Vector2 knockBackDirection)
    {
        isKnockBack = true;
        rb.linearVelocity = -knockBackDirection * knockBackForce;

        yield return new WaitForSeconds(timeKnockBack);
        isKnockBack = false;
    }
}
