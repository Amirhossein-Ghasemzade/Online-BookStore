# Online-BookStore

An ASP.NET Core MVC book store with EF Core & Identity.

- Browse & filter books by category, author, keyword
- Book details with related books
- Author pages
- Full authentication (login required)
- Docker support

## Quick Start

```bash
cd BookStore
dotnet restore
dotnet ef database update
dotnet run
```

Or with Docker:

```bash
docker-compose up --build
```

**Default user:** `admin@bookstore.com` / `Admin@123`
