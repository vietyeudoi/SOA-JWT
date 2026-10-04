# SOA-JWT

## 1. Giới thiệu

**SOA-JWT** là project minh họa xây dựng hệ thống xác thực người dùng bằng **REST API** và **JWT (JSON Web Token)** trên nền tảng ASP.NET Core.

Project thực hiện các chức năng chính:

- Đăng nhập bằng `UserName` và `Password`.
- Kiểm tra tài khoản trong SQL Server.
- Sinh JWT Token sau khi đăng nhập thành công.
- Xác thực JWT Token bằng ASP.NET Core Authentication.
- Bảo vệ API bằng `[Authorize]`.
- Kiểm tra API bằng Swagger và Postman.
- Có thể kết nối với Angular Frontend.

---

## 2. Công nghệ sử dụng

### Backend

- C#
- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- Swagger / OpenAPI

### Frontend

- Angular
- TypeScript
- HttpClient
- HTTP Interceptor

### Công cụ

- Visual Studio
- SQL Server / SQL Server Management Studio
- Swagger UI
- Postman
- Git / GitHub

---

## 3. Cấu trúc và thành phần chính

Project gồm các thành phần chính:

- **Controllers**: xử lý các request từ client.
- **Data**: kết nối và làm việc với database thông qua Entity Framework Core.
- **Models**: định nghĩa dữ liệu người dùng.
- **Services**: xử lý nghiệp vụ liên quan đến JWT.
- **Program.cs**: cấu hình ứng dụng, database, JWT Authentication, Authorization và Swagger.
- **appsettings.json**: cấu hình connection string và JWT.

---

## 4. Database

Project sử dụng **SQL Server** với database:

```text
SOA
```

Bảng chính:

```text
Users
```

Các trường:

| Trường | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `IdUser` | INT | Khóa chính |
| `UserName` | VARCHAR(50) | Tên đăng nhập |
| `Password` | VARCHAR(100) | Mật khẩu |
| `Token` | VARCHAR(500) | JWT Token |

### Tài khoản test

```text
Username: admin
Password: 123456
```

### Kiểm tra dữ liệu

```sql
USE SOA;
GO

SELECT * FROM Users;
```

> Nếu chưa có database, hãy tạo database và bảng `Users` theo cấu trúc mà project đang sử dụng.

---

## 5. Cấu hình project

Trong `appsettings.json`, cần cấu hình:

- Connection String của SQL Server.
- JWT Secret Key.
- JWT Issuer.
- JWT Audience.

Ví dụ:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SOA;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "SOA",
    "Audience": "SOA"
  }
}
```

> Không nên đưa JWT secret hoặc thông tin database thật lên GitHub. Với project thực tế nên sử dụng User Secrets hoặc Environment Variables.

---

## 6. JWT Authentication

JWT được sử dụng để xác thực người dùng sau khi đăng nhập.

Luồng xác thực:

```text
Client
   │
   │ Username + Password
   ▼
Login API
   │
   │ Kiểm tra tài khoản
   ▼
SQL Server
   │
   │ Đúng
   ▼
JwtService
   │
   │ Sinh JWT
   ▼
Client nhận Token
   │
   │ Authorization: Bearer <Token>
   ▼
Authentication Middleware
   │
   │ Kiểm tra Token
   ▼
