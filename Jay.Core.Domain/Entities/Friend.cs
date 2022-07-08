using Jay.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Domain.Entities
{
    public class Friend : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public User JUser { get; set; }
        public User JFriend { get; set; }
    }
}
