# Hướng Dẫn Renew SSL Certificates

## 📋 Domains cần renew SSL:
- admin.goreise.com
- api.goreise.com
- tour.goreise.com
- vote.kinhthanhmoingay.com

## 🚀 Cách 1: Chạy Script Tự Động

### Bước 1: SSH vào server
```bash
ssh root@157.173.124.250
# Hoặc nếu dùng deployer:
ssh deployer@157.173.124.250
```

### Bước 2: Chạy script với sudo
```bash
sudo bash /tmp/renew-all-ssl.sh
```

Script sẽ tự động:
1. Dừng Nginx
2. Renew SSL cho tất cả 4 domains
3. Khởi động lại Nginx
4. Hiển thị trạng thái SSL

### Bước 3: Test các trang web
- https://admin.goreise.com
- https://api.goreise.com
- https://tour.goreise.com
- https://vote.kinhthanhmoingay.com

## 🔧 Cách 2: Chạy Từng Domain (Nếu Cách 1 Lỗi)

### Renew từng domain thủ công:
```bash
# Dừng Nginx
sudo systemctl stop nginx

# Renew SSL cho admin.goreise.com
sudo certbot certonly --standalone -d admin.goreise.com --non-interactive --agree-tos -m webmaster@goreise.com

# Renew SSL cho api.goreise.com
sudo certbot certonly --standalone -d api.goreise.com --non-interactive --agree-tos -m webmaster@goreise.com

# Renew SSL cho tour.goreise.com
sudo certbot certonly --standalone -d tour.goreise.com --non-interactive --agree-tos -m webmaster@goreise.com

# Renew SSL cho vote.kinhthanhmoingay.com
sudo certbot certonly --standalone -d vote.kinhthanhmoingay.com --non-interactive --agree-tos -m webmaster@goreise.com

# Khởi động lại Nginx
sudo systemctl start nginx
sudo systemctl reload nginx
```

## 🔍 Kiểm Tra SSL Status

```bash
# Xem tất cả SSL certificates
sudo certbot certificates

# Xem SSL cho 1 domain cụ thể
sudo certbot certificates | grep admin.goreise.com
```

## ⚙️ Cấu Hình Auto-Renewal (Khuyên Dùng)

### Setup auto-renewal chạy hàng ngày:
```bash
# Kiểm tra auto-renewal đã enable chưa
sudo systemctl status certbot.timer

# Enable auto-renewal nếu chưa có
sudo systemctl enable certbot.timer
sudo systemctl start certbot.timer

# Test auto-renewal
sudo certbot renew --dry-run
```

## 🔄 Schedule Auto-Renewal với Cron

Nếu muốn control chính xác thời gian renewal:

```bash
# Mở crontab
sudo crontab -e

# Thêm dòng này để chạy mỗi ngày lúc 2:00 AM
0 2 * * * certbot renew --quiet --post-hook "systemctl reload nginx"
```

## ⚠️ Lưu Ý Quan Trọng

1. **Phải dừng Nginx** trước khi renew nếu dùng `--standalone`
2. **Let's Encrypt giới hạn**: 5 lần renew/tuần cho mỗi domain
3. **SSL hết hạn sau 90 ngày** - nên renew trước 30 ngày
4. **Email thông báo**: webmaster@goreise.com sẽ nhận thông báo khi SSL sắp hết hạn

## 🆘 Troubleshooting

### Lỗi: Port 80 đã được sử dụng
```bash
# Kiểm tra process nào đang dùng port 80
sudo netstat -tulpn | grep :80

# Dừng Nginx
sudo systemctl stop nginx

# Thử lại
sudo bash /tmp/renew-all-ssl.sh
```

### Lỗi: Permission denied
```bash
# Chắc chắn dùng sudo
sudo bash /tmp/renew-all-ssl.sh
```

### Lỗi: Domain validation failed
```bash
# Kiểm tra DNS records
nslookup admin.goreise.com
nslookup api.goreise.com
nslookup tour.goreise.com
nslookup vote.kinhthanhmoingay.com

# Chắc chắn domains đều trỏ về server IP: 157.173.124.250
```

## 📞 Cần Trợ Giúp?

Nếu gặp lỗi, gửi cho tôi:
1. Output của command: `sudo certbot certificates`
2. Lỗi cụ thể khi chạy script
3. Output của: `sudo systemctl status nginx`
