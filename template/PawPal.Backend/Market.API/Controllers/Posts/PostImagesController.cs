using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.PostImages.Commands.Create;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.PostImages.Commands.Update;
using PawPal.Application.Modules.PostImages.GetById;
using PawPal.Application.Modules.PostImages.GetByIdFile;
using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Application.Modules.Posts.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Update;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PawPal.API.Controllers.Posts
{

    [ApiController]
    [Route("[controller]")]
    public class PostImagesController(ISender sender, IWebHostEnvironment env,IAppDbContext context) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<int> CreatePost([FromForm]CreatePostImageCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            var subFolder = "posts";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string storeFileDirectory = Path.Combine(root, subFolder, "Post_" + command.PostId);
            Console.WriteLine("PATHHHHH AL ZA ROOOOOT->>>>", storeFileDirectory);
            if (!Directory.Exists(storeFileDirectory))
            {
                Directory.CreateDirectory(storeFileDirectory);
            }
            foreach (var file in command.PostImages)
            {
                string safeFileName = Path.GetFileName(file.FileName);
                string route = Path.Combine(storeFileDirectory, safeFileName);
                using (var ms = new MemoryStream())
                {
                    using (var stream = new FileStream(route, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                    }
                }
                var fileLocation = Path.Combine(subFolder, file.FileName).Replace("\\", "/");
            }

            return id;
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetPostImagesByIdDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostImagesById { PostId = id }, cancellationToken);
            return post;
        }
        [AllowAnonymous]
        [HttpGet("download/{id:int}")]
        public async Task<GetImagesPostByIdFileQueryDto> GetImageBlob(int id, CancellationToken cancellationToken)
        {
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        

            var postImage = await context.PostImages.Where(x => x.PostId == id).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
                throw new PawPalNotFoundException("PostImages not found");
            var newPostImage = new GetImagesPostByIdFileQueryDto
            {
                PostId = id,
                PostImages = new List<byte[]>()
            };

            foreach (var img in postImage.PhotoURL)
            {
                string route = root.Replace("\\", "/");
                string drugiRut = route + img;
                newPostImage.PostImages.Add(await System.IO.File.ReadAllBytesAsync(drugiRut));
            }
            
            return newPostImage;
        }
        
        [AllowAnonymous]
        [HttpPut]
        public async Task Update(UpdatePostImageCommand command, CancellationToken cancellationToken)
        {


            await sender.Send(command, cancellationToken);
            var subFolder = "posts";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string storeFileDirectory = Path.Combine(root, subFolder, "Post_" + command.PostId);
            if (!Directory.Exists(storeFileDirectory))
            {
                Directory.CreateDirectory(storeFileDirectory);
            }
            foreach (var file in command.PostImages)
            {
                string safeFileName = Path.GetFileName(file.FileName);
                string route = Path.Combine(storeFileDirectory, safeFileName);
                using (var ms = new MemoryStream())
                {
                    using (var stream = new FileStream(route, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                    }
                }
                var fileLocation = Path.Combine(subFolder, file.FileName).Replace("\\", "/");
            }

        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id,List<string> postImages, CancellationToken ct)
        {
            await sender.Send(new DeletePostImageCommand { PostId = id,PostImages = postImages }, ct);
        }
    }
}
