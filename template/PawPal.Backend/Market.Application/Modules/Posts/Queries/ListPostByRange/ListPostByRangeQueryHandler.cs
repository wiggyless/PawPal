using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PawPal.Application.Modules.Posts.Queries.ListPostByRange
{
    public sealed class ListPostByRangeQueryHandler(IAppDbContext context) : IRequestHandler<ListPostByRangeQuery, PageResult<ListPostByRangeQueryDto>>
    {
        public async Task<PageResult<ListPostByRangeQueryDto>> Handle(ListPostByRangeQuery request, CancellationToken cancellationToken)
        {
            var postList = context.Posts.Include(x => x.Animal).Include(x => x.Animal.Gender).AsQueryable();
            var postImagesList = context.PostImages.AsQueryable();
            var list = postList.Where(x => request.PostIdList.Contains(x.Id)).OrderBy(x => x.Id)
                .Select(x => new ListPostByRangeQueryDto
                {
                    AnimalName = x.Animal.Name,
                    UserId = x.UserId,
                    PostId = x.Id,
                    FirstImage = (postImagesList.FirstOrDefault(y => y.PostId == x.Id).MainImage == null) ? "" :
                postImagesList.FirstOrDefault(y => y.PostId == x.Id).MainImage,
                    AnimalID = x.AnimalID,
                    CityID = x.CityId,
                    Gender = x.Animal.Gender.GenderName,
                    Breed = x.Animal.Breed,
                });

            return await PageResult<ListPostByRangeQueryDto>.FromQueryableAsync(list, request.Paging, cancellationToken);
        }

    }
}