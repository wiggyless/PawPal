using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.ListPostsByUserId
{
    public sealed class ListPostByUserIdQueryHandler(IAppDbContext context) : IRequestHandler<ListPostByUserIdQuery,PageResult<ListPostByUserIdQueryDto>>
    {
        public async Task<PageResult<ListPostByUserIdQueryDto>> Handle(ListPostByUserIdQuery request,CancellationToken cancellationToken)
        {
            var postList = context.Posts.Include(x=>x.Animal).AsQueryable();
            var postImagesList = context.PostImages.AsQueryable();
            postList = postList.Where(x=>x.UserId == request.UserId);
            var finalList = postList.OrderBy(x => x.AnimalID).Select(x => new ListPostByUserIdQueryDto
            {
                AnimalName = x.Animal.Name,
                UserId = x.UserId,
                PostId = x.Id,
                FirstImage = (postImagesList.FirstOrDefault(y => y.PostId == x.Id).MainImage == null)?"": 
                postImagesList.FirstOrDefault(y => y.PostId == x.Id).MainImage
            }); 
            return await PageResult<ListPostByUserIdQueryDto>.FromQueryableAsync(finalList,request.Paging,cancellationToken);
        }
    }
}
