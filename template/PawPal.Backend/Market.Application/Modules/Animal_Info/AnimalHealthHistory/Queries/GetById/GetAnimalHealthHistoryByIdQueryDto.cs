using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.GetById
{
    public class GetAnimalHealthHistoryByIdQueryDto
    {
        public int AnimalHealthHistoryId { get; set; }
        public string? AnimalName { get; set; }
        public string? AnimalCategory { get; set; }
        public bool Vaccinated { get; set; }
        public bool SpayedOrNeutered { get; set; }
        public bool ParasiteFree { get; set; }
        public string? DietaryRestrictions { get; set; }
        public List<GetAllergyFromAnimalDto> AnimalAllergies { get; set; } = new List<GetAllergyFromAnimalDto>();
        public List<GetDisabilityFromAnimalDto> AnimalDisabilities { get; set; } = new List<GetDisabilityFromAnimalDto>();
    }
    public class GetAllergyFromAnimalDto
    {
        public string AllergyName { get; set; }
        public string AllergyDescription { get; set; }
    }

    public class GetDisabilityFromAnimalDto
    {
        public string DisabilityName { get; set; }
        public string DisabilityDescription { get; set; }
    }
    }
