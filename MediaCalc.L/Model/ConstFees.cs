using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCalc.L.Model
{
    public class ConstFees
    {
        [Key]
        [DisplayName("hidden-Id")]
        public int Id { get; set; }


        [DisplayName("hidden-Id Nieruchomości")]
        [ForeignKey(nameof(Flat))]
        public int FlatId { get; set; }
        [DisplayName("Nieruchomość")]
        public virtual Flat Flat { get; set; }


        [DisplayName("Data Od")]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DisplayName("Data Do")]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        [DisplayName("Czynsz")]
        public double RentValue { get; set; }
        
        public double Internet { get; set; }
        
        public double Tv { get; set; }

        [DisplayName("Śmieci")]
        public double Garbage { get; set; }

        [DisplayName("Stawka za gaz")]
        public double GasValueForUnit { get; set; }
        
        [DisplayName("Stawka za ciepłą wodę")]
        public double HotWaterValueForUnit { get; set; }
        
        [DisplayName("Stawka za zimną wodę")]
        public double ColdWaterValueForUnit { get; set; }

        [DisplayName("Stawka za energię")]
        public double EnergyValueForUnit { get; set; }

        public double SumOfConst()
        {
            return RentValue + Internet + Tv + Garbage;
        }


        public ConstFees ShallowCopy()
        {
            return (ConstFees)this.MemberwiseClone();
        }

    }
}
