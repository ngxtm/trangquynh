1/ Chuẩn bị sẵn các thứ sau

- NPC đã có model trong scene (VD: `CoBanHang`)
- Đã có `MoneyManager` trong scene
- Đã có `Inventory` trong scene
- Đã có 3 item asset:
  - `Assets/Item/CaiGio/CaiGio.asset`
  - `Assets/Item/DoiDua/chopstick.asset`
  - `Assets/Item/CaiCuoc/CaiCuoc.asset`

2/ Tạo conversation intro cho NPC

- Tạo object conversation đầu tiên cho NPC, ví dụ đặt tên là `CoBanHangConversation`
- Gắn `NPC Conversation (Script)` vào object đó
- Viết lời thoại mở đầu ở đây
- Ở option nhận nhiệm vụ nên có 1 câu kiểu như: `Vâng ! Cô hãy đợi cháu nhé`

3/ Tạo trigger để nói chuyện

- Tạo child của `CoBanHangConversation`, đặt tên là `ConversationTrigger`
- Gắn `ConversationStarter` vào child này
- Gắn `Sphere Collider` vào child này
- Tick `Is Trigger`
- Chỉnh `Radius` to vừa đủ để player đứng gần là bấm nói chuyện được
- Trong `ConversationStarter`, kéo `CoBanHangConversation` vào ô `My Conversation`

4/ Tạo các conversation còn lại

- Tạo thêm 3 conversation nữa:
  - `CoBanHangReminderConversation`
  - `CoBanHangCompleteConversation`
  - `CoBanHangAfterQuestConversation`

4.1 Với `CoBanHangReminderConversation`
- Đây là hội thoại dùng khi người chơi đã nhận nhiệm vụ nhưng chưa đủ đồ
- Ví dụ nội dung: nhắc người chơi đi tìm đủ 3 món

4.2 Với `CoBanHangCompleteConversation`
- Đây là hội thoại dùng lúc người chơi mang đủ đồ quay lại
- Hội thoại này sẽ hiện sau khi script tự kiểm tra đủ item và tự trả thưởng

4.3 Với `CoBanHangAfterQuestConversation`
- Đây là hội thoại dùng sau khi nhiệm vụ đã xong hẳn
- Có thể là câu cảm ơn ngắn

4.4 Lưu ý rất quan trọng
- Nếu duplicate từ `CoBanHangConversation` để làm 3 conversation còn lại thì phải xóa hoặc tắt child `ConversationTrigger` ở bản copy
- Trong scene chỉ nên có 1 `ConversationTrigger` active để nói chuyện với NPC đó

5/ Tạo UI checklist nhiệm vụ

- Vào `Canvas_Inventory`
- Tìm object `Inventory`
- Tạo 1 UI Panel con ở trong đó, đặt tên là `MissionChecklistPanel`
- Gắn script `MissionChecklistUI` vào `MissionChecklistPanel`

6/ Tạo cây con cho `MissionChecklistPanel`

- Tạo `Title` để hiện chữ `Nhiệm vụ`
- Tạo 3 dòng item:
  - `ItemRow_Bag`
  - `ItemRow_Chopstick`
  - `ItemRow_Hoe`

6.1 Bên trong từng row tạo 2 object con
- 1 object text để hiện tên item
- 1 object tick để hiện dấu hoàn thành

6.2 Text nên điền như sau
- `Title` = `Nhiệm vụ`
- Label dòng 1 = `Cái giỏ`
- Label dòng 2 = `Đôi đũa`
- Label dòng 3 = `Cái cuốc`

6.3 Tick có thể làm theo 2 cách
- Dùng `Image` nếu đã có icon tick
- Hoặc dùng TMP Text với ký tự `✓`

6.4 Lưu ý
- Nên để `MissionChecklistPanel` tắt ban đầu
- Tick của 3 dòng cũng nên tắt ban đầu

7/ Gắn reference cho `MissionChecklistUI`

- `Panel Root` => kéo `MissionChecklistPanel`
- `Title Text` => kéo object text của Title
- `Bag Label` => kéo label của dòng `Cái giỏ`
- `Chopstick Label` => kéo label của dòng `Đôi đũa`
- `Hoe Label` => kéo label của dòng `Cái cuốc`
- `Bag Tick` => kéo object tick của dòng `Cái giỏ`
- `Chopstick Tick` => kéo object tick của dòng `Đôi đũa`
- `Hoe Tick` => kéo object tick của dòng `Cái cuốc`

