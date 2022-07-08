using Jay.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string PostTitle { get; set; }
        public string PostText { get; set; }
        public string MediaUrl { get; set; }
        public int MediaType { get; set; }

        #region relations
        public int UserId { get; set; }

        //navigation prop
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        #endregion
    }
}
