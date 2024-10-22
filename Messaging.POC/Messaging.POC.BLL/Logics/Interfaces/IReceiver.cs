namespace Messaging.POC.BLL.Logics.Interfaces
{
    public interface IReceiver
    {
        void Preprocessing();
        void Run();
        void Postprocessing();
    }
}
