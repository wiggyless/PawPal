using Microsoft.EntityFrameworkCore;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.PostImages.Commands.Create;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.PostImages.Commands.Update;
using PawPal.Application.Modules.PostImages.GetById;
using PawPal.Application.Modules.PostImages.GetByIdFile;
using PawPal.Application.Modules.PostImages.ListMainImages;
using PawPal.Application.Modules.UserImages.Commands.Create;
using PawPal.Application.Modules.UserImages.Commands.Update;
using PawPal.Application.Modules.UserImages.Queries.GetById;

namespace PawPal.API.Controllers.UserImage
{
    [ApiController]
    [Route("[controller]")]
    public class UserImageController(ISender sender, IWebHostEnvironment env, IAppDbContext context) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<int> CreatePost([FromForm] CreateUserImageCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            var subFolder = "users";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string storeFileDirectory = Path.Combine(root, subFolder, "User_" + command.UserID);
            if (!Directory.Exists(storeFileDirectory))
            {
                Directory.CreateDirectory(storeFileDirectory);
            }
            string safeFileName = Path.GetFileName(command.Image.FileName);
            string route = Path.Combine(storeFileDirectory, safeFileName);
            using (var stream = new FileStream(route, FileMode.Create))
            {
                await command.Image.CopyToAsync(stream, cancellationToken);
            }
            return id;
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetUserImageByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await sender.Send(new GetUserImageByIdQuery { UserID = id }, cancellationToken);
            return user;
        }
        [AllowAnonymous]
        [HttpGet("download/{id:int}")]
        public async Task<GetImagesPostByIdFileQueryDto> GetImageBlob(int id, CancellationToken cancellationToken)
        {
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


            var userImage = await context.UserImage.Where(x => x.UserID == id).FirstOrDefaultAsync(cancellationToken);
            if (userImage is null)
                throw new PawPalNotFoundException("UserImage not found");
            var newPostImage = new GetImagesPostByIdFileQueryDto
            {
                PostId = id,
                PostImages = new List<byte[]>()
            };

            string route = root.Replace("\\", "/");
            string fullPath = route + userImage.PhotoURL;
            newPostImage.PostImages.Add(await System.IO.File.ReadAllBytesAsync(fullPath));

            return newPostImage;
        }


        [HttpPut]
        public async Task Update([FromForm] UpdateUserImageCommand command, CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken);
            var subFolder = "users";
            string root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string storeFileDirectory = Path.Combine(root, subFolder, "User_" + command.UserID);
            if (!Directory.Exists(storeFileDirectory))
            {
                Directory.CreateDirectory(storeFileDirectory);
            }
            foreach (var file in Directory.GetFiles(storeFileDirectory))
            {
                System.IO.File.Delete(file);
            }

            string safeFileName = Path.GetFileName(command.Image.FileName);
            string route = Path.Combine(storeFileDirectory, safeFileName);
            using (var stream = new FileStream(route, FileMode.Create))
            {
                await command.Image.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}
