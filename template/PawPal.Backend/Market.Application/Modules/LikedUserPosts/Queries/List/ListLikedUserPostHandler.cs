using PawPal.Application.Modules.Gender.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Queries.List
{
    public sealed class ListLikedUserPostHandler(IAppDbContext context) : IRequestHandler<ListLikedUserPost,ListLikedUserPostDto>
    {
        public async Task<ListLikedUserPostDto> Handle(ListLikedUserPost request,CancellationToken cancellationToken)
        {
            var list = context.LikedUserPosts.Include(x => x.Post).Where(x => x.UserId == request.UserId).AsNoTracking();
            if(list is null)
            {
                throw new PawPalNotFoundException("List is empty");
            }
            var finalList = new ListLikedUserPostDto { UserId = request.UserId,PostList = new List<int>() };
            var filterSet = new HashSet<int>(request.PostIdList);
            finalList.PostList = list.Where(x => filterSet.Contains(x.PostId)).Select(x => x.PostId).ToList();
            return finalList;
        }
    }
}
