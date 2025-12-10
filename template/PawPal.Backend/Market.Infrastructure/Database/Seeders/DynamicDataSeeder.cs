using Market.Infrastructure.Database;
using PawPal.Application.Abstractions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
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
        await SeedRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedAnimalCategoriesAsync(context);
        await SeedGendersAsync(context);
        await SeedAnimalsAsync(context);
        await SeedCantonsAsync(context);
        await SeedCitiesAsync(context);
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
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<UserEntity>();

        var admin = new UserEntity
        {
            FirstName = "nesto",
            LastName = "nesto",
            Email = "admin@market.local",
            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
            IsAdmin = true,
            IsEnabled = true,
            RoleId = 1,
            CityId = 10
        };

        var user = new UserEntity
        {
            FirstName = "nesto",
            LastName = "nesto",
            Email = "manager@market.local",
            PasswordHash = hasher.HashPassword(null!, "User123!"),
            IsManager = true,
            IsEnabled = true,
            RoleId = 2,
            CityId = 11
        };

        var dummyForSwagger = new UserEntity
        {
            FirstName = "nesto",
            LastName = "nesto",
            Email = "string",
            PasswordHash = hasher.HashPassword(null!, "string"),
            IsEmployee = true,
            IsEnabled = true,
            RoleId = 1,
            CityId = 12
        };
        var dummyForTests = new UserEntity
        {
            FirstName = "nesto",
            LastName = "nesto",
            Email = "test",
            PasswordHash = hasher.HashPassword(null!, "test123"),
            IsEmployee = true,
            IsEnabled = true,
            RoleId = 3,
            CityId = 12
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
        var fishCategory = new AnimalCategoriesEntity{ CategoryName = "Fish"};
        var rabbitCategory = new AnimalCategoriesEntity {CategoryName = "Rabbit"};
        var birdCategory = new AnimalCategoriesEntity  { CategoryName = "Bird"};

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
        var female = new GenderEntity {  GenderName = "Female" };

        context.Genders.AddRange(male, female);
        await context.SaveChangesAsync();
    }
    private static async Task SeedAnimalsAsync(DatabaseContext context)
    {
        if (await context.Animals.AnyAsync())
            return;

        var femaleGender = await context.Genders.Where(x => x.GenderName.ToLower() == "female").FirstOrDefaultAsync();
        var maleGender = await context.Genders.Where(x => x.GenderName.ToLower() == "male").FirstOrDefaultAsync();

        var cat = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == "cat").FirstOrDefaultAsync();
        var dog = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == "dog").FirstOrDefaultAsync();
        var rabbit = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == "rabbit").FirstOrDefaultAsync();
        var fish = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == "fish").FirstOrDefaultAsync();
        var bird = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == "bird").FirstOrDefaultAsync();


        if (femaleGender != null && maleGender != null)
        {
            var krompir = new AnimalEntity
            {
                Name = "Krompir",
                Breed = "European Shorthair",
                Age = 2,
                GenderId = femaleGender.Id,
                ChildFriendly = true,
                HasPapers = false,
                CategoryId = cat.Id,
            };

            var kiki = new AnimalEntity
            {
                Name = "Kiki",
                Breed = "German Shepherd",
                Age = 1,
                GenderId = femaleGender.Id,
                ChildFriendly = false,
                HasPapers = false,
                CategoryId = dog.Id,
            };

            var loona = new AnimalEntity
            {
                Name = "Loona",
                Breed = "Budgie",
                Age = 1,
                GenderId = femaleGender.Id,
                ChildFriendly = true,
                HasPapers = false,
                CategoryId = bird.Id
            };

            var evilbnuy = new AnimalEntity
            {
                Name = "World Eater",
                Breed = "Dwarf Rabbit",
                Age = 5,
                GenderId = maleGender.Id,
                ChildFriendly = false,
                HasPapers = true,
                CategoryId = rabbit.Id
            };
            context.Animals.AddRange(krompir, kiki, loona, evilbnuy);
        }
        

        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: Animals added.");

    }
    private static async Task SeedAllergiesAsync(DatabaseContext context)
    {
        if (await context.Allergies.AnyAsync())
            return;

        var pollen = new AllergiesEntity
        {
            Name = "Pollen allergy",
            Description = "Pets with pollen allergies have" +
            " overactive immune systems that treat pollen as a threat. Your pet’s body then responds to this “threat” " +
            "with the pollen allergy symptoms you’ve probably noticed, like itchy, watery eyes, inflamed skin, sneezing, or hives."
        };
        var proteins = new AllergiesEntity
        {
            Name = "Protein allergy",
            Description = "Proteins like beef, chicken, dairy, or fish are common triggers for allergies in pets because our pets eat them most frequently." +
        "An allergy can occur after an animal is exposed to it long term. Symptoms include: hair loss, vomiting, diarrhea, infections and more."
        };

        var fleas = new AllergiesEntity
        {
            Name = "Flea allergy",
            Description = "Flea allergy dermatitis (FAD) is an allergy, more specifically, a hypersensitivity" +
            " reaction against the proteins in the flea's saliva. Common symptoms of flea allergy dermatitis in dogs include hair loss, crusting of the skin, itching and scratching, anemia, and lethargy."
        };
        context.Allergies.AddRange(pollen, proteins, fleas);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: Allergies added.");

    }
    private static async Task SeedDisabilitiesAsync(DatabaseContext context)
    {
        if (await context.Disabilities.AnyAsync())
            return;

        var cerebellar = new DisabilitiesEntity
        {
            Name = "Cerebellar Hypoplasia",
            Description = "Cerebellar Hypoplasia), abbreviated to CH, is a disorder found in cats and " +
            "dogs which causes jerky movements, tremors, and generally uncoordinated motion, just like ataxic cerebral palsy in humans."
        };

        var blind = new DisabilitiesEntity
        {
            Name = "Blindness",
            Description = "Blindness has several causes. Cataract is treated surgically and glaucoma with eye drops or surgery. Other causes are trauma, abnormal blood vessel growth in premature birth and diabetes."
        };
        var deaf = new DisabilitiesEntity
        {
            Name = "Deafness",
            Description = "Animals can lose or be born without the sense of hearing. Most commonly, deafness is associated with specific pigmentation phenotypes, including white coat color, and is heritable."
        };

        context.Disabilities.AddRange(cerebellar,  blind, deaf);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Dynamic seed: Disabilities added.");
    }
    private static async Task SeedAnimalHealthHistoriesAsync(DatabaseContext context)
    {

        if (await context.AnimalHealthHistories.AnyAsync())
            return;

        var krompir = await context.Animals.Where(x => x.Name.ToLower().Contains("krompir")).FirstOrDefaultAsync();
        var krompirHealth = new AnimalHealthHistoryEntity
        {
            AnimalId = krompir.Id,
            Animal = krompir,
            ParasiteFree = true,
            Vaccinated = false,
            DietaryRestrictions = "None",
            SpayedOrNeutered = false
        };
        context.AnimalHealthHistories.Add(krompirHealth);
        await context.SaveChangesAsync();

        var animalAllergy = new AllergiesAnimalHealthHistory
        {
            AnimalHealthHistoryId = krompirHealth.Id,
            AllergyId = 1
        };
        context.AnimalsAllergies.Add(animalAllergy);
        await context.SaveChangesAsync();

        var evilBnuy = await context.Animals.Where(x => x.Name.ToLower() == "World Eater".ToLower()).FirstOrDefaultAsync();

        var evilBnuyHealth = new AnimalHealthHistoryEntity
        {
            AnimalId = evilBnuy.Id,
            Animal = evilBnuy,
            ParasiteFree = true,
            Vaccinated = true,
            DietaryRestrictions = "Do not feed too much, he is fat",
            SpayedOrNeutered = true
        };
        context.AnimalHealthHistories.Add(evilBnuyHealth);
        await context.SaveChangesAsync();

        var animalDisability = new DisabilitiesAnimalHealthHistory
        {
            AnimalHealthHistoryId = evilBnuyHealth.Id,
            AnimalHealthHistory = evilBnuyHealth,
            DisabilityId = 3
        };
        context.AnimalsDisabilities.Add(animalDisability);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: Animal Health Histories added.");


    }
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
    private static async Task SeedRolesAsync(DatabaseContext ct)
    {
        if(await ct.Roles.AnyAsync()) return;

        var basicUser = new RolesEntity
        {
            RoleName = "Basic user"
        };
        var verifiedUser = new RolesEntity
        {
            RoleName = "Verified user"
        };

        var admin = new RolesEntity
        {
            RoleName = "Admin"
        };

        ct.AddRange(basicUser, verifiedUser, admin);
        await ct.SaveChangesAsync();
    }
}