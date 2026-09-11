using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Hàm nhận lực giật lùi từ đạn
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (!isKnockedBack)
        {
            StartCoroutine(KnockbackRoutine(direction.normalized, force, duration));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;

        // Gán vận tốc đẩy ngược lại theo hướng bay của viên đạn
        rb.linearVelocity = direction * force;

        // Đợi trong một khoảng thời gian ngắn
        yield return new WaitForSeconds(duration);

        // Triệt tiêu vận tốc giật lùi để trả lại quyền điều khiển
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    // Lưu ý: Trong code AI di chuyển của Enemy (ví dụ Update/FixedUpdate), 
    // bạn chỉ cho Enemy tự bước đi khi `!isKnockedBack`
}