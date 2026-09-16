## 1. BỐN TRỤ CỘT CƠ BẢN CỦA OOP

- **Tính Đóng gói (Encapsulation):** Gom dữ liệu (biến) và các hành vi (hàm) liên quan vào trong một class, đồng thời che giấu thông tin nội bộ bằng các phạm vi truy cập (`private`, `protected`). Bên ngoài chỉ có thể tương tác thông qua các hàm/thuộc tính công khai (`public`), giúp bảo vệ dữ liệu không bị thay đổi tùy tiện.

- **Tính Kế thừa (Inheritance):** Cho phép một class con tái sử dụng toàn bộ thuộc tính và phương thức từ class cha mà không cần viết lại từ đầu (như cách một script kế thừa `MonoBehaviour` để có sẵn `transform`, `gameObject`).

- **Tính Đa hình (Polymorphism):** Cho phép các đối tượng khác nhau thực thi cùng một hành động theo những cách riêng biệt. Thường được triển khai qua việc nạp chồng phương thức (Method Overloading) hoặc ghi đè phương thức từ class cha (Method Overriding bằng `virtual` / `override`).

- **Tính Trừu tượng (Abstraction):** Tập trung vào việc thể hiện những tính năng cốt lõi của đối tượng và ẩn đi chi tiết cài đặt phức tạp bên dưới (sử dụng qua `interface` hoặc `abstract class`), giúp hệ thống giảm độ phụ thuộc và dễ mở rộng.


---

## 2. MONOBEHAVIOR VÀ VÒNG ĐỜI

**Monobehavior**: là lớp cơ sở (base class) trong Unity mà mọi C# script muốn gắn trực tiếp vào một **GameObject** đều phải kế thừa. Nó đóng vai trò là cầu nối giữa code C# và engine của Unity. 

Những hàm dưới đây thường được gọi trong script đều được kế thừa bởi "class cha" **Monobehavior**. Nếu không có Monobehavior, code C# trong Unity sẽ không thể hoạt động bình thường nếu gắn vào GameObject. Nếu muốn sẽ phải viết lại rất dài và phức tạp:

**Miêu tả ngắn gọn từng hàm**
- **`Awake()`:** Gọi **1 lần duy nhất** ngay khi script nạp vào bộ nhớ (kể cả khi component bị tắt). Dùng để tự khởi tạo biến nội bộ.
	+ GameObject phải bật thì Awake() mới chạy
	+ Script không chạy thì Awake() trong script vẫn chạy
    
- **`OnEnable()`:** Gọi **mỗi lần** GameObject hoặc Component chuyển từ trạng thái Tắt sang Bật (`active = true`). Thường dùng để đăng ký sự kiện (Event).
    
- **`Start()`:** Gọi **1 lần duy nhất** trước frame đầu tiên của game (chỉ chạy khi script đang Bật). Dùng để lấy Component khác hoặc liên kết dữ liệu giữa các object.
    
- **`FixedUpdate()`:** Chạy theo **chu kỳ thời gian cố định** (mặc định 0.02s, không phụ thuộc FPS). Bắt buộc dùng cho mọi tính toán vật lý liên quan đến `Rigidbody`.
    
- **`Update()`:** Chạy **ở mỗi khung hình (frame)**. Phụ thuộc vào FPS của máy. Dùng để bắt phím bấm của người chơi (`Input`) và tính toán chuyển động thông thường.
    
- **`LateUpdate()`:** Chạy ở mỗi khung hình, nhưng **luôn luôn sau khi tất cả hàm `Update()` khác chạy xong**. Dùng để tính toán Camera bám theo nhân vật nhằm tránh hiện tượng giật hình (jitter).
    
- **`OnDisable()`:** Gọi **mỗi lần** GameObject hoặc Component bị tắt đi (`active = false`). Thường dùng để hủy đăng ký sự kiện tránh rò rỉ bộ nhớ.
    
- **`OnDestroy()`:** Gọi **1 lần cuối cùng** ngay trước khi GameObject bị xóa vĩnh viễn khỏi màn chơi (`Destroy(gameObject)`). Dùng để dọn dẹp tài nguyên.

---
## 3. TẢN MẠN VỀ ASSET
![[Buổi 3 png1.png]]
Nhìn hình và nhớ 😋

---
## 4. Transform
Là 1 component bắt buộc phải có trong mỗi `GameObject`, tạo GameObject là tự có.
![[Buổi 3 png2.png]]

Dùng để xác định vị trí, di chuyển, xoay `GameObject`:
- `transform.position`: Vị trí / Tọa độ
- `transform.rotation`: Góc quay 
- `transform.localScale`: Kích thước tỉ lệ  `x;y;z`.

Unity phân biệt hai hệ tọa độ riêng: `position` và `localPosition`.
- `position`: Là tọa độ theo toàn không gian thế giới.
- `localPosition`: Là tọa độ tương đối so với Parent của nó.
=> Nói chung lúc đầu khi chưa chạy, `player` có thể ở một chỗ, nhưng khi chạy theo chương trình, gọi localPosition thì `player` có thể teleport sang chỗ khác theo script.

Các hàm thao tác với Transform thường dùng:
- `transform.Translate`: Dịch chuyển GameObject theo một hướng và một khoảng cách xác định.
- `transform.Rotate`: Xoay GameObject theo các góc quay cho trước.
- `transform.LookAt`: Tự động xoay mặt/hướng của GameObject nhìn thẳng về phía một đối tượng hoặc vị trí mục tiêu.
- `transform.SetParent`: Thay đổi hoặc gán quan hệ cha - con cho `GameObject` (dùng khi nhặt item vào inventory hoặc thả ra). END

---

