using Market.Infrastructure.Database;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Places;

namespace PawPal.Infrastructure.Database.Seeders;

/// <summary>
/// Dynamic seeder koji se pokreće u runtime-u,
/// obično pri startu aplikacije (npr. u Program.cs).
/// Koristi se za unos demo/test podataka koji nisu dio migracije.
/// </summary>
public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Osiguraj da baza postoji (bez migracija)
        await context.Database.EnsureCreatedAsync();

        await SeedProductCategoriesAsync(context);
        await SeedUsersAsync(context);
        await SeedAnimalCategoriesAsync(context);
        await SeedGendersAsync(context);
        await SeedAnimalsAsync(context);
        //await SeedCantonsAsync(context);
        //await SeedCitiesAsync(context);
    }

    private static async Task SeedProductCategoriesAsync(DatabaseContext context)
    {
        if (!await context.ProductCategories.AnyAsync())
        {
            context.ProductCategories.AddRange(
                new ProductCategoryEntity
                {
                    Name = "Računari (demo)",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductCategoryEntity
                {
                    Name = "Mobilni uređaji (demo)",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                }
            );

            await context.SaveChangesAsync();
            Console.WriteLine("✅ Dynamic seed: product categories added.");
        }
    }

    /// <summary>
    /// Kreira demo korisnike ako ih još nema u bazi.
    /// </summary>
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<MarketUserEntity>();

        var admin = new MarketUserEntity
        {
            Email = "admin@market.local",
            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
            IsAdmin = true,
            IsEnabled = true,
        };

        var user = new MarketUserEntity
        {
            Email = "manager@market.local",
            PasswordHash = hasher.HashPassword(null!, "User123!"),
            IsManager = true,
            IsEnabled = true,
        };

        var dummyForSwagger = new MarketUserEntity
        {
            Email = "string",
            PasswordHash = hasher.HashPassword(null!, "string"),
            IsEmployee = true,
            IsEnabled = true,
        };
        var dummyForTests = new MarketUserEntity
        {
            Email = "test",
            PasswordHash = hasher.HashPassword(null!, "test123"),
            IsEmployee = true,
            IsEnabled = true,
        };
        context.Users.AddRange(admin, user, dummyForSwagger, dummyForTests);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: demo users added.");
    }

    private static async Task SeedAnimalCategoriesAsync(DatabaseContext context)
    {
        if (await context.AnimalCategories.AnyAsync())
            return;

        var catCategory = new AnimalCategoriesEntity { CategoryName = "Cat"};
        var dogCategory = new AnimalCategoriesEntity { CategoryName = "Dog"};
        var fishCategory = new AnimalCategoriesEntity{CategoryName = "Fish"};
        var rabbitCategory = new AnimalCategoriesEntity { CategoryName = "Rabbit"};
        var birdCategory = new AnimalCategoriesEntity  {  CategoryName = "Bird"};

        context.AnimalCategories.AddRange(catCategory, dogCategory, fishCategory, 
            rabbitCategory, birdCategory);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: Animal Categories added.");

    }

    private static async Task SeedGendersAsync(DatabaseContext context)
    {
        if (await context.Genders.AnyAsync())
            return;

        var male = new GenderEntity { GenderName = "Male" };
        var female = new GenderEntity { GenderName = "Female" };

        context.Genders.AddRange(male, female);
        await context.SaveChangesAsync();
    }
    private static async Task SeedAnimalsAsync(DatabaseContext context)
    {
        if (await context.Animals.AnyAsync())
            return;
        var krompir = new AnimalEntity
        {
            Name = "Krompir",
            Breed = "European Shorthair",
            Age = 2,
            GenderId = 1,
            ChildFriendly = true,
            HasPapers = false,
            CategoryId = 1
        };

        var kiki = new AnimalEntity
        {
            Name = "Kiki",
            Breed = "German Shepherd",
            Age = 1,
            GenderId = 1,
            ChildFriendly = false,
            HasPapers = false,
            CategoryId = 1
        };

        var loona = new AnimalEntity
        {
            Name = "Loona",
            Breed = "Budgie",
            Age = 1,
            GenderId = 1,
            ChildFriendly = true,
            HasPapers = false,
            CategoryId = 1
        };

        context.Animals.AddRange(krompir, kiki, loona);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: Animals added.");

    }
    // To be fixed
    /*
    private static async Task SeedCitiesAsync(DatabaseContext ct)
    {
        if (await ct.Cities.AnyAsync()) return;

        var tesanj = new CitiesEntity
        {
            Name = "Tešanj",
            Region = "?",
            PostalCode = "74260",
            CantonId = 2,
        };
        var mostar = new CitiesEntity
        {
            Name = "Mostar",
            Region = "?",
            PostalCode = "88000",
            CantonId = 3,
        };
        var bihac = new CitiesEntity
        {
            Name = "Bihać",
            Region = "?",
            PostalCode = "77000",
            CantonId = 1,
        };
        ct.Cities.AddRange(tesanj, mostar, bihac);
        await ct.SaveChangesAsync();
        Console.WriteLine("✅ Dynamic seed: Cantons added.");
    }
    private static async Task SeedCantonsAsync(DatabaseContext ct)
    {
        if (await ct.Cities.AnyAsync()) return;

        var usk = new CantonEntity
        {
            FullName = "Unsko-Sanski",
            Abbreviation = "USK"
        };
        var zdk = new CantonEntity
        {
            FullName = "Zeničko-dobojski kanton",
            Abbreviation = "ZDK"
        };
        var hnk = new CantonEntity
        {
            FullName = "Hercegovačko-neretvanski kanton",
            Abbreviation = "HNK",
        };
        ct.Cantons.AddRange(usk, zdk, hnk);
        await ct.SaveChangesAsync();
        Console.WriteLine("✅ Dynamic seed: Cantons added.");
    }
    */
}