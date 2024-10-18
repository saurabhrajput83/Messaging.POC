using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class MessageField
    {
        public string _fieldName = string.Empty;
        public object _fieldValue;

        public object Value
        {
            get
            {
                return _fieldValue;
            }
            set
            {
                _fieldValue = value;
            }
        }

        public MessageField(string fieldName, string fieldValue)
        {
            _fieldName = fieldName;
            _fieldValue = fieldValue;
        }

        public MessageField(string fieldName, int fieldValue)
        {
            _fieldName = fieldName;
            _fieldValue = fieldValue;
        }
    }
}