8/ Gắn script mission cho NPC

- Gắn `Mission1Controller` vào đúng object `CoBanHangConversation/ConversationTrigger`
- Không gắn vào model `CoBanHang` nếu hệ hiện tại của bạn đang để trigger ở child

9/ Gắn reference cho `Mission1Controller`

9.1 Phần References
- `Conversation Starter` => kéo component `ConversationStarter` ở `ConversationTrigger`
- `Inventory` => kéo object đang giữ script `Inventory`
- `Money Manager` => kéo object đang giữ script `MoneyManager`
- `Checklist UI` => kéo `MissionChecklistPanel`

9.2 Phần Mission Items
- `Bag Item` => kéo `Assets/Item/CaiGio/CaiGio.asset`
- `Chopstick Item` => kéo `Assets/Item/DoiDua/chopstick.asset`
- `Hoe Item` => kéo `Assets/Item/CaiCuoc/CaiCuoc.asset`

9.3 Phần Conversations
- `Intro Conversation` => kéo `CoBanHangConversation`
- `Reminder Conversation` => kéo `CoBanHangReminderConversation`
- `Complete Conversation` => kéo `CoBanHangCompleteConversation`
- `After Quest Conversation` => kéo `CoBanHangAfterQuestConversation`

9.4 Phần Reward
- `Reward Money` => nhập số tiền muốn thưởng

10/ Gắn event cho option nhận nhiệm vụ

- Mở `CoBanHangConversation`
- Tìm đúng option kiểu `Vâng ! Cô hãy đợi cháu nhé`
- Ở phần `Event` của option đó, thêm 1 event mới
- Target phải là object `CoBanHangConversation/ConversationTrigger`
- Chọn hàm `Mission1Controller -> MarkIntroAccepted()`
- Nên để mode là `Runtime`

11/ Có thể bỏ `NPCQuest` cũ

- Nếu NPC này trước đó đang dùng `NPCQuest` để trả tiền thủ công thì nên disable hoặc remove script đó đi
- Vì mission mới đã dùng `Mission1Controller` + `MoneyManager` rồi
- Nếu để cả 2 cùng chạy dễ bị chồng logic

12/ Cách hệ này hoạt động

- Player đến gần `ConversationTrigger` và bấm `F`
- `ConversationStarter` sẽ mở hội thoại intro
- Khi người chơi chọn option nhận nhiệm vụ, event sẽ gọi `MarkIntroAccepted()`
- Khi hội thoại kết thúc, `Mission1Controller` sẽ chuyển mission sang trạng thái đang làm và hiện checklist
- Trong lúc nhặt item, checklist sẽ tự tick theo item trong inventory
- Khi đủ 3 món và quay lại nói chuyện, script sẽ tự:
  - trừ 3 item
  - cộng tiền
  - ẩn checklist
  - mở `Complete Conversation`
- Sau đó các lần nói chuyện tiếp theo sẽ mở `AfterQuest Conversation`

13/ Test nhanh

13.1 Test nhận nhiệm vụ
- Play game
- Đi tới NPC
- Bấm `F`
- Chọn option nhận nhiệm vụ
- Thoát hội thoại ra
- Kiểm tra `MissionChecklistPanel` đã hiện chưa

13.2 Test tick checklist
- Nhặt lần lượt `Cái giỏ`, `Đôi đũa`, `Cái cuốc`
- Kiểm tra từng tick có hiện đúng không

13.3 Test trả nhiệm vụ
- Mang đủ 3 món quay lại NPC
- Bấm `F`
- Kiểm tra item có bị trừ không
- Kiểm tra tiền có cộng không
- Kiểm tra checklist có ẩn không
- Kiểm tra hội thoại complete có hiện không

14/ Nếu bị lỗi thì kiểm tra mấy chỗ này trước

- Option nhận nhiệm vụ đã gắn đúng event chưa
- Event có target đúng `ConversationTrigger` chưa
- `Mission1Controller` có đang nằm cùng object với `ConversationStarter` không
- `Checklist UI` đã kéo đúng `MissionChecklistPanel` chưa
- 3 ô `Bag Tick`, `Chopstick Tick`, `Hoe Tick` có kéo đúng object không
- 3 item asset có kéo đúng `CaiGio.asset`, `chopstick.asset`, `CaiCuoc.asset` không
- Có bị để 2 `ConversationTrigger` active cùng lúc không
- NPC có còn giữ `NPCQuest` cũ làm chồng logic không