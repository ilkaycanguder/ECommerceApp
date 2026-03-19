# ECommerceApp

## Proje Hakkında

.NET 8 ile geliştirilmiş, Onion Architecture ve Mikroservis mimarisi kullanan bir e-ticaret backend uygulaması. CQRS, JWT Authentication, Redis Cache ve RabbitMQ Event-Driven Architecture içermektedir.

---

## Mimari

- **Onion Architecture** — Her mikroservis Domain, Application, Infrastructure ve API katmanlarından oluşur
- **Mikroservis Mimarisi** — Auth, Product ve Log servisleri birbirinden bağımsız çalışır
- **CQRS Pattern** — MediatR ile Command ve Query işlemleri ayrıştırılmıştır
- **Event-Driven Architecture** — MassTransit + RabbitMQ ile servisler arası asenkron iletişim
- **Cache-Aside Pattern** — Redis ile ürün sorguları önbelleğe alınır

---

## Teknolojiler

| Teknoloji | Kullanım |
|---|---|
| .NET 8 | Framework |
| ASP.NET Core | Web API |
| Entity Framework Core 8 | ORM |
| SQL Server | Veritabanı |
| Redis | Cache |
| RabbitMQ | Message Broker |
| MassTransit | Message Bus |
| MediatR | CQRS |
| FluentValidation | Validasyon |
| AutoMapper | Object Mapping |
| JWT Bearer | Authentication |
| Serilog | Logging |
| Seq | Log Aggregation |
| YARP | API Gateway |

---

## Servisler

### Auth Service — `ECommerceApp.Auth.API` — `https://localhost:7044`

JWT token üretimi ve refresh token yönetimi.

| Endpoint | Method | Açıklama |
|---|---|---|
| `/api/auths/register` | POST | Kullanıcı kaydı |
| `/api/auths/login` | POST | Kullanıcı girişi |
| `/api/auths/refresh-token` | POST | Token yenileme |
| `/api/auths/user/{id}` | GET | Kullanıcı bilgisi |

### Product Service — `ECommerceApp.Product.API` — `https://localhost:7120`

Ürün yönetimi, Redis cache ve RabbitMQ event publishing.

| Endpoint | Method | Açıklama |
|---|---|---|
| `/api/products` | GET | Ürün listesi (Redis cache) |
| `/api/products/{id}` | GET | Ürün detayı (Redis cache) |
| `/api/products` | POST | Ürün ekleme |
| `/api/products/{id}` | PUT | Ürün güncelleme (JWT gerekli) |

### Log Service — `ECommerceApp.Log.API` — `https://localhost:7273`

Merkezi log yönetimi, RabbitMQ event consuming.

| Endpoint | Method | Açıklama |
|---|---|---|
| `/api/logs` | GET | Log listesi (JWT gerekli) |
| `/api/logs` | POST | Log ekleme (JWT gerekli) |

### Gateway — `ECommerceApp.Gateway` — `https://localhost:7004`

YARP Reverse Proxy ile tüm servislere tek noktadan erişim ve Rate Limiting.

| Route | Yönlendirme |
|---|---|
| `/api/auths/{**}` | Auth Service |
| `/api/products/{**}` | Product Service |
| `/api/logs/{**}` | Log Service (JWT gerekli) |

---

## Kurulum

### Gereksinimler

- .NET 8 SDK
- SQL Server
- Redis
- RabbitMQ
- Visual Studio 2022

### 1. Repository'yi klonla
```bash
git clone https://github.com/ilkaycanguder/ECommerceApp.git
cd ECommerceApp
git checkout test/v1.0.0
```

### 2. Veritabanlarını oluştur

Visual Studio'da `Tools` → `NuGet Package Manager` → `Package Manager Console`:
```powershell
# Auth veritabanı
Update-Database -Context AuthDbContext -Project ECommerceApp.Auth.Infrastructure -StartupProject ECommerceApp.Auth.API

# Product veritabanı
Update-Database -Context ProductDbContext -Project ECommerceApp.Product.Infrastructure -StartupProject ECommerceApp.Product.API

# Log veritabanı
Update-Database -Context LogDbContext -Project ECommerceApp.Log.Infrastructure -StartupProject ECommerceApp.Log.API
```

### 3. appsettings.json ayarları

Her servisin `appsettings.json` dosyasında şu ayarları kendi ortamına göre güncelle:

**Auth Service** (`src/Services/Auth/ECommerceApp.Auth.API/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "AuthDb": "Server=localhost;Database=ECommerceAuthDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-must-be-at-least-32-characters",
    "Issuer": "ECommerceApp.Auth",
    "Audience": "ECommerceApp",
    "ExpiryMinutes": "60"
  }
}
```

**Product Service** (`src/Services/Product/ECommerceApp.Product.API/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "ProductDb": "Server=localhost;Database=ECommerceProductDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

**Log Service** (`src/Services/Log/ECommerceApp.Log.API/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "LogDb": "Server=localhost;Database=ECommerceLogDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### 4. Servisleri çalıştır

Visual Studio'da birden fazla startup project ayarlamak için:

`Solution` → sağ tıkla → `Set Startup Projects` → `Multiple startup projects` → şu projeleri `Start` olarak işaretle:

- `ECommerceApp.Gateway`
- `ECommerceApp.Auth.API`
- `ECommerceApp.Product.API`
- `ECommerceApp.Log.API`

`OK` → `F5`

---

## Proje Yapısı
```
ECommerceApp
└── src
    ├── Gateway
    │   └── ECommerceApp.Gateway
    ├── Services
    │   ├── Auth
    │   │   ├── ECommerceApp.Auth.Domain
    │   │   ├── ECommerceApp.Auth.Application
    │   │   ├── ECommerceApp.Auth.Infrastructure
    │   │   └── ECommerceApp.Auth.API
    │   ├── Product
    │   │   ├── ECommerceApp.Product.Domain
    │   │   ├── ECommerceApp.Product.Application
    │   │   ├── ECommerceApp.Product.Infrastructure
    │   │   └── ECommerceApp.Product.API
    │   └── Log
    │       ├── ECommerceApp.Log.Application
    │       ├── ECommerceApp.Log.Infrastructure
    │       └── ECommerceApp.Log.API
    └── Shared
        ├── ECommerceApp.Shared
        └── ECommerceApp.Contracts
```

---

## Design Patterns

| Pattern | Kullanım Yeri |
|---|---|
| CQRS | Tüm servislerde Command/Query ayrımı |
| Repository | Veritabanı erişim soyutlaması |
| Unit of Work | Transaction yönetimi |
| Cache-Aside | Redis ile ürün önbellekleme |
| Pipeline Behavior | Validation ve cross-cutting concerns |
| Domain Events | Servisler arası event iletişimi |
| Observer | Log servisi event tüketimi |
| Gateway | YARP ile merkezi routing ve rate limiting |

---

## Branch Stratejisi
```
test/v1.0.0  → geliştirme branch'i
prod/v1.0.0  → production branch'i (test tamamlandıktan sonra merge)
```

---

## Lisans

MIT
```
