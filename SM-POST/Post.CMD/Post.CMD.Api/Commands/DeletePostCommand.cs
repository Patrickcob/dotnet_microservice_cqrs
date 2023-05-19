using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.CORE.Commands;

namespace Post.CMD.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username {get;set;} 
    }
}