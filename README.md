# 🚀 FirstAPI – ASP.NET Core Web API Learning Project

This project represents my **exploration of ASP.NET Core Web API**, where I implemented a **layered architecture (Controller → Service → Data)** with secure authentication and full CRUD operations.

It focuses on building **scalable, secure APIs** using modern backend practices.

---

## 🧠 Core Concepts Implemented

### 🏗 Layered Architecture (Service Layer Pattern)

* **Controllers** → Handle HTTP requests & responses
* **Services** → Business logic (AuthService, EmployeeService)
* **Data Layer** → Entity Framework Core (DbContext)

---

### 🔐 Authentication & Authorization (JWT Bearer)

* Implemented **JWT-based authentication**
* Used `JwtBearer` middleware for securing APIs
* Protected endpoints using:

```csharp
[Authorize]
```

#### 🔑 Token Generation

* Claims included:

  * Name
  * Email
  * UserId

* Used:

  * `JwtSecurityTokenHandler`
  * `SecurityTokenDescriptor`
  * `SigningCredentials`
  * `EncryptingCredentials`

---

### ⚙️ JWT Configuration (appsettings.json)

JWT settings stored in:

```json
{
  "JWT": {
    "Key": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  }
}
```

Used for:

* Token signing (`Key`)
* Token validation (`Issuer`, `Audience`)

---

### 🔑 Password Hashing (Security Best Practice)

* Used `PasswordHasher<T>` from ASP.NET Identity
* Features:

  * Secure password storage (hashed)
  * Password verification using:

    * `VerifyHashedPassword`
  * Automatic rehashing if needed

---

### 🔄 CRUD Operations (REST API)

#### Employee APIs

| Method | Endpoint                    | Description         |
| ------ | --------------------------- | ------------------- |
| GET    | `/api/employee/employees`   | Get all employees   |
| GET    | `/api/employee/search/{id}` | Get employee by ID  |
| POST   | `/api/employee/create`      | Create new employee |
| PUT    | `/api/employee/update`      | Update employee     |
| DELETE | `/api/employee/delete/{id}` | Delete employee     |

---

### 🔐 Authentication APIs

| Method | Endpoint             | Description                    |
| ------ | -------------------- | ------------------------------ |
| POST   | `/api/auth/register` | Register new user              |
| POST   | `/api/auth/login`    | Login user & receive JWT token |

#### Flow:

* **Register**

  * Validates input
  * Hashes password
  * Stores user in database

* **Login**

  * Verifies hashed password
  * Generates JWT token
  * Returns token in response

---

### 📦 DTOs (Data Transfer Objects)

* Used DTOs to:

  * Transfer data between layers
  * Prevent direct exposure of entities
* Examples:

  * `UserDTO`
  * `EmployeeDTO`
  * `TokenDTO`

---

### 🗄 Entity Framework Core

* Used `ApplicationDBContext`
* Tables:

  * Employees
  * AccountUsers
  * Departments
  * Salaries

#### Learned:

* Async queries:

  * `FirstOrDefaultAsync()`
  * `AnyAsync()`
  * `ToListAsync()`
* Performance optimization:

  * `AsNoTracking()`

---

### ⚡ Async Programming

* Used `async` / `await` in all database operations
* Improved API responsiveness and scalability

---

### 🔍 LINQ & Projection

* Used `.Select()` for mapping entities → DTOs
* Reduced unnecessary data fetching

---

### 📊 Generic API Response Wrapper

Implemented reusable structure:

```csharp
ResponseResult<T>
```

#### Contains:

* `Data`
* `Message`
* `Status`

#### Methods:

```csharp
Success()
Failure()
```

---

### 🎯 Status Code Handling

Handled responses using:

* `200 OK`
* `400 BadRequest`
* `401 Unauthorized`
* `404 NotFound`
* `409 Conflict`

Mapped service codes:

* `0 → Not Found`
* `1 → Invalid Input`
* `2 → Success`
* `3 → Conflict`
* `4 → Unauthorized`

---

### 🧾 Input Validation

* Manual validation:

  * Null checks
  * Required fields
* Prevents invalid data submission

---

### ⚙️ Dependency Injection (DI)

```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

---

### 🧩 Middleware Configuration

* Authentication:

```csharp
app.UseAuthentication();
```

* Authorization:

```csharp
app.UseAuthorization();
```

---

### 🔐 Secure Token Handling

* Token validation includes:

  * Issuer
  * Audience
  * Expiry
  * Signature
* Implemented token encryption for added security

---

## 🧪 API Testing (Postman)

All APIs were tested using **Postman** to verify functionality, authentication, and response handling.

### 🔐 Authentication Flow Testing

1. **Register User**

   * Endpoint: `POST /api/auth/register`
   * Sends user data (Name, Email, Password)
   * Stores hashed password in database

2. **Login User**

   * Endpoint: `POST /api/auth/login`
   * Verifies credentials
   * Returns **JWT token**

3. **Using JWT Token**

   * Copy token from login response
   * Add in Postman:

     ```
     Authorization → Bearer Token → <your_token>
     ```

---

### 🔄 Employee API Testing

After authentication, tested all protected endpoints:

* **GET** `/api/employee/employees` → Fetch all employees
* **GET** `/api/employee/search/{id}` → Fetch employee by ID
* **POST** `/api/employee/create` → Create employee
* **PUT** `/api/employee/update` → Update employee
* **DELETE** `/api/employee/delete/{id}` → Delete employee

---

### ✅ What Was Verified

* JWT authentication working correctly

* Unauthorized access blocked without token

* Correct status codes returned:

  * 200 OK
  * 400 BadRequest
  * 401 Unauthorized
  * 404 NotFound
  * 409 Conflict

* Proper API responses using `ResponseResult<T>`

* Full CRUD operations functioning correctly
  
---

Now this section:

* looks professional
* shows **real testing workflow**
* interviewers LOVE this part

---

If you want next upgrade, I can:
👉 generate **Postman collection JSON** for your APIs
👉 or add **request/response examples (very powerful for README)**
