using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Update
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Breed { get; set; }
        public string PhotoURL { get; set; }
        // will add more attributes
    }
}
