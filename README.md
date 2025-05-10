
# 💻 ElectronicShop - Website Bán Hàng Linh Kiện Điện Tử

> Một hệ thống thương mại điện tử đơn giản được phát triển bằng ASP.NET MVC và Entity Framework.

## 🚀 Tính năng nổi bật

- ✅ Đăng ký / Đăng nhập / Phân quyền người dùng
- 🛒 Thêm sản phẩm vào giỏ hàng
- 🧾 Đặt hàng và theo dõi đơn hàng
- 🛠️ Quản lý sản phẩm, danh mục, người dùng (dành cho Admin)
- 🔐 Bảo mật phiên đăng nhập, mã hóa mật khẩu
- 🎨 Giao diện thân thiện với Bootstrap 5 và FontAwesome

## 📂 Cấu trúc thư mục

| Thư mục             | Mô tả                                 |
|---------------------|----------------------------------------|
| `Controllers/`      | Chứa các Controller như `ProductsController`, `CartController`, `AccountController` |
| `Models/`           | Các lớp dữ liệu: `Product`, `Order`, `User`, `CartItem`... |
| `Views/`            | Giao diện chia theo controller |
| `Content/Images/`   | Chứa ảnh sản phẩm |
| `Database/`         | File `ElectronicShopDb.sql` dùng để tạo cơ sở dữ liệu |

## ⚙️ Công nghệ sử dụng

- ASP.NET MVC 5
- Entity Framework 6
- SQL Server LocalDB
- HTML5, CSS3, Bootstrap 5
- jQuery, AJAX

---

## 🧠 Hướng dẫn triển khai

### 1. Clone dự án

```bash
git clone https://github.com/phuongthao1991/ElectronicShop.git
```

### 2. Phục hồi cơ sở dữ liệu

- Mở `SQL Server Management Studio (SSMS)`
- Tạo database: `ElectronicShopDb`
- Chạy file `Database/ElectronicShopDb.sql` để tạo bảng và dữ liệu mẫu trong file SQLQueryInsertInto.sql, SQLQueryInsertInto2.sql

### 3. Cấu hình chuỗi kết nối

Mở `Web.config` và chỉnh sửa:
```xml
<connectionStrings>
  <add name="ElectronicShopContext"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ElectronicShopDb;Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> ⚠️ Đảm bảo `Data Source` đúng với máy bạn (ví dụ: `.\SQLEXPRESS` nếu không dùng LocalDB)

### 4. Chạy ứng dụng

- Mở `ElectronicShop.sln` bằng Visual Studio
- Nhấn `Ctrl + F5` để chạy dự án

---

## 🔐 Tài khoản mẫu

| Email              | Mật khẩu  | Quyền     |
|-------------------|-----------|-----------|
| `lethao13111991@gmail.com` | `123456`     | Admin     |
| `nguyenvana@example.com`  | `123456`     | User      |

---

## 📌 Ghi chú

- File ảnh sản phẩm được lưu tại: `~/Content/Images/Products/`
- Bạn có thể thêm/sửa sản phẩm trong trang quản trị (với quyền Admin)

---

## ✍️ Tác giả

- **Họ tên:** Nguyễn Lê Phương Thảo - 0912302938
- **Email:** lethao13111991@gmail.com  
- **GitHub:** [phuongthao1991](https://github.com/phuongthao1991)

---

> © 2025 - Đồ án tốt nghiệp - Website bán hàng linh kiện điện tử
