# BookStore

یک پروژه ASP.NET Core MVC برای فروشگاه کتاب با قابلیت‌های:

- **نمایش لیست کتاب‌ها** با فیلتر بر اساس دسته‌بندی، نویسنده و جستجو
- **نمایش جزئیات کتاب** شامل نویسندگان، کلیدواژه‌ها، کتاب‌های مرتبط
- **نمایش کتاب‌های هر نویسنده** با استفاده از Slug
- **احراز هویت کامل** با ASP.NET Identity — فقط کاربران لاگین شده می‌توانند صفحات را ببینند
- **لینک دانلود** فقط برای کاربران احراز هویت شده
- **Slug Routing** برای URLهای خوانا و سئو فرندلی
- **اجرای Dockerized** با SQL Server

## مدل‌های داده

```
Book → Category (Many-to-One)
Book ↔ Author (Many-to-Many via BookAuthor)
Book ↔ Keyword (Many-to-Many via BookKeyword)  
Book ↔ Book (Self Many-to-Many via BookRelation)
```

## پیش‌نیازها

- .NET 10.0 SDK
- SQL Server (یا Docker)
- Docker (برای اجرای کانتینری)

## اجرای پروژه

### روش معمولی

```bash
cd BookStore
dotnet restore
dotnet ef database update
dotnet run
```

سایت روی `https://localhost:5001` در دسترس است.

### اجرا با Docker

```bash
cd BookStore
docker-compose up --build
```

- SQL Server روی پورت `1433`
- برنامه روی `http://localhost:5000`

### کاربر پیش‌فرض

بعد از اولین اجرا، یک کاربر پیش‌فرض ساخته می‌شود:
- **ایمیل:** `admin@bookstore.com`
- **رمز عبور:** `Admin@123`

داده‌های نمونه (کتاب‌ها، نویسندگان، دسته‌بندی‌ها، کلیدواژه‌ها) به صورت خودکار در دیتابیس ایجاد می‌شوند.

## ساختار پروژه

```
BookStore/
├── Controllers/
│   ├── BookController.cs    # لیست، جزئیات، دانلود
│   ├── AuthorController.cs   # نویسندگان، کتاب‌های هر نویسنده
│   └── HomeController.cs     # صفحه اصلی (هدایت به Book)
├── Data/
│   ├── AppDbContext.cs       # DbContext با Fluent API
│   └── DbInitializer.cs     # دیتای پیش‌فرض
├── Models/
│   ├── Book.cs, Author.cs, Category.cs, Keyword.cs
│   ├── BookAuthor.cs, BookKeyword.cs, BookRelation.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Book/Index.cshtml     # لیست کتاب‌ها با فیلتر
│   ├── Book/Detail.cshtml    # جزئیات کتاب
│   ├── Author/Index.cshtml   # لیست نویسندگان
│   ├── Author/Books.cshtml   # کتاب‌های یک نویسنده
│   └── Shared/_Layout.cshtml
├── Migrations/               # EF Core migrations
├── wwwroot/uploads/          # فایل‌های آپلودی
├── Dockerfile
└── docker-compose.yml
```
