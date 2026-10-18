# SOA-JWT

## 1. Giới thiệu

**SOA-JWT** là project minh họa xây dựng hệ thống xác thực người dùng bằng **REST API** và **JWT (JSON Web Token)** trên nền tảng ASP.NET Core.

Project thực hiện các chức năng chính:

- Đăng nhập bằng `UserName` và `Password`.
- Kiểm tra tài khoản trong SQL Server.
- Sinh JWT Token sau khi đăng nhập thành công.
- Xác thực JWT Token bằng ASP.NET Core Authentication.
- Bảo vệ API bằng `[Authorize]`.
- Quản lý sản phẩm (CRUD) thông qua RESTful API có xác thực JWT.
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

## 3. Cấu trúc thư mục

```text
SOA-JWT/
├── Controllers/
│   ├── AuthController.cs          # API kiểm tra xác thực
│   ├── HelloController.cs         # API Hello World (protected)
│   ├── LoginController.cs         # API đăng nhập
│   └── ProductsController.cs      # API quản lý sản phẩm (CRUD)
├── Data/
│   └── AppDbContext.cs            # EF Core DbContext
├── Models/
│   ├── User.cs                    # Model người dùng
│   └── Product.cs                 # Model sản phẩm
├── Services/
│   └── JwtService.cs              # Dịch vụ sinh JWT
├── database_setup.sql             # Script khởi tạo toàn bộ database
├── Program.cs                     # Cấu hình ứng dụng
└── appsettings.json               # Cấu hình connection string & JWT
```

---

## 4. Database

Project sử dụng **SQL Server** với database:

```text
SOA
```

### Bảng Users

| Trường | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `IdUser` | INT (PK) | Khóa chính |
| `UserName` | VARCHAR(50) | Tên đăng nhập |
| `Password` | VARCHAR(100) | Mật khẩu |
| `Token` | VARCHAR(500) | JWT Token |

### Bảng Products *(Bài thực hành 3)*

| Trường | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `Id` | INT (PK, IDENTITY) | ID duy nhất của sản phẩm |
| `Name` | NVARCHAR(255) | Tên sản phẩm |
| `Description` | NVARCHAR(MAX) | Mô tả sản phẩm |
| `Price` | DECIMAL(10, 2) | Giá sản phẩm |
| `Quantity` | INT | Số lượng sản phẩm trong kho |
| `CreatedAt` | DATETIME2 | Ngày sản phẩm được tạo |
| `UpdatedAt` | DATETIME2 | Ngày sản phẩm được cập nhật lần cuối |

#### Khởi tạo Database

Chạy file **`database_setup.sql`** trong SQL Server Management Studio — script sẽ tự động:
- Tạo database `SOA` (nếu chưa có)
- Tạo bảng `Users` + dữ liệu mẫu
- Tạo bảng `Products` + dữ liệu mẫu
- Không bị lỗi nếu chạy lại (có `IF NOT EXISTS`)

```sql
-- Chỉ cần chạy lệnh này:
-- File: database_setup.sql
```

> Xem chi tiết trong file [`database_setup.sql`](database_setup.sql).

### Tài khoản test

```text
Username: admin
Password: 123456
```

---

## 5. Cấu hình project

Trong `appsettings.json`, cần cấu hình:

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

---

## 7. Danh sách API

### 7.1 Auth API

| Method | Endpoint | Mô tả | Yêu cầu JWT |
|---|---|---|---|
| `POST` | `/login` | Đăng nhập, nhận JWT token | ❌ |
| `GET` | `/auth` | Kiểm tra xác thực | ✅ |
| `GET` | `/hello` | Hello World | ✅ |

### 7.2 Product API *(Bài thực hành 3)*

| Method | Endpoint | Mô tả | Yêu cầu JWT |
|---|---|---|---|
| `GET` | `/products` | Lấy danh sách tất cả sản phẩm | ✅ |
| `GET` | `/products/{id}` | Lấy thông tin chi tiết 1 sản phẩm | ✅ |
| `POST` | `/products` | Thêm sản phẩm mới | ✅ |
| `PUT` | `/products/{id}` | Cập nhật thông tin sản phẩm | ✅ |
| `DELETE` | `/products/{id}` | Xóa sản phẩm | ✅ |

---

## 8. Hướng dẫn sử dụng API sản phẩm

> **Lưu ý:** Tất cả API sản phẩm đều yêu cầu JWT Bearer Token trong header.

### GET /products — Lấy danh sách sản phẩm

```http
GET /products
Authorization: Bearer <JWT_TOKEN>
```

Kết quả:

