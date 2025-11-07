# LeeTour Application Requirements

## Admin Requirements

### Authentication
- Need to use login page to able see dashboard
- Remove current tours page without login

### Tour Management

#### 1. Tour Pricing & Options
Admin => Tour => Thêm Nhiều loại giá. Tour Option1 (Tên option, Giá tiền), số lượng khách book ảnh hưởng trực tiếp đến giá tiền, vì liên quan tới đặt xe, đặt phòng chung và riêng.

- **Surcharge (Phụ thu)**: Phụ thu các ngày lễ tết, cuối tuần, nếu booking trùng dịp này
- **Promotion**: Early Bird, Last Minutes, Giảm % hoặc giảm số tiền
- **Cancellation Policy**: Chính sách hủy tour

#### 2. Tour Images
Hình ảnh Tour, gồm:
- Hình đại diện (Featured Image)
- 3 hình để chạy slider banner bên trên tour

#### 3. Tour Types & Itinerary
Tour phân làm 2 loại:
- **Daytrip**: Tour trong ngày
- **Tour**: Tour nhiều ngày => Thêm được hành trình của từng ngày. Gồm header và textdetail

#### 4. Review System
Cho viết reviews và hiển thị review.

**Lưu ý**: Nên chuẩn hóa bước validation google captcha để chống spam DOSS

#### 5. Receipt Functionality
Trang admin thêm một chức năng receipt

#### 6. Staff Payment System
Thêm một chức năng cho nhân viên thanh toán tour mà không tích điểm cho khách

---

## Implementation Status

All requirements have been fully implemented. See [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) and [REQUIREMENTS_CHECKLIST.md](REQUIREMENTS_CHECKLIST.md) for details.
