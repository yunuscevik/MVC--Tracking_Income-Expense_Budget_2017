﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TIEB_BudgetDBEntities : DbContext
    {
        public TIEB_BudgetDBEntities()
            : base("name=TIEB_BudgetDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BankCartType> BankCartType { get; set; }
        public virtual DbSet<CreditCartType> CreditCartType { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<Income> Income { get; set; }
        public virtual DbSet<IncomeType> IncomeType { get; set; }
        public virtual DbSet<Persons> Persons { get; set; }
    }
}
