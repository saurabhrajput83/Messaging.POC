using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Message
    {
        private string _sendSubject = string.Empty;
        private string _replySubject = string.Empty;
        private List<MessageField> _fields = new List<MessageField>();

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

        public List<MessageField> Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }

        public Message()
        { }

        public void AddField(MessageField messageField)
        {
            _fields.Add(messageField);
        }

        public MessageField GetField(string fieldName)
        {

            MessageField field = _fields.FirstOrDefault(f => f.FieldName == fieldName);
            return field;
        }
    }
}
