using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Queries.List
{
    public sealed class ListAnimalsQueryHandler(IAppDbContext context)
        : IRequestHandler<ListAnimalsQuery, PageResult<ListAnimalsQueryDto>>
    {
        public async Task<PageResult<ListAnimalsQueryDto>> Handle(ListAnimalsQuery request, CancellationToken cancellationToken)
        {
            var q = context.Animals.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchName))
                q = context.Animals.Where(x => x.Name.ToLower().Contains(request.SearchName.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.SearchBreed))
                q = context.Animals.Where(x => x.Breed.ToLower().Contains(request.SearchBreed.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.SearchGender))
                q = context.Animals.Where(x => x.Gender.GenderName.ToLower().
                Contains(request.SearchGender.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.SearchCategory))
                q = context.Animals.Where(x => x.Category.CategoryName.ToLower().Contains(
                    request.SearchCategory.ToLower()));

            var finalResult = q.OrderBy(x => x.Name).Select(x => new ListAnimalsQueryDto
            {
                Name = x.Name,
                Breed = x.Breed,
                Gender = x.Gender.GenderName,
                Category = x.Category.CategoryName,
            });
            return await PageResult<ListAnimalsQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);

        }
    }
}
