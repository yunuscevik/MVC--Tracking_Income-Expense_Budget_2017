//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TIEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Income
    {
        public int IncomeID { get; set; }
        public int PersonID { get; set; }
        public int IncomeType { get; set; }
        [Display(Name = "Miktar")]
        public decimal Amount { get; set; }
        [Display(Name = "Tarih")]
        public System.DateTime Date { get; set; }
        [Display(Name = "Di�er Gelir T�r�")]
        public string Explanation { get; set; }
    
        public virtual IncomeType IncomeType1 { get; set; }
        public virtual Persons Persons { get; set; }
    }
}