
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
    
- **`OnEnable()`:** Gọi **mỗi lần** GameObject hoặc Component chuyển từ trạng thái Tắt sang Bật (`active = true`). Thường dùng để đăng ký sự kiện (Event).
    
- **`Start()`:** Gọi **1 lần duy nhất** trước frame đầu tiên của game (chỉ chạy khi script đang Bật). Dùng để lấy Component khác hoặc liên kết dữ liệu giữa các object.
    
- **`FixedUpdate()`:** Chạy theo **chu kỳ thời gian cố định** (mặc định 0.02s, không phụ thuộc FPS). Bắt buộc dùng cho mọi tính toán vật lý liên quan đến `Rigidbody`.
    
- **`Update()`:** Chạy **ở mỗi khung hình (frame)**. Phụ thuộc vào FPS của máy. Dùng để bắt phím bấm của người chơi (`Input`) và tính toán chuyển động thông thường.
    
- **`LateUpdate()`:** Chạy ở mỗi khung hình, nhưng **luôn luôn sau khi tất cả hàm `Update()` khác chạy xong**. Dùng để tính toán Camera bám theo nhân vật nhằm tránh hiện tượng giật hình (jitter).
    
- **`OnDisable()`:** Gọi **mỗi lần** GameObject hoặc Component bị tắt đi (`active = false`). Thường dùng để hủy đăng ký sự kiện tránh rò rỉ bộ nhớ.
    
- **`OnDestroy()`:** Gọi **1 lần cuối cùng** ngay trước khi GameObject bị xóa vĩnh viễn khỏi màn chơi (`Destroy(gameObject)`). Dùng để dọn dẹp tài nguyên.

**Sơ đồ trực quan**
	┌──────────────┐
	│   Awake()          │ ──► [Chạy 1 lần duy nhất khi nạp vào bộ nhớ]
	└──────┬───────┘
	       ▼
	┌──────────────┐
	│  OnEnable()      │ ──► [Chạy mỗi khi GameObject/Component được BẬT]
	└──────┬───────┘
	       ▼
	┌──────────────┐
	│   Start()             │ ──► [Chạy 1 lần trước frame đầu tiên]
	└──────┬───────┘
	       ▼
	 ╔═══════════════════════════════════════════════════════════╗
	 ║                     GAME LOOP (Lặp liên tục)                                            ║
	 ║                                                                                                           ║
	 ║  ┌─────────────────┐                                                                      ║
	 ║  │  FixedUpdate()       │ ──► [Chu kỳ cố định: Vật lý/Rigidbody]      ║
	 ║  └────────┬────────┘                                                                      ║
	 ║           ▼                                                                                             ║
	 ║  ┌─────────────────┐                                                                      ║
	 ║  │    Update()             │ ──► [Mỗi frame: Input / Logic thường]       ║
	 ║  └────────┬────────┘                                                                      ║
	 ║           ▼                                                                                             ║
	 ║  ┌─────────────────┐                                                                      ║
	 ║  │   LateUpdate()       │ ──► [Sau Update: Xử lý Camera bám]          ║
	 ║  └─────────────────┘                                                                       ║
	 ╚═══════════════════════════════════════════════════════════╝
	       │
	       ▼ (Khi tắt hoặc xóa đối tượng)
	┌──────────────┐
	│  OnDisable()     │ ──► [Chạy mỗi khi GameObject/Component bị TẮT]
	└──────┬───────┘
	       ▼
	┌──────────────┐
	│  OnDestroy()    │ ──► [Chạy 1 lần khi đối tượng bị HỦY hoàn toàn]
	└──────────────┘