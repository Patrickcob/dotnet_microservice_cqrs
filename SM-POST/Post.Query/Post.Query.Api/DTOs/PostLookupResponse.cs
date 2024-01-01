using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.CMD.Domain.Entities;
using Post.Common.DTOs;

namespace Post.Query.Api.DTOs
{
    public class PostLookupResponse : BaseResponse
    {
        public List<PostEntity> Posts {get;set;}
    }
}