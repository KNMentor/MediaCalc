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


        /*
        public double TotalConstMoneyForOneLease(int flatId, DateTime From, DateTime To)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(flatId, To);
            if (constFeesForFlat == null)
                throw new SystemException("Nie ustawiono kosztów na to mieszkanie!");

            DateTime currentDay = From;
            int TotalDays = (Int32)(To - From).TotalDays;

            double sumForUser = 0;

            for(int i = 0; i <= TotalDays; i++)
            {
                double a = constFeesForFlat.SumOfConst();
                double b = (double)(DateTime.DaysInMonth(currentDay.Year, currentDay.Month));
                double c = CountUsersInFlat(flatId, currentDay);
                sumForUser = sumForUser + (a /(b*c));
                currentDay = currentDay.AddDays(1);
            }



            return Math.Round(sumForUser,2);
        }
        */
        public double TotalConstMoneyForOneLease(Lease lease)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(lease.FlatsId, lease.To);
            if (constFeesForFlat == null)
                throw new SystemException("Nie ustawiono kosztów na to mieszkanie!");

            DateTime currentDay = lease.From;
            int TotalDays = (Int32)(lease.To - lease.From).TotalDays;

            double sumForUser = 0;

            for (int i = 0; i <= TotalDays; i++)
            {
                sumForUser = sumForUser + (constFeesForFlat.SumOfConst() / 
                    ((double)(DateTime.DaysInMonth(currentDay.Year, currentDay.Month)) * 
                    CountUsersInFlat(lease.FlatsId, currentDay)));
                currentDay = currentDay.AddDays(1);
            }
            lease.ConstSum = Math.Round(sumForUser, 2);
            return lease.ConstSum;
        }
        public double TotalConstMoneyForOneLease(Lease lease, DateTime From, DateTime To)
        {
            ConstFees constFeesForFlat = CurrentConstFeeForFlat(lease.FlatsId, lease.To);
            if (constFeesForFlat == null)
                throw new SystemException("Nie ustawiono kosztów na to mieszkanie!");

            DateTime newFrom;
            DateTime newTo;
            int TotalDays;
            double sumForUser = 0;

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
                sumForUser = sumForUser + (constFeesForFlat.SumOfConst() /
                    ((double)(DateTime.DaysInMonth(newFrom.Year, newFrom.Month)) *
                    CountUsersInFlat(lease.FlatsId, newFrom)));
                newFrom = newFrom.AddDays(1);
            }

            lease.ConstSum = Math.Round(sumForUser, 2);
            return lease.ConstSum;
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

        public double TotalVarMoneyForOneLease(Lease lease)
        {
            double oldCold, oldHot, oldEnergy, oldGas;

            lease.CounterMiddleColdWater = lease.CounterStartColdWater;
            oldCold = lease.CounterStartColdWater;
            lease.CounterMiddleEnergy = lease.CounterStartEnergy;
            oldHot = lease.CounterStartEnergy;
            lease.CounterMiddleHotWater = lease.CounterStartHotWater;
            oldEnergy = lease.CounterStartHotWater;
            lease.CounterMiddleGas = lease.CounterStartGas;
            oldGas = lease.CounterStartGas;

            lease.DeltaColdWaterForThisUser = lease.DeltaEnergyForThisUser = lease.DeltaHotWaterForThisUser = lease.DeltaGasForThisUser = 0;


            ConstFees constFeesForFlat = CurrentConstFeeForFlat(lease.FlatsId, lease.To);
            if (constFeesForFlat == null)
                throw new SystemException("Nie ustawiono kosztów na to mieszkanie!");

            DateTime currentDay = lease.From;
            int TotalDays = (Int32)(lease.To - lease.From).TotalDays;
            
            int usersInFlatInThisDay = 0;
            int usersInFlatInPreviousDay = 0;

            for (int i = 0; i <= TotalDays; i++)
            {
                List<Lease> leasesInDay = dbContext.Leases.Where(x => (x.FlatsId == lease.FlatsId) &&
                 (DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(currentDay)) &&
                 (DbFunctions.TruncateTime(x.To) >= DbFunctions.TruncateTime(currentDay))).ToList<Lease>();

                var unique_items = new HashSet<Lease>(leasesInDay);

                foreach(Lease l in unique_items)
                {
                    if(i != 0 && l.From == currentDay)
                    {
                        lease.CounterMiddleColdWater = l.CounterStartColdWater;
                        lease.CounterMiddleEnergy = l.CounterStartEnergy;
                        lease.CounterMiddleHotWater = l.CounterStartHotWater;
                        lease.CounterMiddleGas = l.CounterStartGas;

                        lease.DeltaColdWaterForThisUser = lease.DeltaColdWaterForThisUser + ((lease.CounterMiddleColdWater - oldCold) / usersInFlatInPreviousDay);
                        lease.DeltaEnergyForThisUser = lease.DeltaEnergyForThisUser + ((lease.CounterMiddleEnergy - oldEnergy) / usersInFlatInPreviousDay);
                        lease.DeltaHotWaterForThisUser = lease.DeltaHotWaterForThisUser + ((lease.CounterMiddleHotWater - oldHot) / usersInFlatInPreviousDay);
                        lease.DeltaGasForThisUser = lease.DeltaGasForThisUser + ((lease.CounterMiddleGas - oldGas) / usersInFlatInPreviousDay);

                        oldCold = lease.CounterMiddleColdWater;
                        oldEnergy = lease.CounterMiddleEnergy;
                        oldHot = lease.CounterMiddleHotWater;
                        oldGas = lease.CounterMiddleGas;
                    }

                    if(l.To == currentDay)
                    {
                        usersInFlatInThisDay = CountUsersInFlat(lease.FlatsId, currentDay);

                        lease.CounterMiddleColdWater = l.CounterEndColdWater;
                        lease.CounterMiddleEnergy = l.CounterEndEnergy;
                        lease.CounterMiddleHotWater = l.CounterEndHotWater;
                        lease.CounterMiddleGas = l.CounterStartGas;

                        lease.DeltaColdWaterForThisUser = lease.DeltaColdWaterForThisUser + ((lease.CounterMiddleColdWater - oldCold) / usersInFlatInThisDay);
                        lease.DeltaEnergyForThisUser = lease.DeltaEnergyForThisUser + ((lease.CounterMiddleEnergy - oldEnergy) / usersInFlatInThisDay);
                        lease.DeltaHotWaterForThisUser = lease.DeltaHotWaterForThisUser + ((lease.CounterMiddleHotWater - oldHot) / usersInFlatInThisDay);
                        lease.DeltaGasForThisUser = lease.DeltaGasForThisUser + ((lease.CounterMiddleGas - oldGas) / usersInFlatInPreviousDay);

                        oldCold = lease.CounterMiddleColdWater;
                        oldEnergy = lease.CounterMiddleEnergy;
                        oldHot = lease.CounterMiddleHotWater;
                        oldGas = lease.CounterMiddleGas;
                    }
                }
                usersInFlatInPreviousDay = CountUsersInFlat(lease.FlatsId, currentDay);
                currentDay = currentDay.AddDays(1);
            }

            lease.VariablesSum = Math.Round((lease.DeltaColdWaterForThisUser * constFeesForFlat.ColdWaterValueForUnit +
                                lease.DeltaEnergyForThisUser * constFeesForFlat.EnergyValueForUnit +
                                lease.DeltaGasForThisUser * constFeesForFlat.GasValueForUnit +
                                lease.DeltaHotWaterForThisUser * constFeesForFlat.HotWaterValueForUnit),2);

            return lease.VariablesSum;
        }

    }
}
