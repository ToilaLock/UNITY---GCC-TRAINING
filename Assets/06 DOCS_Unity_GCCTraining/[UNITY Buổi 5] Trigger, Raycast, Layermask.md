## 1. Trigger trong Physics 2D

Vùng kích hoạt logic không tạo ra lực cản vật lý, cho phép các đối tượng đi xuyên qua nhau.

- **Điều kiện hoạt động:**
    - Cả 2 GameObject đều phải gắn **Collider 2D**, trong đó ít nhất một bên tích chọn `Is Trigger = true`.
    - Ít nhất một trong hai đối tượng bắt buộc phải có **Rigidbody 2D**.

- **Tham số nhận vào:** `Collider2D other` (trả về tham chiếu đến Collider của đối tượng vừa chạm vào).

| **Tên hàm**        | **Thời điểm kích hoạt**                          | **Tần suất gọi**             | **Ứng dụng thực tế**                      |
| ------------------ | ------------------------------------------------ | ---------------------------- | ----------------------------------------- |
| `OnTriggerEnter2D` | Đối tượng vừa bước chân vào vùng kích hoạt       | Gọi 1 lần duy nhất           | Nhặt coin, kích hoạt bẫy nổ, qua cửa ải   |
| `OnTriggerStay2D`  | Đối tượng vẫn đang đứng bên trong vùng kích hoạt | Gọi liên tục mỗi Fixed Frame | Vùng độc trừ máu theo giây, dòng nước đẩy |
| `OnTriggerExit2D`  | Đối tượng hoàn toàn rời khỏi vùng kích hoạt      | Gọi 1 lần duy nhất           | Tắt bảng hội thoại, rời khỏi vùng buff    |
```C#
// Ví dụ: Nhặt đồng xu khi Player đi vào
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Đã nhặt đồng xu!");
        Destroy(gameObject);
    }
}
```

## 2. Collision trong Physics 2D

Cơ chế va chạm vật lý có lực đẩy/cản cứng, ngăn không cho các vật thể đi xuyên qua nhau.

- **Điều kiện hoạt động:**
    - Cả 2 GameObject đều có **Collider 2D** (`Is Trigger = false`).
    - Ít nhất một bên có **Rigidbody 2D** (thường là `Dynamic`).
        
- **Tham số nhận vào:** `Collision2D collision` (chứa dữ liệu vật lý chi tiết: mảng điểm tiếp xúc `contacts`, vận tốc va đập `relativeVelocity`, GameObject va chạm).

| **Tên hàm**          | **Thời điểm kích hoạt**                  | **Tần suất gọi**             | **Ứng dụng thực tế**                      |
| -------------------- | ---------------------------------------- | ---------------------------- | ----------------------------------------- |
| `OnCollisionEnter2D` | Hai vật thể cứng vừa va chạm vào nhau    | Gọi 1 lần duy nhất           | Bị quái cắn, bóng nảy bật lại, va vào gai |
| `OnCollisionStay2D`  | Hai vật thể vẫn tiếp tục cọ xát/tiếp xúc | Gọi liên tục mỗi Fixed Frame | Đứng trên băng bị trơn trượt, mài cưa     |
| `OnCollisionExit2D`  | Hai vật thể vừa rời khỏi bề mặt tiếp xúc | Gọi 1 lần duy nhất           | Rời chân khỏi bệ đứng, hết chạm tường     |
```C#
// Ví dụ: Va chạm vật lý với gai nhọn gây sát thương
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Trap"))
    {
        Debug.Log("Va chạm gai nhọn! Giảm máu nhân vật.");
    }
}
```

## 3. Raycast trong Physics 2D

Kỹ thuật bắn tia hoặc quét hình học từ một tọa độ theo hướng vector xác định để kiểm tra va chạm với Collider 2D trên đường đi.

| **Phương thức quét**      | **Kiểu dữ liệu trả về** | **Cơ chế hoạt động**                             | **Trường hợp sử dụng**                       |
| ------------------------- | ----------------------- | ------------------------------------------------ | -------------------------------------------- |
| `Physics2D.Raycast`       | `RaycastHit2D`          | Bắn tia, dừng lại ngay khi chạm vật cản đầu tiên | Kiểm tra chạm đất (Ground Check), tia laser  |
| `Physics2D.RaycastAll`    | `RaycastHit2D[]`        | Bắn tia xuyên thấu qua mọi vật cản               | Đạn xuyên giáp, quét tầm nhìn nhiều mục tiêu |
| `Physics2D.OverlapCircle` | `Collider2D`            | Quét vùng hình tròn bán kính $R$ quanh một điểm  | Ground Check bán kính nhỏ, vùng nổ lựu đạn   |
| `Physics2D.OverlapBox`    | `Collider2D`            | Quét vùng hình hộp chữ nhật có góc xoay          | Hitbox chém kiếm cận chiến, vùng bệ đứng     |
```C#
// Ví dụ: Kiểm tra chạm đất (Ground Check)
[SerializeField] private Transform groundCheckPoint;
[SerializeField] private float rayDistance = 0.2f;
[SerializeField] private LayerMask groundLayer;

public bool IsGrounded()
{
    RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, rayDistance, groundLayer);
    return hit.collider != null;
}
```

## 4. LayerMask

Bộ lọc mặt nạ bit giúp các truy vấn vật lý chỉ tương tác với các Layer mong muốn và bỏ qua toàn bộ Layer khác.

| **Phương pháp cấu hình** | **Cú pháp triển khai**                      | **Ưu điểm**                                                       | **Nhược điểm**                              |
| ------------------------ | ------------------------------------------- | ----------------------------------------------------------------- | ------------------------------------------- |
| **Inspector Exposure**   | `[SerializeField] private LayerMask layer;` | Trực quan, chọn trực tiếp qua dropdown, không lo gõ sai tên chuỗi | Cần gắn qua Editor trên từng đối tượng      |
| **Hàm tiện ích API**     | `LayerMask.GetMask("Ground", "Enemy")`      | Dễ đọc, gom nhiều Layer vào cùng một Mask                         | Nếu gõ sai chính tả chuỗi thì mask trả về 0 |
| **Dịch bit (Bit-shift)** | `1 << LayerMask.NameToLayer("Ground")`      | Tối ưu hiệu năng mức bit, chuẩn lập trình lõi                     | Cú pháp phức tạp hơn với người mới          |
```C#
// Chuyển đổi tên layer "Ground" thành LayerMask bằng code
LayerMask mask = LayerMask.GetMask("Ground");

// Quét kiểm tra xem trong bán kính 0.5f có bề mặt "Ground" hay không
Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, mask);
if (hit != null)
{
    Debug.Log($"Phát hiện mặt đất: {hit.name}");
}
```