using PawPal.Domain.Common;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info
{
    public class CharacteristicsEntity : BaseEntity
    {
        public string CharacteristicName { get; set; }
        public List<AnimalCharacteristics> AnimalCharacteristics { get; set; }
    }
}
