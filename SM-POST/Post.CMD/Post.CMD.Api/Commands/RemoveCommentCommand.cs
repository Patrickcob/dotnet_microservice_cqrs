using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.CORE.Commands;

namespace Post.CMD.Api.Commands
{
    public class RemoveCommentCommand : BaseCommand
    {
        public Guid CommentId {get;set;}
        public string Username {get;set;}
    }
}