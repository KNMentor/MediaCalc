using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCalc.L.Model
{
    public class Lease
    {
        [Key]
        [DisplayName("hidden-Id")]
        public int Id { get; set; }


        [ForeignKey(nameof(Flat))]
        [DisplayName("hidden-FlatsId")]
        public int FlatsId { get; set; }
        [DisplayName("Nieruchomość")]
        public virtual Flat Flat { get; set; }


        [DisplayName("hidden-Id Użytkownika")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [DisplayName("Użytkownik")]
        public virtual User User { get; set; }



        [DisplayName("Data Od")]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DisplayName("Data Do")]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        #region Variable fees for a period
        [DisplayName("Licznik CW początek")]
        public double CounterStartHotWater { get; set; }
        [DisplayName("Licznik ZW początek")]
        public double CounterStartColdWater { get; set; }
        [DisplayName("Licznik EW początek")]
        public double CounterStartEnergy { get; set; }
        [DisplayName("Licznik G początek")]
        public double CounterStartGas { get; set; }

        [DisplayName("hidden-CounterMiddleHotWater")]
        public double CounterMiddleHotWater { get; set; }
        [DisplayName("hidden-CounterMiddleColdWater")]
        public double CounterMiddleColdWater { get; set; }
        [DisplayName("hidden-CounterMiddleEnergy")]
        public double CounterMiddleEnergy { get; set; }
        [DisplayName("hidden-CounterMiddleGas")]
        public double CounterMiddleGas { get; set; }


        [DisplayName("Licznik CW koniec")]
        public double CounterEndHotWater { get; set; }
        [DisplayName("Licznik ZW koniec")]
        public double CounterEndColdWater { get; set; }
        [DisplayName("Licznik E koniec")]
        public double CounterEndEnergy { get; set; }
        [DisplayName("Licznik G koniec")]
        public double CounterEndGas { get; set; }
        #endregion


        [DisplayName("hidden-DeltaHotWater")]
        public double DeltaHotWaterForThisUser { get; set; }
        [DisplayName("hidden-DeltaColdWater")]
        public double DeltaColdWaterForThisUser { get; set; }
        [DisplayName("hidden-DeltaEnergyWater")]
        public double DeltaEnergyForThisUser { get; set; }
        [DisplayName("hidden-DeltaGas")]
        public double DeltaGasForThisUser { get; set; }


        [DisplayName("Suma mediów zmiennych")]
        public double VariablesSum { get; set; }
        [DisplayName("Suma mediów stałych")]
        public double ConstSum { get; set; }

        [DisplayName("Opłacone")]
        public bool Paid { get; set; }

        public Lease ShallowCopy()
        {
            return (Lease)this.MemberwiseClone();
        }


    }
}
