using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.PostImages.Commands.Create;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.PostImages.Commands.Update;
using PawPal.Application.Modules.PostImages.GetById;
using PawPal.Application.Modules.PostImages.GetByIdFile;
using PawPal.Application.Modules.PostImages.ListMainImages;

namespace PawPal.API.Controllers.Posts
{

    [ApiController]
    [Route("[controller]")]
    public class PostImagesController(ISender sender, IWebHostEnvironment env, IAppDbContext context) : ControllerBase
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB per file

        [HttpPost]
        public async Task<ActionResult<int>> CreatePost([FromForm] CreatePostImageCommand command, CancellationToken cancellationToken)
        {
            foreach (var file in command.PostImages)
            {
                var ext = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(ext))
                {
                    return BadRequest($"File type '{ext}' is not allowed.");
                }
                if (file.Length > MaxFileSizeBytes)
                {
                    return BadRequest($"File '{file.FileName}' exceeds the {MaxFileSizeBytes / 1024 / 1024}MB limit.");
                }
            }

            int id = await sender.Send(command, cancellationToken);

            var subFolder = "posts";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string storeFileDirectory = Path.Combine(root, subFolder, "Post_" + command.PostId);

            if (!Directory.Exists(storeFileDirectory))
            {
                Directory.CreateDirectory(storeFileDirectory);
            }

            var savedRelativePaths = new List<string>();

            foreach (var file in command.PostImages)
            {
                string safeFileName = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(storeFileDirectory, safeFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                string relativePath = Path.Combine(subFolder, "Post_" + command.PostId, safeFileName)
                    .Replace("\\", "/");
                savedRelativePaths.Add(relativePath);
            }


            return Ok(id);
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
        [HttpGet("catalogImages")]
        public async Task<List<ListMainImageQueryDto>> GetMainImages([FromQuery(Name = "id")] List<int> request, CancellationToken cancellationToken)
        {
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var postImage = context.PostImages.AsQueryable();
            postImage = postImage.Where(x => request.Contains(x.PostId));
            if (postImage is null)
                throw new PawPalNotFoundException("PostImages not found");
            List<ListMainImageQueryDto> newPostImage = new();
            foreach (var img in postImage)
            {
                string drugiRut = root + img.MainImage;    
                newPostImage.Add(new ListMainImageQueryDto {PostID=img.PostId, MainImage =
                    await System.IO.File.ReadAllBytesAsync(drugiRut,cancellationToken) });
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
            foreach(var file in Directory.GetFiles(storeFileDirectory))
            {
                System.IO.File.Delete(file);
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
        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task Delete(int id,List<string> postImages, CancellationToken ct)
        {
            await sender.Send(new DeletePostImageCommand { PostId = id}, ct);
            var subFolder = "posts";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string storeFileDirectory = Path.Combine(root, subFolder, "Post_" + id);
            if (Directory.Exists(storeFileDirectory))
            {
                Directory.Delete(storeFileDirectory, true);
            }
            return;
        }
    }
}
