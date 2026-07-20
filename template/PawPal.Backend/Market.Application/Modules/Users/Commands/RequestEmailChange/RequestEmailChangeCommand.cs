namespace PawPal.Application.Modules.Users.Commands.RequestEmailChange
{
    public class RequestEmailChangeCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string NewEmail { get; set; }
    }
}
