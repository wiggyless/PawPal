using Microsoft.AspNetCore.Http;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.ListHistory
{
    internal class ListAdoptionRequestHistoryQueryHandler (IAppDbContext context):
        IRequestHandler<ListAdoptionRequestHistoryQuery, PageResult<ListAdoptionRequestHistoryQueryDto>>
    {
        public async Task<PageResult<ListAdoptionRequestHistoryQueryDto>> Handle(ListAdoptionRequestHistoryQuery request, CancellationToken cancellationToken)
        {
            var reqList = context.AdoptionRequests.Include(x => x.Post)
                .Include(x => x.Post.Animal)
                .Include(x => x.Post.City)
                .Include(x => x.Post.City.Canton)
                .Include(x => x.Post.Animal.Gender).AsQueryable();
            reqList = reqList.Where(x => x.Post.UserId == request.UserID && x.Status.ToLower() != "sent").AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
                reqList = reqList.Where(x => x.Status.ToLower().Contains(request.SearchStatus.ToLower()));
            if (request.SearchDateSent != null)
            {
                reqList = reqList.Where(x => x.DateSent == request.SearchDateSent);
            }
            var finalList = reqList.OrderBy(x => x.DateSent).Select(y => new ListAdoptionRequestHistoryQueryDto
            {
                RequestId = y.Id,
                Name = y.Post.Animal.Name,
                Gender = y.Post.Animal.Name,
                Breed = y.Post.Animal.Breed,
                City = y.Post.City.Name,
                Canton = y.Post.City.Canton.FullName,
                Status = y.Status,
                DateSent = y.DateSent,
                RequirementId = y.RequirementId,
                UserID = y.UserId,
                PostID = y.Post.Id,
            }).AsQueryable();

            return await PageResult<ListAdoptionRequestHistoryQueryDto>.FromQueryableAsync(finalList,
                request.Paging, cancellationToken);
            /*


        // this is something new I learned to use. Way faster and easier to understand than our stoneage way 
        // Here you you have a list of posts where you associate one post with a list of requests and what attributes you take
        var combined = postList.GroupJoin(
                        reqList,
                        post => post.Id,               // Key from Posts
                        request => request.PostId,      // Key from AdoptionRequests
                        (post, requests) => new {       // Result selector
                        PostId = post.Id,
                        Name = post.Animal.Name,
                        Gender = post.Animal.Gender.GenderName,
                        Breed = post.Animal.Breed,
                        City = post.City.Name,
                        Canton = post.City.Canton.FullName,
                         Requests = requests.ToList() // This is now a collection
                        }
                        ).ToList();

        var finalList = new List<ListAdoptionRequestQueryDto>();
        foreach (var item in combined) { 
            foreach(var req in item.Requests)
            {
                finalList.Add(new ListAdoptionRequestQueryDto
                {
                    RequestId = req.Id,
                    Name = item.Name,
                    Gender = item.Gender,
                    Breed = item.Breed,
                    City = item.City,
                    Canton = item.Canton,
                    Status = req.Status,
                    DateSent = req.DateSent,
                    RequirementId = req.Id,
                    UserID = req.UserId,

                });
            }
        }

        */
        }
    }
}
