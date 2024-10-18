using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIBCO.RV.Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Receiver().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }

        }
    }
}
