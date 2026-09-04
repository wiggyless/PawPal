using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PawPal.API.Extensions;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Delete_;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Application.Modules.Posts.Commands.CreateAnimalPost;
using PawPal.Application.Modules.Posts.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Update;
using PawPal.Application.Modules.Posts.Commands.UpdateAnimalPost;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using PawPal.Application.Modules.Posts.Queries.List;
using PawPal.Application.Modules.Posts.Queries.ListPostByRange;
using PawPal.Application.Modules.Posts.Queries.ListPostsByUserId;
namespace PawPal.API.Controllers.Posts
{
    [EnableRateLimiting("IpBasedPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class PostsController(ISender sender) : ControllerBase
    {
        [HttpPost]

        public async Task<ActionResult<int>> CreatePost(CreatePostCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]

        public async Task<GetPostByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostByIdQuery { Id = id }, cancellationToken);
            return post;
        }
        [AllowAnonymous]
        [HttpGet("userPost")]

        public async Task<PageResult<ListPostByUserIdQueryDto>> GetPostListById([FromQuery] ListPostByUserIdQuery query, CancellationToken cancellationToken)
        {
            var list = await sender.Send(query, cancellationToken);
            return list;
        }

        [HttpGet("likedPost")]

        public async Task<PageResult<ListPostByRangeQueryDto>> GetPostListLiked([FromQuery] ListPostByRangeQuery query, CancellationToken cancellationToken)
        {
            var list = await sender.Send(query, cancellationToken);
            return list;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListPostQueryDto>> List([FromQuery] ListPostQuery query, CancellationToken token)
        {
            var list = await sender.Send(query, token);
            return list;
        }

        [HttpPut("{id:int}")]
        public async Task Update(UpdatePostCommand upc, int id, CancellationToken ct)
        {
            upc.Id = id;
            await sender.Send(upc, ct);
        }

        [HttpPost("animal")]
        public async Task<ActionResult<int>> CreateAnimalPost([FromForm] CreateAnimalPostRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateAnimalPostCommand
            {
                Name = request.Name,
                Breed = request.Breed,
                GenderId = request.GenderId,
                Age = request.Age,
                HasPapers = request.HasPapers,
                ChildFriendly = request.ChildFriendly,
                CategoryId = request.CategoryId,
                Vaccinated = request.Vaccinated,
                SpayedOrNeutered = request.SpayedOrNeutered,
                ParasiteFree = request.ParasiteFree,
                DietaryRestrictions = request.DietaryRestrictions,
                Allergies = request.Allergies,
                Disabilities = request.Disabilities,
                PostImages = request.PostImages.ToFileUploads()
            };
            int id = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("animal/{id:int}")]
        public async Task UpdateAnimalPost(int id, [FromForm] UpdateAnimalPostRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateAnimalPostCommand
            {
                PostId = id,
                Name = request.Name,
                Breed = request.Breed,
                GenderId = request.GenderId,
                Age = request.Age,
                HasPapers = request.HasPapers,
                ChildFriendly = request.ChildFriendly,
                CategoryId = request.CategoryId,
                Vaccinated = request.Vaccinated,
                SpayedOrNeutered = request.SpayedOrNeutered,
                ParasiteFree = request.ParasiteFree,
                DietaryRestrictions = request.DietaryRestrictions,
                Allergies = request.Allergies,
                Disabilities = request.Disabilities,
                PostImages = request.PostImages.ToFileUploads()
            };
            await sender.Send(command, cancellationToken);
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(DeletePostCommand deletePost, CancellationToken ct)
        {
            await sender.Send(deletePost, ct);
            await sender.Send(new DeletePostImageCommand { PostId = deletePost.Id }, ct);
        }
    }
}