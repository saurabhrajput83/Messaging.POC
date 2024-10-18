using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIBCO.RV.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Publisher().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
