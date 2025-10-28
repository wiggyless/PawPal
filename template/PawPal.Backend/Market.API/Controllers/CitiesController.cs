using PawPal.Application.Modules.Places.Queries.GetById;

// copy paste ahh pocetak ovog predmeta. Zbunjujuci utjecaj ovog templatea je
// prijekorno sirok, cak toliko sirok da nadmasuje moju sopstvenu mogucnost zbunjivanja
// samoga sebe. Moji dani su tek poceli ovde da pisem ove mrve od koda, kojeg ni
// prosijak ( junior u firmi) ne bi ni probao staviti na svoj jezik, jer njega cula
// zahtjevaju nesto vise, nesto sladje, a slasti ovde Bogami nema, niti ce biti
// Gorcina ovde ostaje i poslije ispiranja (odlaznja i ucenja neceg drugo i zanimljivijeg)
// Bog me gleda, a ja njega ne mogu, al bi samo da pitam: "Zasto" i poslije toga samo da 
// utonem u mrak, i postanem jedan sa njim, i svim materijama svijeta, gdje svi smo
// jedanki i gdje nema ovoga, gdje cemo pricati o ovome kao o strasnoj nocnoj mori
// Gladan sam, odo sta pojest,i da... noge mi smrde, a okupo sam se


namespace PawPal.API.Controllers
{
[ApiController]
[Route("[controller]")]
    public class CitiesController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<GetCityByIdQueryDto> GetById(int id,CancellationToken tk)
        {
            var category = await sender.Send(new GetCityByIdQuery { Id = id }, tk);
            return category;
        }
    }
}
