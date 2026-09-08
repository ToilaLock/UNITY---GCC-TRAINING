**1. TRIGGER**
Vùng kích hoạt logic không tạo ra lực cản vật lý, cho phép các đối tượng đi xuyên qua nhau.

Điều kiện hoạt động:
- Cả 2 GameObject đều phải gắn Collider 2D, trong đó ít nhất một bên tích chọn Is Trigger = true.
- Ít nhất một trong hai đối tượng bắt buộc phải có Rigidbody 2D.

Tham số nhận vào: Collider2D other (trả về tham chiếu đến Collider của đối tượng vừa chạm vào).