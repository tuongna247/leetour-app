# 🧪 Pricing Management - Testing Guide

## ✅ Bước 3 & 4: Testing Complete Implementation

### 🎯 Mục tiêu Testing
1. Verify pricing page loads không có 500 error
2. Test save pricing data (tourOptions, surcharges, promotions, cancellationPolicies)
3. Verify data persistence
4. Test surcharge calculation trong booking flow

---

## 📋 Pre-requisites

### ✅ Đã hoàn thành:
- [x] PATCH handler đã được thêm vào API
- [x] Tour model có đầy đủ schemas
- [x] Dev server đang chạy tại `http://localhost:3000`

### 📦 Cần có trong database:
- Ít nhất 1 tour để test
- User account để login (admin role)

---

## 🧪 Test Cases

### Test Case 1: Pricing Page Load (No 500 Error)

**Mục đích**: Verify pricing page không còn bị 500 error

**Steps:**
1. Login vào admin dashboard
2. Go to Tour Management
3. Click vào một tour (hoặc tạo tour mới)
4. Click button "Manage Pricing"
5. Hoặc click icon 💰 trong tour list

**Expected Result:**
- ✅ Page load thành công
- ✅ Hiển thị 4 tabs: Pricing Options, Surcharges, Promotions, Cancellation Policies
- ✅ Tour information hiển thị ở header
- ✅ Save button visible
- ✅ KHÔNG CÓ 500 error

**Actual Result**: [Fill after testing]

---

### Test Case 2: Add Surcharge & Save

**Mục đích**: Test add surcharge và save data

**Steps:**
1. Truy cập pricing page của một tour
2. Click tab "Surcharges"
3. Click "Add Surcharge"
4. Fill in form:
   ```
   Surcharge Name: Weekend Surcharge
   Surcharge Type: Weekend
   Start Date: 2025-01-01
   End Date: 2025-12-31
   Amount Type: Percentage
   Amount: 20
   Description: Extra 20% for weekend bookings
   Active: ✓
   ```
5. Click "Save All Changes"

**Expected Result:**
- ✅ Success alert appears
- ✅ No errors in console
- ✅ Page doesn't crash

**Actual Result**: [Fill after testing]

---

### Test Case 3: Data Persistence

**Mục đích**: Verify data được lưu vào database

**Steps:**
1. Sau khi save surcharge ở Test Case 2
2. Refresh page (F5)
3. Check surcharge vẫn còn hiển thị

**Expected Result:**
- ✅ Surcharge data vẫn hiển thị
- ✅ All fields giữ nguyên giá trị
- ✅ Active status đúng

**Actual Result**: [Fill after testing]

---

### Test Case 4: Add Multiple Pricing Items

**Mục đích**: Test add nhiều loại pricing cùng lúc

**Steps:**
1. Tab "Pricing Options": Add 1 option
   ```
   Option Name: Small Group
   Base Price: 150
   Add tier: 1-2 people = $150/person
   Add tier: 3-4 people = $120/person
   ```

2. Tab "Surcharges": Add 2 surcharges
   ```
   Surcharge 1: Weekend (20%)
   Surcharge 2: Lunar New Year (30%)
   ```

3. Tab "Promotions": Add 1 promotion
   ```
   Name: Early Bird
   Type: Early Bird
   Discount: 15%
   Days Before: 30
   ```

4. Tab "Cancellation Policies": Add 3 policies
   ```
   30+ days: 100% refund
   14-29 days: 50% refund
   0-13 days: 0% refund
   ```

5. Click "Save All Changes"

**Expected Result:**
- ✅ All tabs save successfully
- ✅ Success message appears
- ✅ Data persists after refresh

**Actual Result**: [Fill after testing]

---

### Test Case 5: Surcharge Calculation in Booking

**Mục đích**: Test surcharge tự động apply trong booking flow

**Steps:**
1. Ensure tour có surcharge:
   - Weekend Surcharge: 20% (Sat-Sun all year)
   - Lunar New Year: 30% (Jan 28 - Feb 5)

2. Go to booking page: `/tours/[tourId]/booking`

3. Select date trong weekend (e.g., Saturday)

4. Check pricing breakdown

