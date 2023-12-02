using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.MessageBus
{
    public interface IMessageBus
    {
        public Task PublishMessage(object message, string topic_queue_Name);
    }
}
