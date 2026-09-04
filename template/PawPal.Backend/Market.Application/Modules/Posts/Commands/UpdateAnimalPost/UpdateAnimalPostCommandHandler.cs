using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using PawPal.Domain.Entities.Posts;
using PawPal.Shared.Constants;
using System.Collections.Generic;
using System.Linq;

namespace PawPal.Application.Modules.Posts.Commands.UpdateAnimalPost
{
    public sealed class UpdateAnimalPostCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IFileStorageService fileStorage)
        : IRequestHandler<UpdateAnimalPostCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnimalPostCommand request, CancellationToken cancellationToken)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);
            if (post == null)
                throw new PawPalNotFoundException($"Post with Id {request.PostId} does not exist!");

            if (post.UserId != currentUser.UserId && currentUser.RoleId != Roles.Admin)
                throw new PawPalConflictException("User is not allowed to do this action");

            var animal = await context.Animals.FirstOrDefaultAsync(a => a.Id == post.AnimalID, cancellationToken);
            if (animal == null)
                throw new PawPalNotFoundException($"Animal with Id {post.AnimalID} does not exist!");

            var healthHistory = await context.AnimalHealthHistories.FirstOrDefaultAsync(h => h.AnimalId == animal.Id, cancellationToken);
            var isNewHealthHistory = healthHistory == null;
            healthHistory ??= new AnimalHealthHistoryEntity { AnimalId = animal.Id, Animal = animal };

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

            var currentAllergies = isNewHealthHistory
                ? new List<AllergiesAnimalHealthHistory>()
                : await context.AnimalsAllergies.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);
            var currentDisabilities = isNewHealthHistory
                ? new List<DisabilitiesAnimalHealthHistory>()
                : await context.AnimalsDisabilities.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);

            await using var transaction = await context.BeginTransactionAsync(cancellationToken);
            string? subFolder = null;
            try
            {
                animal.Name = request.Name.Trim();
                animal.Breed = validatedBreed;
                animal.Age = request.Age;
                animal.GenderId = gender.Id;
                animal.Gender = gender;
                animal.HasPapers = request.HasPapers;
                animal.ChildFriendly = request.ChildFriendly;
                animal.CategoryId = category.Id;
                animal.Category = category;

                healthHistory.Vaccinated = request.Vaccinated;
                healthHistory.SpayedOrNeutered = request.SpayedOrNeutered;
                healthHistory.ParasiteFree = request.ParasiteFree;
                healthHistory.DietaryRestrictions = request.DietaryRestrictions;
                if (isNewHealthHistory)
                {
                    context.AnimalHealthHistories.Add(healthHistory);
                    post.AnimalHealthHistory = healthHistory;
                }

                var newAllergyIds = matchedAllergies.Select(a => a.Id).ToList();
                var newDisabilityIds = matchedDisabilities.Select(d => d.Id).ToList();

                foreach (var toRemove in currentAllergies.Where(x => !newAllergyIds.Contains(x.AllergyId)))
                    context.AnimalsAllergies.Remove(toRemove);
                foreach (var toRemove in currentDisabilities.Where(x => !newDisabilityIds.Contains(x.DisabilityId)))
                    context.AnimalsDisabilities.Remove(toRemove);

                foreach (var toAdd in matchedAllergies.Where(a => !currentAllergies.Select(x => x.AllergyId).Contains(a.Id)))
                    context.AnimalsAllergies.Add(new AllergiesAnimalHealthHistory { AllergyId = toAdd.Id, Allergy = toAdd, AnimalHealthHistory = healthHistory });
                foreach (var toAdd in matchedDisabilities.Where(d => !currentDisabilities.Select(x => x.DisabilityId).Contains(d.Id)))
                    context.AnimalsDisabilities.Add(new DisabilitiesAnimalHealthHistory { DisabilityId = toAdd.Id, Disability = toAdd, AnimalHealthHistory = healthHistory });

                await context.SaveChangesAsync(cancellationToken);

                subFolder = $"posts/Post_{post.Id}";
                fileStorage.DeleteFolder(subFolder);
                var savedPaths = await fileStorage.SaveFilesAsync(request.PostImages, subFolder, cancellationToken);

                var postImages = await context.PostImages.FirstOrDefaultAsync(x => x.PostId == post.Id, cancellationToken);
                if (postImages == null)
                {
                    postImages = new PostImagesEntity { PostId = post.Id };
                    context.PostImages.Add(postImages);
                }
                postImages.PhotoURL = savedPaths.ToList();
                postImages.MainImage = savedPaths[0];
                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return Unit.Value;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
