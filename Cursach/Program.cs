using System;
using Cursach;
using System.Collections.Generic;
using Cursach.Client;
using Cursach.Model;

namespace Cursach
{
    class Program
    {
        static void Main(string[] args)
        {
            DotaInfoBot dotaInfo = new DotaInfoBot();
            dotaInfo.Start();
            Console.ReadKey();
        }
    }
}
