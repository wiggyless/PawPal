using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using PawPal.Domain.Entities.Adoptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.ListHistory
{
    internal class ListAdoptionRequestHistoryQueryHandler (IAppDbContext context, IAppCurrentUser currentUser):
        IRequestHandler<ListAdoptionRequestHistoryQuery, PageResult<ListAdoptionRequestHistoryQueryDto>>
    {
        public async Task<PageResult<ListAdoptionRequestHistoryQueryDto>> Handle(ListAdoptionRequestHistoryQuery request, CancellationToken cancellationToken)
        {
            request.UserID = currentUser.UserId ?? throw new PawPalConflictException("User is not authenticated");

            var reqList = context.AdoptionRequests.Include(x => x.Post)
                .Include(x => x.Post.Animal)
                .Include(x => x.Post.City)
                .Include(x => x.Post.City.Canton)
                .Include(x => x.Post.Animal.Gender).AsQueryable();
            reqList = reqList.Where(x => x.Post.UserId == request.UserID).AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
            {
                var matchingStatuses = Enum.GetValues<AdoptionRequestStatus>()
                    .Where(s => s.ToString().Contains(request.SearchStatus, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                reqList = reqList.Where(x => matchingStatuses.Contains(x.Status));
            }
            if (request.SearchDateSent != null)
            {
                reqList = reqList.Where(x => x.DateSent == request.SearchDateSent);
            }
            var finalList = reqList.OrderBy(x => x.DateSent).Select(y => new ListAdoptionRequestHistoryQueryDto
            {
                RequestId = y.Id,
                Name = y.Post.Animal.Name,
                Gender = y.Post.Animal.Gender.GenderName,
                Breed = y.Post.Animal.Breed,
                City = y.Post.City.Name,
                Canton = y.Post.City.Canton.FullName,
                Status = y.Status == AdoptionRequestStatus.Accepted ? "Accepted"
                    : y.Status == AdoptionRequestStatus.Denied ? "Denied"
                    : "Pending",
                DateSent = y.DateSent,
                RequirementId = y.RequirementId,
                UserID = y.UserId,
                PostID = y.Post.Id,
            }).AsQueryable();

            return await PageResult<ListAdoptionRequestHistoryQueryDto>.FromQueryableAsync(finalList,
                request.Paging, cancellationToken);
        }
    }
}