**Expected Result:**
- ✅ Warning alert hiển thị: "Surcharges Apply to Selected Date"
- ✅ List surcharges applicable
- ✅ Price breakdown shows:
   ```
   Subtotal: $X
   Weekend Surcharge (+20%): $Y
   Total Surcharges: $Y
   Taxes: $Z
   Total: $Total
   ```

**Actual Result**: [Fill after testing]

---

### Test Case 6: Edit Existing Pricing Data

**Mục đích**: Test edit functionality

**Steps:**
1. Open pricing page của tour đã có data
2. Edit một surcharge (thay đổi amount từ 20% → 25%)
3. Click Save
4. Refresh page
5. Verify change được lưu

**Expected Result:**
- ✅ Edit thành công
- ✅ New value hiển thị sau refresh

**Actual Result**: [Fill after testing]

---

### Test Case 7: Delete Pricing Item

**Mục đích**: Test delete functionality

**Steps:**
1. Click delete icon trên một surcharge
2. Click Save
3. Refresh page
4. Verify surcharge đã bị xóa

**Expected Result:**
- ✅ Item deleted
- ✅ Không còn hiển thị sau refresh

**Actual Result**: [Fill after testing]

---

### Test Case 8: Toggle Active/Inactive

**Mục đích**: Test active toggle

**Steps:**
1. Set surcharge active = false
2. Save
3. Go to booking page
4. Select date trong surcharge period

**Expected Result:**
- ✅ Surcharge KHÔNG apply (vì inactive)
- ✅ No warning alert
- ✅ Price normal

**Actual Result**: [Fill after testing]

---

## 🔍 API Endpoint Tests

### Test PATCH Endpoint Directly

**Using cURL:**

```bash
# Get tour ID first
curl http://localhost:3000/api/admin/tours?limit=1

# Then PATCH with tour ID
curl -X PATCH http://localhost:3000/api/admin/tours/[TOUR_ID] \
  -H "Content-Type: application/json" \
  -d '{
    "surcharges": [{
      "surchargeName": "Test Surcharge",
      "surchargeType": "weekend",
      "startDate": "2025-01-01",
      "endDate": "2025-12-31",
      "amountType": "percentage",
      "amount": 15,
      "isActive": true
    }]
  }'
```

**Expected Response:**
```json
{
  "status": 200,
  "data": { /* updated tour object */ },
  "msg": "Tour pricing updated successfully"
}
```

---

## 📊 Test Results Summary

### API Tests
- [ ] GET /api/admin/tours/[id] - Returns tour with pricing fields
- [ ] PATCH /api/admin/tours/[id] - Updates pricing data
- [ ] Data persists after PATCH
- [ ] No 500 errors

### UI Tests
- [ ] Pricing page loads successfully
- [ ] All 4 tabs render correctly
- [ ] Forms work properly
- [ ] Save button functional
- [ ] Alert notifications work

### Data Flow Tests
- [ ] Save → Refresh → Data persists
- [ ] Edit → Save → Changes persist
- [ ] Delete → Save → Item removed
- [ ] Toggle active → Behavior correct

### Integration Tests
- [ ] Surcharge applies in booking
- [ ] Promotion applies correctly
- [ ] Cancellation policy displays
- [ ] Price calculations accurate

---

## 🐛 Known Issues

### Issue 1: [If any]
**Description**:
**Severity**:
**Status**:
**Workaround**:

---

## ✅ Sign-off

**Tested by**: _________________
**Date**: _________________
**Environment**: Development (localhost:3000)
**Database**: _________________

**Overall Status**:
- [ ] All tests passed
- [ ] Some tests failed (see details above)
- [ ] Not tested yet

**Notes**:
[Add any additional notes here]

---

## 📝 Next Steps (If needed)

1. [ ] Fix any bugs found during testing
2. [ ] Add validation for edge cases
3. [ ] Add loading states
4. [ ] Add confirmation dialogs
5. [ ] Performance optimization
6. [ ] Write unit tests
7. [ ] Deploy to staging
8. [ ] User acceptance testing

---

## 🎯 Success Criteria

**Feature is considered complete when:**
1. ✅ No 500 errors on pricing page
2. ✅ Can add/edit/delete all pricing types
3. ✅ Data persists correctly
4. ✅ Surcharges apply in booking flow
5. ✅ All calculations accurate
6. ✅ UI is user-friendly
7. ✅ No console errors

**Current Status**: Ready for manual testing
**Blockers**: Need at least 1 tour in database to test
