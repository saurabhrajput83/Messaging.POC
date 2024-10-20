using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics.Interfaces
{
    public interface IReceiver
    {
        void Preprocessing();
        void Run();
        void Postprocessing();
    }
}
