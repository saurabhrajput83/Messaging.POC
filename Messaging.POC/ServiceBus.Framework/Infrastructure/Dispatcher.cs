namespace ServiceBus.Framework.Infrastructure
{
    public class Dispatcher
    {

        private Queue _queue = null;


        public Dispatcher(Queue queue)
        {
            _queue = queue;
        }

        public void Join()
        {

        }
    }
}
