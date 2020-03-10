using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    [Serializable]
    class Message
    {
        public string Header { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public object Data { get; set; }

        public Message(string header, object data)
        {
            Header = header;
            Data = data;
        }
    }
}
