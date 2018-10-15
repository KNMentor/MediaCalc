using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCalc.L.Model
{
    public class User
    {
        [Key]
        [DisplayName("hidden-Id")]
        public int Id { get; set; }
        
        [DisplayName("Imię")]
        public string Name { get; set; }
        
        [DisplayName("Nazwisko")]
        public string Surname { get; set; }

        [DisplayName("Pesel")]
        public string Pesel { get; set; }

        [DisplayName("Numer Telefonu")]
        public string Phone { get; set; }

        [DisplayName("hidden-Leases")]
        public virtual List<Lease> Leases { get; set; }

        public override string ToString()
        {
            return this.Pesel + " | " + this.Name + " | " + this.Surname;
        }

        public User ShallowCopy()
        {
            return (User)this.MemberwiseClone();
        }
    }
}
