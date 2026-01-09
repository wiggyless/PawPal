using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace PawPal.Application.Modules.Posts.Queries.List
{
    public sealed class ListPostQueryHandler(IAppDbContext context) : IRequestHandler<ListPostQuery,PageResult<ListPostQueryDto>>
    {
        public async Task<PageResult<ListPostQueryDto>> Handle(ListPostQuery request,CancellationToken cancellationToken)
        {
            var posts = context.Posts.Include(x=>x.Animal).AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchCityName))
                posts = posts.Where(x => x.City.Name.ToLower().Contains(request.SearchCityName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchCategoryName))
                posts = posts.Where(x => x.Animal.Category.CategoryName.ToLower().Contains(request.SearchCategoryName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchGender))
                posts = posts.Where(x => x.Animal.Gender.GenderName.ToLower().Contains(request.SearchGender.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchBreed))
                posts = posts.Where(x => x.Animal.Breed.ToLower().Contains(request.SearchBreed.ToLower()));
            if (request.SearchDateAddedMax !=null && request.SearchDateAddedMin !=null)
                posts = posts.Where(x => x.DateAdded >= request.SearchDateAddedMin && x.DateAdded <= request.SearchDateAddedMax);
            var postList = posts.OrderBy(x => x.Animal.Category.CategoryName).Select(x => new ListPostQueryDto
            {
                PostID = x.Id,
                UserID = x.UserId,
                Name = x.Animal.Name,
                AnimalID = x.AnimalID,
                CategoryID = x.Animal.CategoryId,
                Breed = x.Animal.Breed,
                GenderID = x.Animal.GenderId,
                CityID = x.CityId,
                Age = x.Animal.Age,
                DateAdded = x.DateAdded,
            });
        
            return await PageResult<ListPostQueryDto>.FromQueryableAsync(postList, request.Paging, cancellationToken);
        
        }
    }
}
