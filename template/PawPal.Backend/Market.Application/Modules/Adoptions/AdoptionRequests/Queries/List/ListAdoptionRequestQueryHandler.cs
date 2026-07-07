using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List
{
    public sealed class ListAdoptionRequestQueryHandler(IAppDbContext context) :
        IRequestHandler<ListAdoptionRequestQuery,PageResult<ListAdoptionRequestQueryDto>>
    {
        public async Task<PageResult<ListAdoptionRequestQueryDto>> Handle(ListAdoptionRequestQuery request,CancellationToken cancellationToken)
        {
            var reqList = context.AdoptionRequests.Include(x => x.Post)
                .Include(x => x.Post.Animal)
                .Include(x => x.Post.City)
                .Include(x => x.Post.City.Canton)
                .Include(x => x.Post.Animal.Gender).AsQueryable();

            if (request.Sent)
            {
                reqList = reqList.Where(x => x.UserId == request.UserID).AsQueryable();
            }
            else
            {
                reqList = reqList.Where(x => x.Post.UserId == request.UserID).AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
            {
                reqList = reqList.Where(x => x.Status.ToLower().Contains(request.SearchStatus.ToLower()));
            }
            else
            {
                reqList = reqList.Where(x => x.Status.ToLower() == "pending");
            }

            if (request.SearchDateSent != null)
            {
                reqList = reqList.Where(x => x.DateSent == request.SearchDateSent);
            }

            var finalList = reqList.OrderBy(x => x.DateSent).Select(y => new ListAdoptionRequestQueryDto
            {
                RequestId = y.Id,
                Name = y.Post.Animal.Name,
                Gender = y.Post.Animal.Gender.GenderName,
                Breed = y.Post.Animal.Breed,
                City = y.Post.City.Name,
                Canton = y.Post.City.Canton.FullName,
                Status = y.Status,
                DateSent = y.DateSent,
                RequirementId = y.RequirementId,
                UserID = y.UserId,
                PostID = y.Post.Id,
                MainImage = context.PostImages
                        .Where(img => img.PostId == y.PostId)
                        .Select(img => img.MainImage)
                        .FirstOrDefault() ?? " "
            }).AsQueryable();

            return await PageResult<ListAdoptionRequestQueryDto>.FromQueryableAsync(finalList,
                request.Paging, cancellationToken);
        }
    }
}