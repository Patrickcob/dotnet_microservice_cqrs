using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Post.CMD.Domain.Entities
{
    [Table("Comment", Schema = "dbo")]
    public class CommentEntity
    {
        [Key]
        public Guid CommentId { get; set; }

        public string Username { get; set; }

        public DateTime CommentDate { get; set; }

        public string Comment {get;set;}

        public bool Edited {get;set;}

        public Guid PostId {get;set;}

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}