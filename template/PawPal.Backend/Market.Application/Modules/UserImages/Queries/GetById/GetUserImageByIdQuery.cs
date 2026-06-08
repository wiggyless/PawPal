using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Queries.GetById
{
    public class GetUserImageByIdQuery : IRequest<IActionResult>
    {
        public int UserID { get; set; } 
    }
}
