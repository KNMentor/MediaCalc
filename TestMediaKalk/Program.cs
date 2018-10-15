using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaCalc.L.Logic;
using MediaCalc.L.Model;
using MediaCalc.L.Data;

namespace TestMediaKalk
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaCalcDbContext dbContext = new MediaCalcDbContext();
            Logic logic = new Logic();
            Lease lease = dbContext.Leases.FirstOrDefault(l => l.UserId == 3);

            Console.WriteLine(logic.TotalVarMoneyForOneLease(lease));
            Console.WriteLine(logic.TotalConstMoneyForOneLease(lease));


            lease = dbContext.Leases.FirstOrDefault(l => l.UserId == 1);

            Console.WriteLine(logic.TotalVarMoneyForOneLease(lease));
            Console.WriteLine(logic.TotalConstMoneyForOneLease(lease));


            Console.ReadKey();
        }
    }
}
