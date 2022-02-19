using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Core.Event.Abstract
{
    public abstract class IEvent
    {
        public Guid RequestId { get; private init; }
        public DateTime InsertedDate { get; private init; }

        public IEvent()
        {
            RequestId = Guid.NewGuid();
            InsertedDate = DateTime.UtcNow;
        }
    }
}
