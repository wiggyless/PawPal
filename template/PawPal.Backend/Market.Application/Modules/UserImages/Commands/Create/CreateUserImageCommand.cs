using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Create
{
    public class CreateUserImageCommand : IRequest<int>
    {
        public int UserID { get; set; }
        public IFormFile Image { get; set; }
    }

}
