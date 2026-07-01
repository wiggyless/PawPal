using PawPal.Application.Modules.Users.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.ListDisabledAccounts
{
    public class ListDisabledAccountsQuery : BasePagedQuery<ListDisabledAccountsQueryDto>
    {
        public string? SearchFirstName { get; init; }
        public string? SearchLastName { get; init; }
        public string? SearchEmail { get; init; }
    }
}
