## 1. New Input System

Hệ thống xử lý đầu vào hiện đại thay thế **Input Manager (Legacy)**, tối ưu việc hỗ trợ đa nền tảng (Keyboard, Mouse, Gamepad, Touch).

**Kiến trúc cốt lõi**
- **Input Action Asset:** File cấu hình tập trung chứa toàn bộ hành động (Action) của trò chơi.
- **Action Maps:** Phân chia ngữ cảnh hành động để tránh xung đột phím (Ví dụ: `Gameplay`, `UI/Menu`, `Pause`).
- **Player Input Component:** Cầu nối gắn trực tiếp lên GameObject để chuyển tiếp sự kiện phím vào code C#.
## 2. Physics 2D

Hệ thống mô phỏng va chạm và các quy luật vật lý trong không gian hai chiều.

**Điều kiện xảy ra va chạm vật lý (`OnCollisionEnter2D`)**
1. Cả **2 đối tượng** đều phải có **Collider 2D** (`BoxCollider2D`, `CircleCollider2D`,...).
2. Ít nhất **1 trong 2 đối tượng** phải có **Rigidbody 2D**.

**Trigger & Raycast 2D**
Xác định cách phát hiện vật thể đi vào vùng kích hoạt hoặc quét tia kiểm tra va chạm:
- **Trigger (`Is Trigger = true`):** Không cản trở di chuyển, chỉ phát hiện khi có vật thể đi xuyên qua vùng chỉ định.
- **Raycast 2D:** Bắn một tia vô hình theo hướng xác định để kiểm tra va chạm (thường dùng làm Ground Check hoặc tầm nhìn AI).
```C#
// Xử lý khi đi vào vùng Trigger (nhặt đồ, vùng bẫy)
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log($"Nhặt được: {gameObject.name}");
        Destroy(gameObject);
    }
}

// Xử lý bắn Raycast kiểm tra chạm đất (Ground Check)
private bool CheckGround(Transform checkPos, float distance, LayerMask groundLayer)
{
    RaycastHit2D hit = Physics2D.Raycast(checkPos.position, Vector2.down, distance, groundLayer);
    return hit.collider != null;
}
```

## 3. Các phương pháp di chuyển nhân vật

**Dịch chuyển tọa độ (`Transform.Translate` / Thay đổi `position`):**
- _Đặc điểm:_ Thay đổi trực tiếp vị trí đối tượng. Dễ cài đặt, phản hồi tức thì.
- _Hạn chế:_ Bỏ qua tính toán vật lý, dễ gây lỗi xuyên thấu (Tunneling) qua vật cản.
- _Phù hợp:_ Game Top-down, Puzzle, Grid-based movement.
```C#
// 1. Dịch chuyển tọa độ trực tiếp (Update)
transform.position += (Vector3)moveInput * speed * Time.deltaTime;
```

**Thay đổi vận tốc (`Rigidbody2D.linearVelocity` / `velocity`):**
- _Đặc điểm:_ Gán vận tốc trực tiếp cho đối tượng trong `FixedUpdate()`.
- _Ưu điểm:_ Giữ nguyên tương tác vật lý chính xác, không bị kẹt hay xuyên tường.
- _Phù hợp:_ Game 2D Platformer, Runner.
```C#
// 2. Thay đổi vận tốc vật lý (FixedUpdate)
rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
```

**Tác động lực tức thời (`Rigidbody2D.AddForce`):**
- _Đặc điểm:_ Truyền xung lực (Impulse) bộc phát lên vật thể.
- _Phù hợp:_ Cơ chế Nhảy (`Jump`), Lướt (`Dash`), hoặc lực đẩy khi trúng đòn (Knockback).
```C#
// 3. Tác động lực tức thời (Jump / Dash)
rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
```

