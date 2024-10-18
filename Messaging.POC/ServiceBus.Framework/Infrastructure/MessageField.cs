using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class MessageField
    {
        private string _fieldName = string.Empty;
        private object _fieldValue;

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }

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


        public MessageField()
        { }

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
