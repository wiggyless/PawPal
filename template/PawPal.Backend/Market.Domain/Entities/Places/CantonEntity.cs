using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Common;
using PawPal.Domain.Entities;
namespace PawPal.Domain.Entities.Places
{
    public class CantonEntity : BaseEntity
    {
        public required string FullName { get; set; }
        public required string Abbreviation { get; set; }
        public List<CitiesEntity> Cities { get; set; }
    }
}
