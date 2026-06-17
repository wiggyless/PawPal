using PawPal.Application.Modules.Messaging.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Messaging.Queries.GetConversations
{
    public class GetConversationsQuery : IRequest<List<ConversationDto>>
    {
        public int UserId { get; set; }
    }
}
