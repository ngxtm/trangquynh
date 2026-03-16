1/ Tạo folder trong Assets/Item, đặt tên cho folder là tên item

2/ Chuột phải trong folder vừa tạo => Create => New Item ![alt text](image/Screenshot2.png)

3/ Đổi tên cho cái New Item vừa tạo thành tên của vật thể đó

4/ Có 2 TH:

4.1 TH1: Vật thể cần picked up đã có sẵn trên scene
- Chỉnh thông số của vật thể đó như trong bước 5
- Kéo vật thể từ scene xuống folder trong Assets/Items/xxx vừa tạo

4.2 TH2: Vật thể chưa có sẵn trên scene
- Kéo từ prefab lên scene
- Chỉnh các bước như ở bước 5
- Kéo vật thể từ scene xuống folder trong Assets/Items/xxx vừa tạo

5/

5.1 Thêm rigidbody:
- Interpolate: Interpolate
- Collusion detection: Continuous Dynamic

5.2 Thêm Mesh Collier
- Convex: tick

5.3 Thêm Item Script
- Kéo Item từ folder Assets/Item/xxx ban đầu mới tạo vào ô đó (lưu ý không kéo prefab)

6/ Thêm icon cái giỏ vào folder Assets/Item/xxx
- Lưu ý: icon phải được tách nền trước khi đưa vào game
- Chọn icon => Texture type: sprite => Sprite mode: single => Apply ![alt text](image/Screenshot3.png)
- Chọn item xxx trong Assets/Item/xxx kéo icon vừa mới sprite xong vào ô icon
