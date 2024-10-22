namespace ServiceBus.Framework.Infrastructure
{
    public sealed class Queue
    {

        private static Queue _default = null;

        private Queue()
        {

        }

        public static Queue Default
        {
            get
            {

                if (_default != null)
                {
                    _default = new Queue();
                }
                return _default;
            }
        }

    }
}
