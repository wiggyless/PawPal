using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Common;
using PawPal.Domain.Entities;
namespace PawPal.Domain.Entities.Places
{
    public class CitiesEntity : BaseEntity
    {
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public int? CantonId { get; set; }
        public CantonEntity? Canton { get; set; }
    }
}
