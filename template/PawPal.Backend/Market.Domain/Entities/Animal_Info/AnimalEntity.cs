using PawPal.Domain.Common;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info
{
    public class AnimalEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set; }
        public GenderEntity Gender { get; set; }
        public bool HasPapers { get; set; }
        public bool ChildFriendly { get; set; }
        public int CategoryId { get; set; }
        public AnimalCategoriesEntity Category { get; set; }
        public List<AnimalCharacteristics> AnimalCharacteristics { get; set; }

    }
}
