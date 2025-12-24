using PawPal.Application.Modules.Animal_Info.Animals.Commands.Delete;
using PawPal.Application.Modules.News.Commands.Create;
using PawPal.Application.Modules.News.Commands.Delete;
using PawPal.Application.Modules.News.Commands.Update;
using PawPal.Application.Modules.News.Queries.GetById;
using PawPal.Application.Modules.News.Queries.List;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PawPal.API.Controllers.News
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController(ISender sender, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IWebHostEnvironment environment = env;
        string subFolder = "news_photos";//we will store all news photos in this subfolder
        [HttpPost]
        public async Task<ActionResult<int>> CreateNews
            ([FromForm] CreateNewsCommand command, CancellationToken ct) //from form means we will be handling file uploads
        {
            if (command.Photo != null) //htjela sam da odvojim ovo u zaseban servis ali imam problema pa 
                //myb ovo fixam neki drugi put, za sad nek ostane ovako jer radi
            {
                var fileName = command.Photo.FileName; //what is the file's name

                var storeFileDirectory = Path.Combine(environment.WebRootPath, subFolder);
                //full path to the subfolder

                if (!Directory.Exists(storeFileDirectory))
                {
                    Directory.CreateDirectory(storeFileDirectory); //create the subfolder if it doesn't exist
                }

                string route = Path.Combine(storeFileDirectory, fileName); //full path to the FILE

                using (var ms = new MemoryStream()) //next few lines demonstrate how to save the uploaded file
                {
                    await command.Photo.CopyToAsync(ms, ct); //copy the uploaded file data to the memory stream
                    var content = ms.ToArray(); //convert the memory stream to a byte array
                    await System.IO.File.WriteAllBytesAsync(route, content, ct); //write the byte array to the specified file path
                }

                var fileLocation = Path.Combine(subFolder, fileName).Replace("\\", "/"); //final file location

                command.PhotoURL = fileLocation; //we set the photoURL property to this so we can store it in the database
            }
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetNewsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var news = await sender.Send(new GetNewsByIdQuery { Id = id }, ct);
            return news;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListNewsQueryDto>> List([FromQuery]
        ListNewsQuery query, CancellationToken ct)
        {
            var res = await sender.Send(query, ct);
            return res;
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteNewsCommand { Id = id }, ct);
        }

        //[HttpPut("{id:int}")]
        //public async Task Update(UpdateNewsCommand cmd, int id, CancellationToken ct)
        //{
        //    cmd.Id = id;
        //    if (cmd.Photo != null)
        //    {
        //        var fileName = cmd.Photo.FileName;
        //        var storeFileDirectory = Path.Combine(environment.WebRootPath, subFolder);
        //        string route = Path.Combine(storeFileDirectory, fileName);
        //    }
        //    await sender.Send(cmd, ct);
        //}
    }
}
