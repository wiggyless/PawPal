using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info.ManyToMany
{
    public class AnimalCharacteristics : BaseEntity
    {
        public int AnimalId { get; set; }
        public AnimalEntity Animal { get; set; }
        public int CharacteristicId { get; set; }
        public CharacteristicsEntity Characteristic { get; set; }
    }
}
