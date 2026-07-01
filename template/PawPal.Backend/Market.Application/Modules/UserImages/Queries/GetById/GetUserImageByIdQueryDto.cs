
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Queries.GetById
{
    public class GetUserImageByIdQueryDto
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string PhotoURL { get; set; }
    }
}
