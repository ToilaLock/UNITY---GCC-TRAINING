### 1. Animator Component

Là một Component gắn trực tiếp lên GameObject để kết nối mô hình/sprite trong Scene với hệ thống hoạt ảnh (Mecanim).

- **Vai trò:**
    - Tiếp nhận dữ liệu điều khiển từ **Animator Controller**.
    - Quản lý trạng thái phát hoạt ảnh thời gian thực, đồng bộ vị trí xương (rig) hoặc thay đổi Sprite trên từng frame.
    - Cung cấp API để lập trình viên giao tiếp qua code C# (ví dụ: `animator.SetBool()`, `animator.SetFloat()`, `animator.SetTrigger()`).

- **Các thuộc tính quan trọng trên Inspector:**
    - `Controller`: Kéo file `Animator Controller` vào đây.
    - `Avatar`: Dùng cho mô hình 3D Humanoid/Generic (trong 2D mục này thường để `None`).
    - `Apply Root Motion`: Cho phép hoạt ảnh tự di chuyển vị trí Transform của vật thể hay giữ nguyên để script code điều khiển.
    - `Update Mode`: Chế độ cập nhật (Normal theo `Time.deltaTime`, Animate Physics theo `FixedUpdate`, hoặc Unscaled Time khi game pause).
    - `Culling Mode`: Tắt tính toán animation khi nhân vật ra khỏi tầm nhìn camera để tối ưu hiệu năng.

### 2. Animation Clip

Là một tệp tài nguyên tĩnh (Asset có đuôi `.anim`), lưu trữ chuỗi dữ liệu chuyển động theo dòng thời gian (Timeline).

- **Đặc điểm & Cấu tạo:**
    - **2D Sprite Animation:** Chứa mốc thời gian (Keyframes) để đổi qua lại giữa các frame ảnh Sprite (Run, Jump, Attack, Idle).
    - **Thuộc tính tùy biến:** Ngoài đổi Sprite, một Clip có thể animate hầu hết các thuộc tính của Component khác (màu sắc `Color`, tọa độ `Position`, độ mờ `Alpha`, bật/tắt `BoxCollider2D`).
    - **Animation Events:** Cho phép đặt các điểm mốc (marker) ngay trên thanh thời gian để kích hoạt một hàm C# cụ thể (ví dụ: phát âm thanh bước chân ở giây thứ 0.2, sinh hitbox chém kiếm ở giây thứ 0.4).

- **Thiết lập cơ bản:** Cần chú ý cờ **Loop Time** trong Inspector của file `.anim` (bật đối với hoạt ảnh lặp như chạy/thở, tắt đối với hoạt ảnh 1 lần như chết/nhảy/bắn).


### 3. Animator Controller

Là một tài nguyên dạng máy đồ thị (Asset đuôi `.controller`), hoạt động như "bộ não" điều phối toàn bộ các Animation Clip của một nhân vật.

- **Vai trò & Cấu trúc:**
    - Chứa các **Node (State)** đại diện cho các hành động (Idle, Walk, Attack, Die).
    - Chứa các mũi tên chuyển trạng thái (**Transitions**) nối giữa các Node.
    - Quản lý các biến điều kiện (**Parameters**) gồm 4 kiểu dữ liệu:
        - `Float`: Tốc độ di chuyển, vận tốc rơi.
        - `Int`: Số combo đòn đánh, ID vũ khí.
        - `Bool`: `isGrounded`, `isDead`, `isCrouching`.
        - `Trigger`: Kích hoạt một hành động tức thời như `Shoot`, `Jump`, `Hurt` (tự động reset về false sau khi chuyển trạng thái).

- **Cài đặt Transition:**
    - `Has Exit Time`: Nếu tích chọn, hoạt ảnh cũ phải chạy xong (hoặc đến % nhất định) mới được chuyển sang hoạt ảnh mới. Với game 2D Platformer/Action đòi hỏi phản hồi bấm phím tức thì (như Nhảy, Bắn), mục này thường phải **bỏ tích**.
    - `Transition Duration`: Thời gian hòa trộn giữa 2 clip (trong 2D Sprite thường đặt về `0` để đổi hình lập tức, tránh bị nhòe).

### 4. Blend Tree

Là một State đặc biệt bên trong Animator Controller, cho phép **hòa trộn mượt mà nhiều Animation Clip cùng lúc** dựa trên một hoặc nhiều tham số (`Float`).

- **Khác biệt với State thường:**
    - State thường chỉ phát đúng 1 Animation Clip tại một thời điểm.
    - Blend Tree nội suy giữa các clip dựa trên trọng số giá trị.

- **Ứng dụng thực tế:**
    - **1D Blend Tree (Dùng 1 tham số):** Hòa trộn tốc độ di chuyển dọc trục ngang. Tham số `Speed`:
        - `Speed = 0` $\rightarrow$ phát 100% clip _Idle_.
        - `Speed = 2.5` $\rightarrow$ phát 50% _Idle_ + 50% _Walk_.
        - `Speed = 5` $\rightarrow$ phát 100% clip _Run_.
    - **2D Blend Tree (Dùng 2 tham số - `InputX`, `InputY`):** Cực kỳ phổ biến cho game Top-down (di chuyển 8 hướng) hoặc ngắm bắn tự do:
        - Điều khiển hướng di chuyển theo vector chuột/joystick mà không cần tạo hàng chục mũi tên Transition phức tạp.

### 5. Finite State Machine (FSM - Máy trạng thái hữu hạn)

Là mô hình toán học/logic dùng để thiết kế hành vi của hệ thống. Nguyên tắc cốt lõi: **Tại một thời điểm cụ thể, đối tượng chỉ được ở DUY NHẤT một trạng thái (State)** và việc chuyển đổi giữa các trạng thái tuân theo các điều kiện xác định.

- **FSM trong Animator của Unity:**
    - Bản thân giao diện đồ thị của Animator Controller chính là một dạng trực quan hóa của FSM:
        - **State:** `Idle`, `Run`, `Jump`, `Attack`.
        - **Transitions:** Điều kiện chuyển (ví dụ: từ `Idle` sang `Run` khi biến `speed > 0.1f`).
        - **Entry / Any State:** `Entry` là trạng thái mặc định khi bắt đầu; `Any State` cho phép chuyển thẳng tới một trạng thái khẩn cấp từ bất kỳ đâu (ví dụ: bị dính đòn `Hurt` hoặc `Die`).

- **FSM ứng dụng bằng Code C# (Mở rộng cho Gameplay & AI):**
    - Thường dùng để quản lý logic hành vi nhân vật/Boss ngoài đời thực để tránh code `if-else` lộn xộn trong `Update()`.
    - Mô hình chuẩn: Tạo một Interface `IState` gồm `Enter()`, `Execute()`, `Exit()`. Khi nhân vật chuyển từ `PatrolState` sang `ChaseState`, code sẽ dọn dẹp trạng thái cũ và kích hoạt logic của trạng thái mới một cách an toàn và độc lập.