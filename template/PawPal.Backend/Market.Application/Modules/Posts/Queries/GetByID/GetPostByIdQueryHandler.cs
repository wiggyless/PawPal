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
                    Id = x.Id,
                    AnimalID = x.AnimalID,
                    UserId = x.UserId,
                    DateAdded = x.DateAdded,
                }).FirstOrDefaultAsync(cancelationToken);
            if (post == null) throw new PawPalNotFoundException("Post not found");
            return post;
        }
    }
}
