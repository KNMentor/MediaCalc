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
            Lease lease = dbContext.Leases.Where(l => l.User.Name == "Tomasz").FirstOrDefault<Lease>();

            //logic.TotalConstMoneyForOneLease(1, lease.From, lease.To);
            logic.TotalConstMoneyForOneUserStartedIn(dbContext.Users.Where(x => x.Id == 3).FirstOrDefault<User>(), new DateTime(2018, 1, 1), new DateTime(2018, 1, 6));


            Console.WriteLine(logic.CountUsersInFlat(1, new DateTime(2018, 1, 2)));


            Console.ReadKey();
        }
    }
}
