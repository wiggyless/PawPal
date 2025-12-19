using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.List
{
    public sealed class ListPostQueryHandler(IAppDbContext context) : IRequestHandler<ListPostQuery,PageResult<ListPostQueryDto>>
    {
        public async Task<PageResult<ListPostQueryDto>> Handle(ListPostQuery request,CancellationToken cancellationToken)
        {
            var posts = context.Posts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchCityName))
                posts = context.Posts.Where(x => x.City.Name.ToLower().Contains(request.SearchCityName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchCategoryName))
                posts = context.Posts.Where(x => x.Animal.Category.CategoryName.ToLower().Contains(request.SearchCategoryName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchGender))
                posts = context.Posts.Where(x => x.Animal.Gender.GenderName.ToLower().Contains(request.SearchGender.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchBreed))
                posts = context.Posts.Where(x => x.Animal.Breed.ToLower().Contains(request.SearchBreed.ToLower()));
            if (request.SearchDateAddedMax !=null && request.SearchDateAddedMin !=null)
                posts = context.Posts.Where(x => x.DateAdded >= request.SearchDateAddedMin && x.DateAdded <= request.SearchDateAddedMax);
            var postList = posts.OrderBy(x => x.Animal.Category.CategoryName).Select(x => new ListPostQueryDto
            {
                UserName = x.User.FirstName,
                Animal = x.Animal,
                CityName = x.City.Name,
                DateAdded = x.DateAdded,
                // more paramteres to be added myb ;D
            });
            return await PageResult<ListPostQueryDto>.FromQueryableAsync(postList, request.Paging, cancellationToken);
        
        }
    }
}
