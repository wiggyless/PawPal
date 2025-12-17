using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info
{
    public class BreedEntity : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryID { get; set; }

        public AnimalCategoriesEntity Category { get; set; }
    }
}
