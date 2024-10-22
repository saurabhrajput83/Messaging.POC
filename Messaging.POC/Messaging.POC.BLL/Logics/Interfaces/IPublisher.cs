namespace Messaging.POC.BLL.Logics.Interfaces
{
    public interface IPublisher
    {
        void Preprocessing();
        void Run();
        void Postprocessing();
    }
}
