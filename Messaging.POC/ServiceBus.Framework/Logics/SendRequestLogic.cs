using Azure.Core;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Logics
{
    public class SendRequestLogic
    {
        private List<string> _requests;
        private Dictionary<string, Message> _responseMessages;

        public List<string> Requests => _requests;

        public SendRequestLogic()
        {

            _requests = new List<string>();
            _responseMessages = new Dictionary<string, Message>();
        }

        public string CreateNewRequest()
        {
            string requestId = Helper.GetInboxName();
            _requests.Add(requestId);

            return requestId;
        }

        public void AddResponseMessages(string requestId, Message responseMsg)
        {
            if (!_responseMessages.ContainsKey(requestId))
                _responseMessages.Add(requestId, responseMsg);
            else
                _responseMessages[requestId] = responseMsg;
        }

        public async Task<Message> GetResponse(string requestId, double timeout)
        {

            return await Task.Run(() =>
            {
                return GetResponseMessage(requestId, timeout);
            });


        }

        private Message GetResponseMessage(string requestId, double timeout)
        {
            Message responseMsg = null;
            Stopwatch s = new Stopwatch();
            s.Start();

            while (s.Elapsed <= TimeSpan.FromMilliseconds(timeout))
            {
                if (_responseMessages.ContainsKey(requestId))
                {
                    responseMsg = _responseMessages[requestId];
                    break;
                }
            }

            s.Stop();

            return responseMsg;
        }
    }
}
