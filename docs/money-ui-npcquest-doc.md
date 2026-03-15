# TÀI LIỆU HƯỚNG DẪN: HỆ THỐNG TIỀN & NHẬN THƯỞNG TỪ NPC

**Mô tả tổng quan:** Hệ thống này cho phép người chơi thu thập tiền (Vàng) thông qua việc tương tác với NPC. Khi thu thập đủ mốc yêu cầu (Mặc định: 50 Vàng), hệ thống sẽ tự động gọi sự kiện "Qua màn".

Hệ thống gồm 2 phần chính:
1. **`MoneyManager.cs`**: Quản lý tổng số tiền, cập nhật UI và kiểm tra điều kiện chiến thắng.
2. **`NPCQuest.cs`**: Xử lý nhận diện người chơi, nhận nút bấm (phím E) và gửi tiền về `MoneyManager`.

---

## PHẦN 1: SETUP NGƯỜI CHƠI (PLAYER)

Để NPC có thể nhận diện được người chơi đang đến gần, Player cần phải đáp ứng 2 điều kiện:

1. Chọn GameObject nhân vật chính, nhìn lên cùng của bảng Inspector, đổi **Tag** thành **`Player`**.
2. Nhân vật phải có ít nhất 1 Collider (Ví dụ: `Capsule Collider` hoặc `Character Controller`) và 1 `Rigidbody` (để tính toán va chạm vật lý).

---

## PHẦN 2: SETUP BẢNG ĐIỀU KHIỂN TIỀN (MONEY MANAGER)

1. Tạo một GameObject rỗng trong Scene, đặt tên là `GameManager` (hoặc gắn trực tiếp vào GameObject tổng chứa UI).
2. Kéo script **`MoneyManager`** thả vào GameObject này.
3. Thiết lập thông số trong Inspector:
   * **Target Money:** Để mặc định là `50` (Số tiền cần đạt để qua màn).
   * **Current Value Text:** Kéo GameObject UI Text hiển thị số tiền hiện tại vào đây.
   * **Target Value Text:** Kéo GameObject UI Text hiển thị số mục tiêu (50) vào đây.

---

## PHẦN 3: SETUP NPC TRẢ THƯỞNG (NPC QUEST)

Thực hiện các bước sau trên bất kỳ mô hình NPC nào bạn muốn biến thành người giao nhiệm vụ:

1. Kéo script **`NPCQuest`** thả vào mô hình NPC.
2. Tạo vùng tương tác (Vùng nhận diện):
   * Bấm `Add Component` -> Tìm và thêm **`Sphere Collider`**.
   * **Cực kỳ quan trọng:** Tích chọn vào ô vuông **`Is Trigger`**.
   * Chỉnh sửa thông số **Radius** to ra (khoảng `2` hoặc `3`) để tạo thành một vùng bao quanh NPC. Khi Player bước vào vùng này mới có thể bấm phím tương tác.
3. Liên kết dữ liệu trong Inspector của `NPCQuest`:
   * **Quest Name:** Đặt tên nhiệm vụ cho dễ nhớ (VD: *Tìm nhẫn cho trưởng làng*).
   * **Reward Money:** Chỉnh số tiền NPC sẽ thưởng (VD: `10`, `20`...).
   * **Player Wallet:** Kéo GameObject `GameManager` (đã tạo ở Phần 2) vào ô này để NPC biết nơi cần gửi tiền đến.

---

## PHẦN 4: CÁCH HOẠT ĐỘNG & TEST TRONG GAME

1. Điều khiển Player chạy lại gần NPC (bước vào vùng Sphere Collider).
2. Nhấn phím **`E`**.
3. Quan sát UI. Tiền sẽ được cộng thêm đúng bằng số `Reward