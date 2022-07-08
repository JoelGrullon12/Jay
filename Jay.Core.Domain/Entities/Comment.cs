using Jay.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Text { get; set; }

        #region relations
        public int UserId { get; set; }
        public int PostId { get; set; }

        //navigation prop
        public User User { get; set; }
        public Post Posts { get; set; }
        #endregion
    }
}
