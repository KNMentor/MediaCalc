using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MediaCalc.L.Model
{
    public class Flat //: ModelBase//, IEditableObject
    {
        [Key]
        [DisplayName("hidden-Id")]
        public int Id { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Miasto")]
        public string City { get; set; }
        [DisplayName("Kod pocztowy")]
        public string PostCode { get; set; }
        
        [DisplayName("Ulica")]
        public string Street { get; set; }
        
        [DisplayName("Numer domu")]
        public string HomeNumber { get; set; }

        [DisplayName("Numer pokoju")]
        public string RoomNumber { get; set; }

        [DisplayName("Max mieszkańców")]
        public int MaxUsers { get; set; }

        [DisplayName("hidden-ConstFees")]
        public virtual List<ConstFees> ConstFees { get; set; }
        [DisplayName("hidden-Leases")]
        public virtual List<Lease> Leases { get; set; }


        public override string ToString()
        {
            return this.Name + " | " + this.Street + " | " + this.HomeNumber + " | " + this.RoomNumber;
        }

        public Flat ShallowCopy()
        {
            return (Flat)this.MemberwiseClone();
        }
    }
}
