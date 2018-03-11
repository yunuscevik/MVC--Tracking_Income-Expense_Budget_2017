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

    public partial class Persons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persons()
        {
            this.BankCartType = new HashSet<BankCartType>();
            this.CreditCartType = new HashSet<CreditCartType>();
            this.Expense = new HashSet<Expense>();
            this.Income = new HashSet<Income>();
        }
    
        public int PersonID { get; set; }
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Display(Name = "Soyad")]
        public string SurName { get; set; }
        [Display(Name = "Kullan�c� Ad�")]
        public string UserName { get; set; }
        [Display(Name = "�ifre")]
        public string Password { get; set; }
        [Display(Name = "Mail Adresi")]
        public string Email { get; set; }
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }
        [Display(Name = "Do�um Tarihi")]
        public System.DateTime BrithDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankCartType> BankCartType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditCartType> CreditCartType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expense { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income> Income { get; set; }
    }
}
