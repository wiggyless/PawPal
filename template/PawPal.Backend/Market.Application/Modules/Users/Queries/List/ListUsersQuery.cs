using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.List
{
    public sealed class ListUsersQuery : BasePagedQuery<ListUsersQueryDto>
    {
        public string? SearchFirstName { get; init; }
        public string? SearchLastName { get; init; }
        public string? SearchEmail { get; init; }
    }
}
