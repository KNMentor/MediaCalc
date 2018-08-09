using MediaCalc.L.Data;
using MediaCalc.L.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCalc.L.Logic
{
    public class Logic
    {
        public MediaCalcDbContext dbContext { get; set; } = new MediaCalcDbContext();

        public List<User> Users { get; set; }
        public List<Flat> Flats { get; set; }
        public List<Lease> Leases { get; set; }
        public List<ConstFees> ConstFees { get; set; }

        public Logic()
        {

        }

        public int CountUsersInFlat(Flat flat, DateTime Day)
        {
            List<Lease> leasesInDay = dbContext.Leases.Where(x => x.FlatsId == flat.Id).Where(x => x.To < Day).Where(x => x.From > Day).ToList<Lease>();
            var unique_items = new HashSet<Lease>(leasesInDay);
            return unique_items.Count();
        }
        public int CountUsersInFlat(int flatId, DateTime Day)
        {
            List<Lease> leasesInDay = dbContext.Leases.Where(x => (x.FlatsId == flatId) && 
                (DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(Day))  &&
                (DbFunctions.TruncateTime(x.To) >= DbFunctions.TruncateTime(Day))).ToList<Lease>();
            var unique_items = new HashSet<Lease>(leasesInDay);
            return unique_items.Count();
        }
        
        public ConstFees CurrentConstFeeForFlat(int flatId, DateTime Day)
        {
            return dbContext.ConstFees.Where(x => (x.FlatId == flatId) &&
                (DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(Day)) &&
                (DbFunctions.TruncateTime(x.To) >= DbFunctions.TruncateTime(Day))).FirstOrDefault();
        }
        public ConstFees CurrentConstFeeForFlat(int flatId)
        {
            return dbContext.ConstFees.Where(x => (x.FlatId == flatId) &&
                (DbFunctions.TruncateTime(x.From) == DbFunctions.TruncateTime(DateTime.Now))).FirstOrDefault();
        }

        public double TotalConstMoneyForOneLease(int flatId, DateTime From, DateTime To)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(flatId, To);
            DateTime currentDay = From;
            int TotalDays = (Int32)(To - From).TotalDays;

            double SumForUser = 0;

            for(int i = 0; i <= TotalDays; i++)
            {
                double a = constFeesForFlat.SumOfConst();
                double b = (double)(DateTime.DaysInMonth(currentDay.Year, currentDay.Month));
                double c = CountUsersInFlat(flatId, currentDay);
                SumForUser = SumForUser + (a /(b*c));
                currentDay = currentDay.AddDays(1);
            }

            return Math.Round(SumForUser,2);
        }
        public double TotalConstMoneyForOneLease(Lease lease)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(lease.FlatsId, lease.To);
            DateTime currentDay = lease.From;
            int TotalDays = (Int32)(lease.To - lease.From).TotalDays;

            double SumForUser = 0;

            for (int i = 0; i <= TotalDays; i++)
            {
                SumForUser = SumForUser + (constFeesForFlat.SumOfConst() / 
                    ((double)(DateTime.DaysInMonth(currentDay.Year, currentDay.Month)) * 
                    CountUsersInFlat(lease.FlatsId, currentDay)));
                currentDay = currentDay.AddDays(1);
            }
            return Math.Round(SumForUser, 2);
        }
        public double TotalConstMoneyForOneLease(Lease lease, DateTime From, DateTime To)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(lease.FlatsId, lease.To);
            DateTime newFrom;
            DateTime newTo;
            int TotalDays;
            double SumForUser = 0;

            if (lease.To <= To)
                newTo = lease.To;
            else
                newTo = To;

            if (lease.From >= From)
                newFrom = lease.From;
            else
                newFrom = From;

            TotalDays = (Int32)(newTo - newFrom).TotalDays;

            for (int i = 0; i <= TotalDays; i++)
            {
                SumForUser = SumForUser + (constFeesForFlat.SumOfConst() /
                    ((double)(DateTime.DaysInMonth(newFrom.Year, newFrom.Month)) *
                    CountUsersInFlat(lease.FlatsId, newFrom)));
                newFrom = newFrom.AddDays(1);
            }

            return Math.Round(SumForUser, 2);
        }

        public double TotalConstMoneyForOneUserStartedIn(User user, DateTime From, DateTime To)
        {
            double sum = 0;
            List<Lease> leasesForThisUser = dbContext.Leases.Where
                (x => (x.UserId == user.Id) &&
                ((DbFunctions.TruncateTime(From) <= DbFunctions.TruncateTime(x.From) && DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(To)) ||
                    (DbFunctions.TruncateTime(From) <= DbFunctions.TruncateTime(x.To) && DbFunctions.TruncateTime(x.To) <= DbFunctions.TruncateTime(To)))).ToList<Lease>();

            foreach(Lease lease in leasesForThisUser)
                sum = sum + TotalConstMoneyForOneLease(lease, From, To);

            return Math.Round(sum, 2);
        }

    }
}
