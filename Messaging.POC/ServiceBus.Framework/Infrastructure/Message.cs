using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Message
    {
        public string _sendSubject = string.Empty;
        public string _replySubject = string.Empty;

        public string SendSubject
        {
            get
            {
                return _sendSubject;
            }
            set
            {
                _sendSubject = value;
            }
        }

        public string ReplySubject
        {
            get
            {
                return _replySubject;
            }
            set
            {
                _replySubject = value;
            }
        }

        public void AddField(MessageField messageField)
        {
        }

        public MessageField GetField(string fieldName)
        {
            return new MessageField("", 0);
        }
    }
}
