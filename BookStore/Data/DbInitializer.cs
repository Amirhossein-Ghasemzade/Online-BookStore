using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            await context.Database.EnsureCreatedAsync();
            if (await context.Authors.AnyAsync()) goto CreateUser;

            #region Categories

            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Title = "Programming", Slug = "programming" },
                    new Category { Title = "Web Development", Slug = "web-development" },
                    new Category { Title = "Data Science", Slug = "data-science" },
                    new Category { Title = "ادبیات فارسی", Slug = "persian-literature" },
                    new Category { Title = "تاریخ", Slug = "history" },
                    new Category { Title = "فلسفه", Slug = "philosophy" },
                    new Category { Title = "management", Slug = "management" }
                );
                await context.SaveChangesAsync();
            }

            #endregion

            #region Authors

            context.Authors.AddRange(
                // ایرانی
                new Author { FullName = "جلال آل احمد", Slug = "jalal-al-e-ahmad" },
                new Author { FullName = "صادق هدایت", Slug = "sadegh-hedayat" },
                new Author { FullName = "محمدرضا شجریان", Slug = "mohammad-reza-shajarian" },
                new Author { FullName = "علی شریعتی", Slug = "ali-shariati" },
                new Author { FullName = "احمد شاملو", Slug = "ahmad-shamlou" },
                new Author { FullName = "مهدی اخوان ثالث", Slug = "mehdi-akhavan-sales" },
                new Author { FullName = "نادر ابراهیمی", Slug = "nader-ebrahimi" },
                new Author { FullName = "دکتر محمد مصدق", Slug = "mohammad-mosaddegh" },
                // بین‌المللی
                new Author { FullName = "Robert C. Martin", Slug = "robert-c-martin" },
                new Author { FullName = "Martin Fowler", Slug = "martin-fowler" },
                new Author { FullName = "Eric Evans", Slug = "eric-evans" },
                new Author { FullName = "James Gosling", Slug = "james-gosling" },
                new Author { FullName = "Andrew Hunt", Slug = "andrew-hunt" },
                new Author { FullName = "David Thomas", Slug = "david-thomas" },
                new Author { FullName = "Napoleon Hill", Slug = "napoleon-hill" },
                new Author { FullName = "Stephen R. Covey", Slug = "stephen-covey" }
            );

            #endregion

            #region Keywords

            context.Keywords.AddRange(
                new Keyword { Word = "clean-code" },
                new Keyword { Word = "design-patterns" },
                new Keyword { Word = "software-architecture" },
                new Keyword { Word = "domain-driven-design" },
                new Keyword { Word = "java" },
                new Keyword { Word = "python" },
                new Keyword { Word = "dastan-kootah" },
                new Keyword { Word = "roman" },
                new Keyword { Word = "siasat" },
                new Keyword { Word = "success" },
                new Keyword { Word = "leadership" },
                new Keyword { Word = "poetry" },
                new Keyword { Word = "موسیقی" },
                new Keyword { Word = "تاریخ-ایران" }
            );

            #endregion

            await context.SaveChangesAsync();

            #region Fetch References

            var catProgramming = await context.Categories.FirstAsync(c => c.Slug == "programming");
            var catWeb = await context.Categories.FirstAsync(c => c.Slug == "web-development");
            var catData = await context.Categories.FirstAsync(c => c.Slug == "data-science");
            var catLit = await context.Categories.FirstAsync(c => c.Slug == "persian-literature");
            var catHistory = await context.Categories.FirstAsync(c => c.Slug == "history");
            var catPhilo = await context.Categories.FirstAsync(c => c.Slug == "philosophy");
            var catMgmt = await context.Categories.FirstAsync(c => c.Slug == "management");

            var authorJalal = await context.Authors.FirstAsync(a => a.Slug == "jalal-al-e-ahmad");
            var authorHedayat = await context.Authors.FirstAsync(a => a.Slug == "sadegh-hedayat");
            var authorShajarian = await context.Authors.FirstAsync(a => a.Slug == "mohammad-reza-shajarian");
            var authorShariati = await context.Authors.FirstAsync(a => a.Slug == "ali-shariati");
            var authorShamlou = await context.Authors.FirstAsync(a => a.Slug == "ahmad-shamlou");
            var authorAkhavan = await context.Authors.FirstAsync(a => a.Slug == "mehdi-akhavan-sales");
            var authorNader = await context.Authors.FirstAsync(a => a.Slug == "nader-ebrahimi");
            var authorMosaddegh = await context.Authors.FirstAsync(a => a.Slug == "mohammad-mosaddegh");
            var authorMartin = await context.Authors.FirstAsync(a => a.Slug == "robert-c-martin");
            var authorFowler = await context.Authors.FirstAsync(a => a.Slug == "martin-fowler");
            var authorEvans = await context.Authors.FirstAsync(a => a.Slug == "eric-evans");
            var authorHunt = await context.Authors.FirstAsync(a => a.Slug == "andrew-hunt");
            var authorCovey = await context.Authors.FirstAsync(a => a.Slug == "stephen-covey");

            var kwClean = await context.Keywords.FirstAsync(k => k.Word == "clean-code");
            var kwDesign = await context.Keywords.FirstAsync(k => k.Word == "design-patterns");
            var kwArch = await context.Keywords.FirstAsync(k => k.Word == "software-architecture");
            var kwDDD = await context.Keywords.FirstAsync(k => k.Word == "domain-driven-design");
            var kwJava = await context.Keywords.FirstAsync(k => k.Word == "java");
            var kwPython = await context.Keywords.FirstAsync(k => k.Word == "python");
            var kwDastan = await context.Keywords.FirstAsync(k => k.Word == "dastan-kootah");
            var kwRoman = await context.Keywords.FirstAsync(k => k.Word == "roman");
            var kwSiasat = await context.Keywords.FirstAsync(k => k.Word == "siasat");
            var kwSuccess = await context.Keywords.FirstAsync(k => k.Word == "success");
            var kwLeadership = await context.Keywords.FirstAsync(k => k.Word == "leadership");
            var kwPoetry = await context.Keywords.FirstAsync(k => k.Word == "poetry");
            var kwMusic = await context.Keywords.FirstAsync(k => k.Word == "موسیقی");
            var kwHistory = await context.Keywords.FirstAsync(k => k.Word == "تاریخ-ایران");

            #endregion

            #region Books

            // --- کتاب‌های ایرانی ---
            var bookGharbzadegi = new Book { Title = "غرب‌زدگی", Slug = "gharbzadegi", PageCount = 180, Description = "کتاب معروف جلال آل احمد که به نقد مدرنیته و تأثیر فرهنگ غرب بر ایران می‌پردازد.", FilePath = "/uploads/sample.pdf", CategoryId = catHistory.Id };

            var bookBlindOwl = new Book { Title = "بوف کور", Slug = "bof-e-koor", PageCount = 120, Description = "معروف‌ترین اثر صادق هدایت، رمانی سوررئال و عمیق درباره تنهایی و مرگ.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookChamedan = new Book { Title = "چمدان", Slug = "chamedan", PageCount = 200, Description = "مجموعه داستان‌های کوتاه صادق هدایت.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookRazDastan = new Book { Title = "راز داستان", Slug = "raz-e-dastan", PageCount = 250, Description = "مجموعه مقالات جلال آل احمد درباره ادبیات داستانی.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookNaghsh = new Book { Title = "نقش بر آب", Slug = "naghsh-bar-ab", PageCount = 320, Description = "مجموعه اشعار احمد شاملو با مضامین اجتماعی و عاشقانه.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookAzAta = new Book { Title = "از این اوستا", Slug = "az-in-osta", PageCount = 280, Description = "مجموعه اشعار مهدی اخوان ثالث (م. امید)، شاعر برجسته معاصر ایران.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookBarMadar = new Book { Title = "بر مدار بی‌نهایت", Slug = "bar-madare-binehayat", PageCount = 400, Description = "داستانی بلند از نادر ابراهیمی درباره عشق و زندگی.", FilePath = "/uploads/sample.pdf", CategoryId = catLit.Id };

            var bookRazMosaddegh = new Book { Title = "کودتای ۲۸ مرداد", Slug = "coup-28-mordad", PageCount = 350, Description = "تحلیلی تاریخی از کودتای ۲۸ مرداد ۱۳۳۲ و نقش دکتر مصدق.", FilePath = "/uploads/sample.pdf", CategoryId = catHistory.Id };

            var bookShajarian = new Book { Title = "راز و نیاز", Slug = "raz-va-niaz", PageCount = 150, Description = "مجموعه‌ای از اشعار و تصانیف محمدرضا شجریان به همراه تحلیل دستگاه‌های موسیقی ایرانی.", FilePath = "/uploads/sample.pdf", CategoryId = catPhilo.Id };

            var bookShariati = new Book { Title = "بازگشت به خویشتن", Slug = "bazgasht-be-khishtan", PageCount = 220, Description = "مجموعه سخنرانی‌های علی شریعتی درباره هویت و خودآگاهی.", FilePath = "/uploads/sample.pdf", CategoryId = catPhilo.Id };

            // --- کتاب‌های بین‌المللی ---
            var bookCleanCode = new Book { Title = "Clean Code", Slug = "clean-code", PageCount = 464, Description = "A handbook of agile software craftsmanship by Robert C. Martin.", FilePath = "/uploads/sample.pdf", CategoryId = catProgramming.Id };

            var bookCleanArch = new Book { Title = "Clean Architecture", Slug = "clean-architecture", PageCount = 432, Description = "A craftsman's guide to software structure and design by Robert C. Martin.", FilePath = "/uploads/sample.pdf", CategoryId = catProgramming.Id };

            var bookRefactoring = new Book { Title = "Refactoring", Slug = "refactoring", PageCount = 448, Description = "Improving the design of existing code by Martin Fowler.", FilePath = "/uploads/sample.pdf", CategoryId = catWeb.Id };

            var bookDDD = new Book { Title = "Domain-Driven Design", Slug = "domain-driven-design", PageCount = 560, Description = "Tackling complexity in the heart of software by Eric Evans.", FilePath = "/uploads/sample.pdf", CategoryId = catData.Id };

            var bookPragmatic = new Book { Title = "The Pragmatic Programmer", Slug = "the-pragmatic-programmer", PageCount = 352, Description = "Your journey to mastery by Andrew Hunt and David Thomas.", FilePath = "/uploads/sample.pdf", CategoryId = catProgramming.Id };

            var bookThinkAndGrow = new Book { Title = "Think and Grow Rich", Slug = "think-and-grow-rich", PageCount = 256, Description = "The original guide to success by Napoleon Hill.", FilePath = "/uploads/sample.pdf", CategoryId = catMgmt.Id };

            var bookCovey = new Book { Title = "The 7 Habits of Highly Effective People", Slug = "7-habits", PageCount = 384, Description = "Powerful lessons in personal change by Stephen R. Covey.", FilePath = "/uploads/sample.pdf", CategoryId = catMgmt.Id };

            context.Books.AddRange(
                bookGharbzadegi, bookBlindOwl, bookChamedan, bookRazDastan,
                bookNaghsh, bookAzAta, bookBarMadar, bookRazMosaddegh,
                bookShajarian, bookShariati,
                bookCleanCode, bookCleanArch, bookRefactoring, bookDDD,
                bookPragmatic, bookThinkAndGrow, bookCovey
            );

            #endregion

            await context.SaveChangesAsync();

            #region Fetch Books

            bookGharbzadegi = await context.Books.FirstAsync(b => b.Slug == "gharbzadegi");
            bookBlindOwl = await context.Books.FirstAsync(b => b.Slug == "bof-e-koor");
            bookChamedan = await context.Books.FirstAsync(b => b.Slug == "chamedan");
            bookRazDastan = await context.Books.FirstAsync(b => b.Slug == "raz-e-dastan");
            bookNaghsh = await context.Books.FirstAsync(b => b.Slug == "naghsh-bar-ab");
            bookAzAta = await context.Books.FirstAsync(b => b.Slug == "az-in-osta");
            bookBarMadar = await context.Books.FirstAsync(b => b.Slug == "bar-madare-binehayat");
            bookRazMosaddegh = await context.Books.FirstAsync(b => b.Slug == "coup-28-mordad");
            bookShajarian = await context.Books.FirstAsync(b => b.Slug == "raz-va-niaz");
            bookShariati = await context.Books.FirstAsync(b => b.Slug == "bazgasht-be-khishtan");
            var bookCleanCode2 = await context.Books.FirstAsync(b => b.Slug == "clean-code");
            var bookCleanArch2 = await context.Books.FirstAsync(b => b.Slug == "clean-architecture");
            var bookRefactoring2 = await context.Books.FirstAsync(b => b.Slug == "refactoring");
            var bookDDD2 = await context.Books.FirstAsync(b => b.Slug == "domain-driven-design");
            var bookPragmatic2 = await context.Books.FirstAsync(b => b.Slug == "the-pragmatic-programmer");
            var bookThinkAndGrow2 = await context.Books.FirstAsync(b => b.Slug == "think-and-grow-rich");
            var bookCovey2 = await context.Books.FirstAsync(b => b.Slug == "7-habits");

            #endregion

            #region BookAuthors

            context.BookAuthors.AddRange(
                // کتاب‌های ایرانی
                new BookAuthor { BookId = bookGharbzadegi.Id, AuthorId = authorJalal.Id },
                new BookAuthor { BookId = bookBlindOwl.Id, AuthorId = authorHedayat.Id },
                new BookAuthor { BookId = bookChamedan.Id, AuthorId = authorHedayat.Id },
                new BookAuthor { BookId = bookRazDastan.Id, AuthorId = authorJalal.Id },
                new BookAuthor { BookId = bookNaghsh.Id, AuthorId = authorShamlou.Id },
                new BookAuthor { BookId = bookAzAta.Id, AuthorId = authorAkhavan.Id },
                new BookAuthor { BookId = bookBarMadar.Id, AuthorId = authorNader.Id },
                new BookAuthor { BookId = bookRazMosaddegh.Id, AuthorId = authorMosaddegh.Id },
                new BookAuthor { BookId = bookShajarian.Id, AuthorId = authorShajarian.Id },
                new BookAuthor { BookId = bookShariati.Id, AuthorId = authorShariati.Id },
                // کتاب‌های بین‌المللی
                new BookAuthor { BookId = bookCleanCode2.Id, AuthorId = authorMartin.Id },
                new BookAuthor { BookId = bookCleanArch2.Id, AuthorId = authorMartin.Id },
                new BookAuthor { BookId = bookRefactoring2.Id, AuthorId = authorFowler.Id },
                new BookAuthor { BookId = bookDDD2.Id, AuthorId = authorEvans.Id },
                new BookAuthor { BookId = bookPragmatic2.Id, AuthorId = authorHunt.Id },
                new BookAuthor { BookId = bookThinkAndGrow2.Id, AuthorId = authorHunt.Id },
                new BookAuthor { BookId = bookCovey2.Id, AuthorId = authorCovey.Id }
            );

            #endregion

            #region BookKeywords

            context.BookKeywords.AddRange(
                // ایرانی
                new BookKeyword { BookId = bookGharbzadegi.Id, KeywordId = kwSiasat.Id },
                new BookKeyword { BookId = bookGharbzadegi.Id, KeywordId = kwHistory.Id },
                new BookKeyword { BookId = bookBlindOwl.Id, KeywordId = kwDastan.Id },
                new BookKeyword { BookId = bookBlindOwl.Id, KeywordId = kwRoman.Id },
                new BookKeyword { BookId = bookChamedan.Id, KeywordId = kwDastan.Id },
                new BookKeyword { BookId = bookNaghsh.Id, KeywordId = kwPoetry.Id },
                new BookKeyword { BookId = bookAzAta.Id, KeywordId = kwPoetry.Id },
                new BookKeyword { BookId = bookBarMadar.Id, KeywordId = kwRoman.Id },
                new BookKeyword { BookId = bookRazMosaddegh.Id, KeywordId = kwHistory.Id },
                new BookKeyword { BookId = bookRazMosaddegh.Id, KeywordId = kwSiasat.Id },
                new BookKeyword { BookId = bookShajarian.Id, KeywordId = kwMusic.Id },
                new BookKeyword { BookId = bookShajarian.Id, KeywordId = kwPoetry.Id },
                new BookKeyword { BookId = bookShariati.Id, KeywordId = kwSiasat.Id },
                new BookKeyword { BookId = bookShariati.Id, KeywordId = kwHistory.Id },
                // بین‌المللی
                new BookKeyword { BookId = bookCleanCode2.Id, KeywordId = kwClean.Id },
                new BookKeyword { BookId = bookCleanCode2.Id, KeywordId = kwDesign.Id },
                new BookKeyword { BookId = bookCleanArch2.Id, KeywordId = kwArch.Id },
                new BookKeyword { BookId = bookCleanArch2.Id, KeywordId = kwClean.Id },
                new BookKeyword { BookId = bookRefactoring2.Id, KeywordId = kwDesign.Id },
                new BookKeyword { BookId = bookDDD2.Id, KeywordId = kwDDD.Id },
                new BookKeyword { BookId = bookDDD2.Id, KeywordId = kwDesign.Id },
                new BookKeyword { BookId = bookPragmatic2.Id, KeywordId = kwClean.Id },
                new BookKeyword { BookId = bookPragmatic2.Id, KeywordId = kwDesign.Id },
                new BookKeyword { BookId = bookThinkAndGrow2.Id, KeywordId = kwSuccess.Id },
                new BookKeyword { BookId = bookCovey2.Id, KeywordId = kwLeadership.Id },
                new BookKeyword { BookId = bookCovey2.Id, KeywordId = kwSuccess.Id }
            );

            #endregion

            #region BookRelations

            context.BookRelations.AddRange(
                // کتاب‌های مرتبط ایرانی
                new BookRelation { BookId = bookGharbzadegi.Id, RelatedBookId = bookRazMosaddegh.Id },
                new BookRelation { BookId = bookRazMosaddegh.Id, RelatedBookId = bookGharbzadegi.Id },
                new BookRelation { BookId = bookBlindOwl.Id, RelatedBookId = bookChamedan.Id },
                new BookRelation { BookId = bookChamedan.Id, RelatedBookId = bookBlindOwl.Id },
                new BookRelation { BookId = bookNaghsh.Id, RelatedBookId = bookAzAta.Id },
                new BookRelation { BookId = bookAzAta.Id, RelatedBookId = bookNaghsh.Id },
                // کتاب‌های مرتبط بین‌المللی
                new BookRelation { BookId = bookCleanCode2.Id, RelatedBookId = bookCleanArch2.Id },
                new BookRelation { BookId = bookCleanArch2.Id, RelatedBookId = bookCleanCode2.Id },
                new BookRelation { BookId = bookPragmatic2.Id, RelatedBookId = bookCleanCode2.Id },
                new BookRelation { BookId = bookThinkAndGrow2.Id, RelatedBookId = bookCovey2.Id },
                new BookRelation { BookId = bookCovey2.Id, RelatedBookId = bookThinkAndGrow2.Id }
            );

            #endregion

            await context.SaveChangesAsync();

            CreateUser:
            if (!await userManager.Users.AnyAsync())
            {
                var user = new IdentityUser { UserName = "admin@bookstore.com", Email = "admin@bookstore.com" };
                await userManager.CreateAsync(user, "Admin@123");
            }
        }
    }
}
