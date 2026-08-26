
**MỤC TIÊU:**
	- Nắm vững các từ khóa quan trọng trong C# (var, const/readonly, ref/out). 
	- Hiểu rõ vòng đời GameObject (Awake, OnEnable, Start, Update, FixedUpdate, LateUpdate, OnDisable, OnDestroy). 
	- Thao tác cơ bản trong Unity: Import Asset, Sprite Renderer (Sorting Layer/Order), Vector & Time.deltaTime. 
	- Sử dụng hàm toán học Mathf, điều khiển Transform, công cụ Gizmos và C# Attributes ([Range], [ExecuteInEditMode], [SerializeField]). 

**MỘT SỐ THÔNG TIN VỀ CLASS:**
	- Tham chiếu: truyền một biến vào hàm và nếu biến đó thay đổi trong hàm, biến đó sẽ bị thay đổi giá trị. VD: void nanNa(int &x)
	- Tham trị: truyền một biến vào hàm và nếu biến đó thay đổi trong hàm, biến ngoài hàm vẫn không đổi.
	- **Class chính là tham chiếu** (đại khái là thế)
	- Struct chính là tham trị (đại khái là thế)
	- **Public và private trong class:** 
		- Private bên trong Class không thể đụng tới
		- Public có thể đụng tới ở bên ngoài Class
	- **Constructor** - Hàm khởi tạo:
			```class Player{
				private int hp;
				private int attack;
				public Player (int hp, int attack){
					this hp = hp; this attack = attack;
				} 
			}```

**TẠI SAO CODE GAME NÊN DÙNG CLASS**
	Ví dụ trong lệnh: *P1.attack(P2)*
	- Nếu P2 là struct thì hàm attack sẽ được khởi tạo và gắn các biến của P2 vào, copy một vùng bộ nhớ mới tạo ra một P2(2), sau đó hàm attack chạy, sau khi chạy xong thì hàm biến mất và P2(2) cũng biến mất (nó là thứ duy nhất bị thay đổi và P2 vẫn giữ nguyên.
	- Nếu P2 là class thì hàm attack sẽ chạy với tất cả những gì class P2 được khởi tạo từ trước, sau đó hàm sẽ không bị xóa đi mất, các giá trị tại P2 sẽ bị thay đổi.

## 1. Từ khóa quan trọng trong C#

- `var`: kiểu dữ liệu tự động suy luận định dạng:
- VD: `var LocDang = "deptrai";` -> Tự hiểu là `string`

- `const`: giá trị cố định vĩnh viễn, không thể thay đổi (số ngày trong tuần, số Pi, Level tối đa)
- `readonly`: muốn một biến không bị sửa đổi lung tung sau khi tạo/thoát khỏi hàm, giá trị ban đầu cần phụ thuộc vào dữ liệu chạy game (VD: thông số nhân vật load từ điểm lưu trước)
	VD:
	```
		public class Player
		{
		// const
		public const int MAX_LEVEL = 100;
		public const float GRAVITY = 9.8f;```
		
		// readonly
		public readonly string playerId;
		public readonly int maxHealth;
		
		// Hàm khởi tạo (Constructor)
		public Player(string id, int customHp) {
			playerId = id; // Lần 1: Gán id ban đầu 
			playerId = "PREFIX_" + id; // Lần 2: Sửa lại/nối chuỗi hợp lệ 
			// readonly có thể sửa tùy ý trong hàm khởi tạo, sau đó thì không
			
			// MAX_LEVEL = 50;  // LỖI
		}
		
		public void UpdateStats() {
			// maxHealth = 200; // LỖI
		}
		}
	```

- - **`ref` (Truyền 2 chiều - Đọc & Ghi):**
    - Biến truyền vào **bắt buộc phải có giá trị từ trước**.
    - Bên trong hàm thích đọc hay sửa giá trị tùy ý, không bắt buộc phải gán lại.
- **`out` (Truyền 1 chiều ra - Chỉ Ghi):**
    - Biến truyền vào **không cần khởi tạo giá trị trước**.
    - Bên trong hàm **bắt buộc phải gán giá trị** cho biến trước khi thoát hàm (thường dùng khi muốn hàm trả về nhiều kết quả cùng lúc).
	```
	// ref: Cần giá trị ban đầu để sửa tiếp
	void TangMau(ref int hp) {
	    hp += 10;
	}
	
	// out: Không cần giá trị ban đầu, hàm tự tạo ra kết quả trả về
	void LayToaDo(out int x, out int y) {
	    x = 100; // Bắt buộc phải gán
	    y = 200; // Bắt buộc phải gán
	}
	```

## 2. VÒNG ĐỜI

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

## 3. TIME
- `Time.deltaTime`: Khoảng thời gian giữa 2 lần `Update()`
- `Time.fixedDeltaTime`: Khoảng thời gian giữa 2 lần `FixUpdate()`
- `Time.unscaledDeltaTime`: Khoảng thời gian giữa 2 lần `Update() nhưng không ảnh hưởng bởi `Time.Timescale`
- `Time.timeScale`: là khoảng thời gian game chạy, ví dụ: `Time.TimeScale` = 10, `playerSpeed` = 8 -> `playerSpeed` thực thụ là 80

## 4. THƯ VIỆN MATHF & DI CHUYỂN TRANSFORM
**Một số hàm trong Mathf**
- `Mathf.Clamp(float value, float min, float max)`: Giới hạn giá trị nằm trong khoảng từ min đến max (thường dùng để khóa máu không âm/vượt max, hoặc giới hạn góc quay camera).
- `Mathf.Lerp(float a, float b, float t)`: Nội suy tuyến tính giữa a và b theo tỉ lệ t ($0 \le t \le 1$), tạo hiệu ứng chuyển động hoặc đổi màu mượt mà.
- `Mathf.Abs(float f)`: Lấy giá trị tuyệt đối của một số.
- `Mathf.MoveTowards(float current, float target, float maxDelta)`: Tăng hoặc giảm dần từ current về target với bước nhảy cố định maxDelta mà không bao giờ vượt quá đích.
- `Mathf.PingPong(float t, float length)`: Tạo dao động qua lại tuần hoàn từ $0$ đến length (rất tiện để làm vật thể lắc lư, nhấp nháy đèn).
- `Mathf.RoundToInt / FloorToInt / CeilToInt`: Làm tròn số thực thành số nguyên (làm tròn gần nhất / làm tròn xuống / làm tròn lên).

**Một số hàm trong Transform**
- `transform.Translate(Vector3 translation)`: Di chuyển vật thể theo hướng và khoảng cách chỉ định (thường nhân với Time.deltaTime).
- `transform.Rotate(Vector3 eulers)`: Xoay vật thể quanh các trục X, Y, Z.
- `transform.LookAt(Transform target)`: Tự động xoay trục Z (mặt trước) của vật thể hướng thẳng về phía mục tiêu.
- `transform.SetParent(Transform parent)`: Gán đối tượng làm con của một Transform khác (hoặc truyền null để tách ra làm gốc).
- `transform.Find(string n)`: Tìm kiếm một đối tượng con theo tên nằm bên dưới hệ thống phân cấp của nó.
- `transform.position` vs `transform.localPosition`:
 	`position`: Tọa độ thực trong không gian thế giới (World Space).
 	`localPosition`: Tọa độ tương đối so với vật thể cha (Local Space).

