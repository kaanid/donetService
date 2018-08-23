using System;
using System.Collections.Generic;
using System.Text;

namespace Kaa.ServiceDemo.Service
{
    public class PrintService : IPrintService
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