## 5. TIME
- `Time.deltaTime`: Khoảng thời gian giữa 2 lần `Update()`
- `Time.fixedDeltaTime`: Khoảng thời gian giữa 2 lần `FixUpdate()`
- `Time.unscaledDeltaTime`: Khoảng thời gian giữa 2 lần `Update()` nhưng không ảnh hưởng bởi `Time.Timescale`
- `Time.timeScale`: là khoảng thời gian game chạy, ví dụ: `Time.TimeScale` = 10, `playerSpeed` = 8 -> `playerSpeed` thực thụ là 80 -> Có thể chỉnh ở **Project Setting**

---

## 6. MATHF

Đa số dùng để xử lý Logic game

**Một số hàm trong Mathf**:.

| Hàm                                                              | Công dụng                                                                                                                    |
| ---------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| `Mathf.Clamp(float value, float min, float max)`                 | Giới hạn giá trị nằm trong khoảng từ min đến max (thường dùng để khóa máu không âm/vượt max, hoặc giới hạn góc quay camera). |
| `Mathf.Lerp(float a, float b, float t)`                          | Nội suy tuyến tính giữa a và b theo tỉ lệ t ($0 \le t \le 1$), tạo hiệu ứng chuyển động hoặc đổi màu mượt mà.                |
| `Mathf.Abs(float f)`                                             | Lấy giá trị tuyệt đối của một số.                                                                                            |
| `Mathf.MoveTowards(float current, float target, float maxDelta)` | Tăng hoặc giảm dần từ current về target với bước nhảy cố định maxDelta mà không bao giờ vượt quá đích.                       |
| `Mathf.PingPong(float t, float length)`                          | Tạo dao động qua lại tuần hoàn từ $0$ đến length (rất tiện để làm vật thể lắc lư, nhấp nháy đèn).                            |
| `Mathf.RoundToInt / FloorToInt / CeilToInt`                      | Làm tròn số thực thành số nguyên (làm tròn gần nhất / làm tròn xuống / làm tròn lên).                                        |

---
## 7. VECTOR
**Một số loại vector cơ bản**
- `Vector2(x,y)`: Dùng cho 2 chiều
- `Vector3(x,y,z)`: Dùng cho 3 chiều

**Các hướng Vector có sẵn trong Unity:** 

| Hướng             | Tọa độ       |
| ----------------- | ------------ |
| `Vector3.zero`    | `(0, 0, 0)`  |
| `Vector3.one`     | `(1, 1, 1)`  |
| `Vector3.up`      | `(0, 1, 0)`  |
| `Vector3.down`    | `(0, -1, 0)` |
| `Vector3.right`   | `(1, 0, 0)`  |
| `Vector3.left`    | `(-1, 0, 0)` |
| `Vector3.forward` | `(0, 0, 1)`  |
| `Vector3.back`    | `(0, 0, -1)` |


**Các phép toán với Vector trong Unity:**

| Phép Toán                                           | Công dụng                                                                                                                                                                                                                                                                |
| --------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Vector3.Distance(position 1, position 2)`          | Tính khoảng cách giữa 2 `Object`.                                                                                                                                                                                                                                        |
| `.normalized`                                       | Dùng để chuẩn hóa Vector thành 1 Vector hướng (Vector đơn vị). `Vector3 direction = direction.normalized` -> Hữu ích khi cần di chuyển chéo                                                                                                                              |
| `Vector3.Lerp(position 1, position 2, float tỉ lệ)` | Dùng để làm mượt chuyển động. Chuyển động từ position 1 -> position 2 với khoảng cách là `tỉ lệ`, tức là sẽ di chuyển được `tỉ lệ`% quãng đường khi được gọi, khi được dùng trong hàm `Update()` và được nhân với `deltaTime` thì sẽ tạo được hiệu ứng chuyển động mượt. |

---

## 8. GIZMOS
**Gizmos** nói chung là vẽ lên Scene mà không tác động gì đến game

**Hai hàm sự kiện chính của Gizmos**
- **`OnDrawGizmos()`:** Được gọi liên tục mỗi khi cửa sổ Scene cập nhật, vẽ Gizmos bất kể GameObject có được chọn hay không.

- **`OnDrawGizmosSelected()`:** Chỉ vẽ Gizmos khi GameObject chứa script đó đang được click chọn trong Hierarchy/Scene (giúp cửa sổ Scene không bị rối khi có quá nhiều đối tượng).

**Các hàm vẽ thông dụng**

| Hàm Gizmos                              | Công dụng thực tế                                                                 |
| --------------------------------------- | --------------------------------------------------------------------------------- |
| `Gizmos.DrawWireCube(center, size)`     | Vẽ khung viền hộp / ô túi đồ / vùng biên giới hạn (Boundary).                     |
| `Gizmos.DrawCube(center, size)`         | Vẽ khối hộp đặc.                                                                  |
| `Gizmos.DrawWireSphere(center, radius)` | Vẽ vòng tròn bán kính tầm đánh, tầm nhìn phát hiện của AI hoặc vùng kích hoạt nổ. |
| `Gizmos.DrawSphere(center, radius)`     | Đánh dấu tâm điểm, điểm mốc (Waypoint)                                            |
| `Gizmos.DrawLine(from, to)`             | Nối đường thẳng giữa hai đối tượng hoặc vẽ đường Raycast kiểm tra va chạm.        |
| `Gizmos.DrawRay(from, direction)`       | Vẽ tia bắn từ vị trí xuất phát theo một hướng và độ dài vector.                   |
| `Gizmos.DrawIcon(center, name)`         | Hiển thị một icon ảnh tùy chỉnh ngay tại vị trí trong Scene.                      |

---