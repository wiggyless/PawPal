using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cantons.Lists
{
    public sealed class ListCantonsQueryDto
    {
        public required int Id { get; set; }
        public required string FullName { get; set; }  
        public required List<ListCantonsQueryCitiesDto> Cities { get; set; }
    }
    public sealed class ListCantonsQueryCitiesDto
    {
        public required int Id { get; set; }
        public  required string Name { get; set; }
    }
}