Protected API
```

JWT được kiểm tra các thông tin như:

- Signature.
- Issuer.
- Audience.
- Thời gian hết hạn.
- Tính hợp lệ của token.

---

## 7. Chạy project

### Bước 1: Clone project

```bash
git clone https://github.com/vietyeudoi/SOA-JWT.git
```

Sau đó:

```bash
cd SOA-JWT
```

### Bước 2: Mở project

Mở solution bằng Visual Studio.

### Bước 3: Kiểm tra SQL Server

Đảm bảo SQL Server đang chạy và database `SOA` đã tồn tại.

Kiểm tra bảng:

```text
Users
```

### Bước 4: Kiểm tra cấu hình

Mở:

```text
appsettings.json
```

và kiểm tra:

```text
ConnectionStrings
Jwt
```

### Bước 5: Chạy project

Trong Visual Studio:

```text
F5
```

hoặc:

```text
Ctrl + F5
```

---

## 8. Swagger

Sau khi chạy project, mở Swagger:

```text
https://localhost:7081/swagger
```

Hoặc:

```text
https://localhost:7081/swagger/index.html
```

> Port có thể khác tùy cấu hình trong project.

Swagger được sử dụng để xem và kiểm tra các API.

---

## 9. Đăng nhập

### Request

```http
POST /login
```

Body:

```json
{
  "userName": "admin",
  "password": "123456"
}
```

Nếu thông tin đăng nhập chính xác, API trả về JWT Token.

Ví dụ:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

Token này được sử dụng cho các API yêu cầu xác thực.

---

## 10. Sử dụng JWT trên Swagger

Sau khi đăng nhập thành công:

1. Copy JWT Token.
2. Mở Swagger.
3. Nhấn **Authorize 🔒**.
4. Nhập:

```text
Bearer <JWT_TOKEN>
```

Ví dụ:

```text
Bearer eyJhbGciOiJIUzI1NiIs...
```

5. Nhấn **Authorize**.
6. Gọi API được bảo vệ.

---

## 11. API Hello World

API Hello World được bảo vệ bằng JWT.

```http
GET /hello
```

### Không có JWT

Kết quả:

```text
401 Unauthorized
```

### Có JWT hợp lệ

Kết quả:

```text
200 OK
```

Ví dụ:

```text
Hello World!
```

Điều này chứng minh Authentication Middleware đang hoạt động.

---

## 12. API Auth

API `/auth` dùng để kiểm tra việc xác thực bằng JWT.

```http
GET /auth
```

Request cần có header:

```http
Authorization: Bearer <JWT_TOKEN>
```

Nếu token hợp lệ:

```text
200 OK
```

Nếu token không hợp lệ hoặc hết hạn:

```text
401 Unauthorized
```

---

## 13. Test bằng Postman

### Bước 1: Login

Method:

```text
POST
```

URL:

```text
https://localhost:7081/login
```

Body → `raw` → `JSON`:

```json
{
  "userName": "admin",
  "password": "123456"
}
```

Nhấn **Send** và copy JWT Token.

### Bước 2: Gọi API Hello

Method:

```text
GET
```

URL:

```text
https://localhost:7081/hello
```

Vào:

```text
Authorization
```

Chọn:

```text
Bearer Token
```

Dán JWT vào ô Token.

Nhấn **Send**.

Kết quả mong đợi:

```text
200 OK
```

---

## 14. HTTP Status Code

| Status | Ý nghĩa |
|---|---|
| `200 OK` | Request thành công |
| `400 Bad Request` | Dữ liệu request không hợp lệ |
| `401 Unauthorized` | Chưa xác thực hoặc JWT không hợp lệ |
| `403 Forbidden` | Không có quyền truy cập |
| `404 Not Found` | Không tìm thấy API |
| `500 Internal Server Error` | Lỗi phía server |

---

## 15. Kiến trúc tổng quan

```text
                 CLIENT
        ┌─────────────────────┐
        │ Swagger / Postman   │
        │ Angular             │
        └──────────┬──────────┘
                   │
                   ▼
          ┌─────────────────┐
          │ ASP.NET Core API│
          └────────┬────────┘
                   │
        ┌──────────┼──────────┐
        ▼          ▼          ▼
   Controller  Middleware   Service
        │          │          │
        │      JWT Validate    │
        └──────────┼──────────┘
                   │
                   ▼
             Entity Framework
                   │
                   ▼
              SQL Server
```

---

## 16. Bảo mật

Project hiện tại phục vụ mục đích học tập và sử dụng tài khoản test đơn giản.

Trong hệ thống thực tế nên:

- Hash password bằng BCrypt, Argon2 hoặc PBKDF2.
- Không lưu password dạng plaintext.
- Không commit JWT secret lên GitHub.
- Sử dụng HTTPS.
- Sử dụng Secret Manager hoặc Environment Variables.
- Thiết lập thời gian hết hạn cho JWT.
- Sử dụng Role/Claim để phân quyền.

---

## 17. GitHub

Repository:

```text
https://github.com/vietyeudoi/SOA-JWT.git
```

Các lệnh Git cơ bản:

```bash
git status
```

```bash
git add .
```

```bash
git commit -m "Update project"
```

```bash
git push
```

---

## 18. Kết quả đạt được

- [x] Xây dựng REST API bằng ASP.NET Core.
- [x] Kết nối SQL Server.
- [x] Sử dụng Entity Framework Core.
- [x] Đăng nhập bằng username/password.
- [x] Sinh JWT Token.
- [x] Xác thực JWT.
- [x] Bảo vệ API bằng `[Authorize]`.
- [x] Kiểm tra API bằng Swagger.
- [x] Kiểm tra API bằng Postman.
- [x] Có thể tích hợp Angular Frontend.
- [x] Đưa project lên GitHub.

---

## 19. Tác giả

**Project:** SOA-JWT

**Mục đích:** Bài tập môn SOA và REST API.

**Repository:**  
https://github.com/vietyeudoi/SOA-JWT
