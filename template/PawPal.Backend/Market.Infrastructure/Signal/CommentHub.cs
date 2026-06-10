using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PawPal.Application.Common;
using PawPal.Domain.Common;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Infrastructure.Signal
{
    [AllowAnonymous]
    public class CommentHub : Hub
    {
 
    }
}