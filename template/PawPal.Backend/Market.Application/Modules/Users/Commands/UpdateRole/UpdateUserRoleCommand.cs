namespace PawPal.Application.Modules.Users.Commands.UpdateRole
{
    public class UpdateUserRoleCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
