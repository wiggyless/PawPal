using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.GetByID
{
    public sealed class GetPostByIdQueryHandler(IAppDbContext context): 
        IRequestHandler<GetPostByIdQuery, GetPostByIdQueryDto>
    {
        public async Task<GetPostByIdQueryDto> Handle(GetPostByIdQuery request,CancellationToken cancelationToken)
        {
            var post = await context.Posts.
                Where(x => x.Id == request.Id).
                Select(x => new GetPostByIdQueryDto
                {
                    PostID = x.Id,
                    UserID = x.UserId,
                    Name = x.Animal.Name,
                    AnimalID = request.Id,
                    CategoryID = x.Animal.CategoryId,
                    Breed = x.Animal.Breed,
                    GenderID = x.Animal.GenderId,
                    CityID = x.CityId,
                    Age = x.Animal.Age,
                    DateAdded = x.DateAdded,
                }).FirstOrDefaultAsync(cancelationToken);
            if (post == null) throw new PawPalNotFoundException("Post not found");
            return post;
        }
    }
}
