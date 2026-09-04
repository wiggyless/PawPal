using PawPal.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Update
{
    public class UpdateUserImageCommand : IRequest<Unit>
    {
        public FileUpload Image { get; set; }
    }
}
