using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Create
{
    public class CreatePostCommand : IRequest<int>
    {
        public int AnimalID { get; set; }
        public int CityID { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
    }
}