```json
[
  {
    "id": 1,
    "name": "Laptop Dell",
    "description": "Laptop Dell XPS 15",
    "price": 25000000.00,
    "quantity": 10,
    "createdAt": "2026-10-18T08:00:00Z",
    "updatedAt": "2026-10-18T08:00:00Z"
  }
]
```

### GET /products/{id} — Lấy chi tiết sản phẩm

```http
GET /products/1
Authorization: Bearer <JWT_TOKEN>
```

### POST /products — Thêm sản phẩm

```http
POST /products
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json

{
  "name": "Laptop Dell",
  "description": "Laptop Dell XPS 15",
  "price": 25000000,
  "quantity": 10
}
```

Kết quả: `201 Created`

### PUT /products/{id} — Cập nhật sản phẩm

```http
PUT /products/1
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json

{
  "name": "Laptop Dell XPS",
  "description": "Laptop Dell XPS 15 Updated",
  "price": 27000000,
  "quantity": 8
}
```

Kết quả: `200 OK`

### DELETE /products/{id} — Xóa sản phẩm

```http
DELETE /products/1
Authorization: Bearer <JWT_TOKEN>
```

Kết quả:

```json
{
  "message": "Đã xóa sản phẩm id = 1 thành công"
}
```

---

## 9. Chạy project

### Bước 1: Clone project

```bash
git clone https://github.com/vietyeudoi/SOA-JWT.git
cd SOA-JWT
```

### Bước 2: Mở project

Mở solution bằng Visual Studio.

### Bước 3: Chuẩn bị Database

Đảm bảo SQL Server đang chạy và database `SOA` đã tồn tại với các bảng:

- `Users`
- `Products`

> Chạy file **`database_setup.sql`** để tạo toàn bộ database và dữ liệu mẫu.

### Bước 4: Kiểm tra cấu hình

Mở `appsettings.json` và kiểm tra `ConnectionStrings` và `Jwt`.

### Bước 5: Chạy project

```text
F5  hoặc  Ctrl + F5
```

---

## 10. Swagger

Sau khi chạy project, mở Swagger:

```text
https://localhost:7081/swagger/index.html
```

> Port có thể khác tùy cấu hình trong project.

### Sử dụng JWT trên Swagger

1. Gọi `POST /login` để lấy token.
2. Copy JWT Token.
3. Nhấn **Authorize 🔒**.
4. Nhập: `Bearer <JWT_TOKEN>`
5. Nhấn **Authorize**.
6. Gọi các API sản phẩm.

---

## 11. HTTP Status Code

| Status | Ý nghĩa |
|---|---|
| `200 OK` | Request thành công |
| `201 Created` | Tạo tài nguyên thành công |
| `400 Bad Request` | Dữ liệu request không hợp lệ |
| `401 Unauthorized` | Chưa xác thực hoặc JWT không hợp lệ |
| `403 Forbidden` | Không có quyền truy cập |
| `404 Not Found` | Không tìm thấy tài nguyên |
| `500 Internal Server Error` | Lỗi phía server |

---

## 12. Kiến trúc tổng quan

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
             ┌──────────┐
             │  Users   │
             │ Products │
             └──────────┘
```

---

## 13. Bảo mật

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

## 14. Kết quả đạt được

### Bài thực hành 2 — JWT Authentication

- [x] Xây dựng REST API bằng ASP.NET Core.
- [x] Kết nối SQL Server bằng Entity Framework Core.
- [x] Đăng nhập bằng username/password.
- [x] Sinh JWT Token.
- [x] Xác thực JWT bằng Middleware.
- [x] Bảo vệ API bằng `[Authorize]`.
- [x] Kiểm tra API bằng Swagger và Postman.

### Bài thực hành 3 — Product Management Service

- [x] Xây dựng Model `Product` với đầy đủ các trường.
- [x] Tạo bảng `Products` trong SQL Server.
- [x] `GET /products` — Lấy danh sách sản phẩm.
- [x] `GET /products/{id}` — Lấy chi tiết sản phẩm.
- [x] `POST /products` — Thêm sản phẩm mới.
- [x] `PUT /products/{id}` — Cập nhật sản phẩm.
- [x] `DELETE /products/{id}` — Xóa sản phẩm.
- [x] Tất cả API sản phẩm được bảo vệ bằng JWT.

---

## 15. GitHub

Repository:

```text
https://github.com/vietyeudoi/SOA-JWT.git
```

---

## 16. Tác giả

**Project:** SOA-JWT

**Mục đích:** Bài tập môn SOA và REST API.

**Repository:**  
https://github.com/vietyeudoi/SOA-JWT
