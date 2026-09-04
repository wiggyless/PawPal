using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using PawPal.Domain.Entities.Posts;
using PawPal.Shared.Constants;
using System.Collections.Generic;
using System.Linq;

namespace PawPal.Application.Modules.Posts.Commands.CreateAnimalPost
{
    public sealed class CreateAnimalPostCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IFileStorageService fileStorage)
        : IRequestHandler<CreateAnimalPostCommand, int>
    {
        public async Task<int> Handle(CreateAnimalPostCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.RoleId != Roles.VerifiedUser)
                throw new PawPalConflictException("User is not verified to make this action");

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == currentUser.UserId, cancellationToken);
            if (user == null)
                throw new PawPalNotFoundException("User does not exist inside the database");

            var gender = await context.Genders.FirstOrDefaultAsync(x => x.Id == request.GenderId, cancellationToken);
            if (gender == null)
                throw new PawPalNotFoundException($"Gender with Id {request.GenderId} does not exist!");

            var category = await context.AnimalCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
            if (category == null)
                throw new PawPalNotFoundException($"Category with Id {request.CategoryId} does not exist!");

            var breedsList = await context.Breeds.Where(b => b.CategoryID == request.CategoryId).ToListAsync(cancellationToken);
            var validatedBreed = breedsList.FirstOrDefault(b => b.Name.ToLower() == request.Breed.ToLower())?.Name ?? "";
            if (validatedBreed == "")
                throw new PawPalConflictException($"{request.Breed} is not a breed that belongs to the category {category.CategoryName}!");

            var requestedAllergyNames = request.Allergies
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim().ToLower())
                .Distinct()
                .ToList();
            var requestedDisabilityNames = request.Disabilities
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Select(d => d.Trim().ToLower())
                .Distinct()
                .ToList();

            var matchedAllergies = requestedAllergyNames.Count == 0
                ? new List<AllergiesEntity>()
                : await context.Allergies.Where(a => requestedAllergyNames.Contains(a.Name.ToLower())).ToListAsync(cancellationToken);
            if (matchedAllergies.Count != requestedAllergyNames.Count)
                throw new PawPalNotFoundException("This allergy does not exist in our database!");

            var matchedDisabilities = requestedDisabilityNames.Count == 0
                ? new List<DisabilitiesEntity>()
                : await context.Disabilities.Where(d => requestedDisabilityNames.Contains(d.Name.ToLower())).ToListAsync(cancellationToken);
            if (matchedDisabilities.Count != requestedDisabilityNames.Count)
                throw new PawPalNotFoundException("This disability does not exist in our database!");

            if (request.PostImages is null || request.PostImages.Count == 0)
                throw new ValidationException("At least one image is required.");

            var animal = new AnimalEntity
            {
                Name = request.Name,
                Breed = validatedBreed,
                Age = request.Age,
                GenderId = gender.Id,
                Gender = gender,
                HasPapers = request.HasPapers,
                ChildFriendly = request.ChildFriendly,
                CategoryId = category.Id,
                Category = category
            };

            var healthHistory = new AnimalHealthHistoryEntity
            {
                Animal = animal,
                Vaccinated = request.Vaccinated,
                SpayedOrNeutered = request.SpayedOrNeutered,
                ParasiteFree = request.ParasiteFree,
                DietaryRestrictions = request.DietaryRestrictions
            };

            var post = new PostsEntity
            {
                UserId = user.Id,
                User = user,
                CityId = user.CityId,
                Animal = animal,
                AnimalHealthHistory = healthHistory,
                DateAdded = DateTime.Now,
                Status = PostStatus.Active
            };

            await using var transaction = await context.BeginTransactionAsync(cancellationToken);
            string? subFolder = null;
            try
            {
                context.Animals.Add(animal);
                context.AnimalHealthHistories.Add(healthHistory);
                foreach (var allergy in matchedAllergies)
                {
                    context.AnimalsAllergies.Add(new AllergiesAnimalHealthHistory
                    {
                        AllergyId = allergy.Id,
                        Allergy = allergy,
                        AnimalHealthHistory = healthHistory
                    });
                }
                foreach (var disability in matchedDisabilities)
                {
                    context.AnimalsDisabilities.Add(new DisabilitiesAnimalHealthHistory
                    {
                        DisabilityId = disability.Id,
                        Disability = disability,
                        AnimalHealthHistory = healthHistory
                    });
                }
                context.Posts.Add(post);
                await context.SaveChangesAsync(cancellationToken);

                subFolder = $"posts/Post_{post.Id}";
                var savedPaths = await fileStorage.SaveFilesAsync(request.PostImages, subFolder, cancellationToken);

                context.PostImages.Add(new PostImagesEntity
                {
                    PostId = post.Id,
                    PhotoURL = savedPaths.ToList(),
                    MainImage = savedPaths[0]
                });
                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return post.Id;
            }
            catch
            {
                if (subFolder is not null)
                {
                    try { fileStorage.DeleteFolder(subFolder); } catch { /* best-effort cleanup, don't mask original exception */ }
                }
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
