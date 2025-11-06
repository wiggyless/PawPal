using PawPal.Domain.Common;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info
{
    public class DisabilitiesEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        List<DisabilitiesAnimalHealthHistory> AnimalDisabilities { get; set; } = [];
    }
}
