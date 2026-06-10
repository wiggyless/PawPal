namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.UpdateStatus;

public class UpdateAdoptionStatusCommand : IRequest
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
}