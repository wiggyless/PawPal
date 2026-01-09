using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cities.Queries.Lists
{
    public sealed class ListCitiesQueryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}