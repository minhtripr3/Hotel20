﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel20.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HotelDB10Entities1 : DbContext
    {
        public HotelDB10Entities1()
            : base("name=HotelDB10Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<BookSerVice> BookSerVices { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<OrderRoom> OrderRooms { get; set; }
        public virtual DbSet<PhuThu> PhuThus { get; set; }
        public virtual DbSet<Quanly> Quanlies { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<SerVice> SerVices { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Thue> Thues { get; set; }
    }
}