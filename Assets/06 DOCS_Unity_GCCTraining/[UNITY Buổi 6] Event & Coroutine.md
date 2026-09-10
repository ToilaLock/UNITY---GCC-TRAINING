## 1. Sự kiện (Event) trong C# & Unity

Cơ chế cho phép một đối tượng thông báo (phát tín hiệu) tới các đối tượng khác khi có một hành động xảy ra mà không cần phụ thuộc trực tiếp vào nhau (Decoupling / Loose Coupling).

### Tổng quan các loại Event

|Loại Event|Nguồn gốc|Ưu điểm|Nhược điểm|Trường hợp sử dụng|
|---|---|---|---|---|
|**Delegate**|C# thuần|Linh hoạt, nền tảng của mọi event trong C#|Cú pháp khai báo dài dòng|Tự xây dựng cấu trúc callback tùy chỉnh|
|**Action**|C# thuần (`System`)|Viết ngắn gọn, không cần định nghĩa delegate thủ công|Không hiển thị lên Unity Inspector|Xử lý logic thuần code giữa các hệ thống quản lý|
|**UnityEvent**|Unity Engine|Kéo thả trực quan trong Inspector, thân thiện với Level Designer|Chậm hơn Action/Delegate một chút về hiệu năng|Kích hoạt hiệu ứng UI, âm thanh, bẫy từ Inspector|

### Cú pháp Đăng ký và Gọi (Phát) Event

- **Toán tử đăng ký / hủy đăng ký:**
    - C# Delegate / Action: Dùng toán tử `+=` để lắng nghe và `-=` để hủy lắng nghe (luôn hủy ở `OnDisable`/`OnDestroy` để tránh rò rỉ bộ nhớ).
    - UnityEvent: Dùng `AddListener(...)` và `RemoveListener(...)` qua code, hoặc bấm dấu `+` trên Inspector.
    
- **Cách gọi (phát tín hiệu):** Dùng phương thức `.Invoke()` (hoặc `?.Invoke()` để kiểm tra null an toàn trước khi gọi).

```C#
using System;
using UnityEngine;
using UnityEngine.Events;

public class EventDemo : MonoBehaviour
{
    // 1. Delegate thuần
    public delegate void OnHealthChangedDelegate(int currentHp);
    public OnHealthChangedDelegate onHealthChanged;

    // 2. Action (C# built-in delegate, không trả về giá trị)
    public event Action<int> OnAmmoChanged;

    // 3. UnityEvent (Hiển thị được ra Inspector)
    [SerializeField] private UnityEvent onPlayerDeath;

    private void Start()
    {
        // --- ĐĂNG KÝ SỰ KIỆN (Subscribe) ---
        onHealthChanged += UpdateHealthUI;
        OnAmmoChanged += UpdateAmmoText;
        onPlayerDeath.AddListener(PlayDeathSound);

        // --- GỌI / PHÁT SỰ KIỆN (Invoke) ---
        onHealthChanged?.Invoke(80);
        OnAmmoChanged?.Invoke(30);
        onPlayerDeath?.Invoke();
    }

    private void OnDisable()
    {
        // --- HỦY ĐĂNG KÝ (Unsubscribe để tránh Memory Leak) ---
        onHealthChanged -= UpdateHealthUI;
        OnAmmoChanged -= UpdateAmmoText;
        onPlayerDeath.RemoveListener(PlayDeathSound);
    }

    private void UpdateHealthUI(int hp) => Debug.Log($"HP còn lại: {hp}");
    private void UpdateAmmoText(int ammo) => Debug.Log($"Đạn còn: {ammo}");
    private void PlayDeathSound() => Debug.Log("Phát âm thanh nhân vật chết!");
}
```

## 2. Coroutine trong Unity

Coroutine là một phương thức có khả năng tạm dừng việc thực thi tại một thời điểm nhất định và trả quyền kiểm soát lại cho Unity, sau đó tiếp tục chạy từ đúng điểm dừng ở các khung hình tiếp theo.

### Khái niệm luồng hoạt động
- Coroutine **không phải là đa luồng (Multi-threading)**; nó vẫn chạy trên **luồng chính (Main Thread)** của Unity.
- Khai báo bằng kiểu trả về: `IEnumerator`.
- Tạm dừng bằng từ khóa: `yield return`.

### Các câu lệnh `yield return` phổ biến

| Câu lệnh tạm dừng                             | Thời điểm tiếp tục thực thi                         | Ứng dụng thực tế                                    |
| --------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- |
| `yield return null;`                          | Chờ đến khung hình tiếp theo (Next frame)           | Làm hiệu ứng mượt (mờ dần Alpha, di chuyển Lerp)    |
| `yield return new WaitForSeconds(n);`         | Chờ đủ n giây (chịu ảnh hưởng bởi `Time.timeScale`) | Đếm ngược hồi chiêu đòn đánh, đạn tự hủy, delay bẫy |
| `yield return new WaitForSecondsRealtime(n);` | Chờ đủ n giây thời gian thực (bỏ qua Pause game)    | Animation bảng menu khi đang dừng màn chơi          |
| `yield break;`                                | Thoát vĩnh viễn khỏi Coroutine ngay lập tức         | Hủy chuỗi hành động khi mục tiêu đã chết            |

### Bắt đầu và Dừng Coroutine
- **Bắt đầu:** `StartCoroutine(TênHàm())` hoặc lưu vào một biến `Coroutine runningRoutine = StartCoroutine(...)`.
- **Dừng:**
    - `StopCoroutine(runningRoutine)`: Dừng chính xác một coroutine cụ thể đang chạy.
    - `StopAllCoroutines()`: Dừng toàn bộ các coroutine đang hoạt động trên GameObject đó.

```C#
using System.Collections;
using UnityEngine;

public class CoroutineDemo : MonoBehaviour
{
    private Coroutine flashRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Tránh chạy đè nếu coroutine trước đó chưa kết thúc
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashEffectRoutine(3, 0.2f));
        }
    }

    // Coroutine tạo hiệu ứng nhấp nháy
    private IEnumerator FlashEffectRoutine(int flashCount, float delay)
    {
        for (int i = 0; i < flashCount; i++)
        {
            Debug.Log("Bật hiển thị!");
            yield return new WaitForSeconds(delay); // Chờ 0.2 giây

            Debug.Log("Tắt hiển thị!");
            yield return new WaitForSeconds(delay); // Chờ 0.2 giây
        }

        Debug.Log("Kết thúc hiệu ứng nhấp nháy.");
        flashRoutine = null;
    }
}
```