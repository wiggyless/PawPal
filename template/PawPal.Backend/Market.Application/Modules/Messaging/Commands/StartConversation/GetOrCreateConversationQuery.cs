using PawPal.Application.Modules.Messaging.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Messaging.Commands.StartConversation
{
    public class GetOrCreateConversationQuery : IRequest<ConversationDto>
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
    }
}
