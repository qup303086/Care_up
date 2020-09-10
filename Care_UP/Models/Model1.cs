namespace Care_UP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public virtual DbSet<Care_UP.Models.Members> Members { get; set; }
        public virtual DbSet<Care_UP.Models.Attendants> Attendants { get; set; }
        public virtual DbSet<Care_UP.Models.DetailedDates> DetailedDates { get; set; }
        public virtual DbSet<Care_UP.Models.CareRecords> CareRecords { get; set; }
        public virtual DbSet<Care_UP.Models.Elders> Elders { get; set; }
        public virtual DbSet<Care_UP.Models.Locations> Locations { get; set; }
        public virtual DbSet<Care_UP.Models.Cities> Cities { get; set; }
        public virtual DbSet<Care_UP.Models.Orders> Orders { get; set; }

        public virtual DbSet<Care_UP.Models.Pay> Pays { get; set; }
        //public virtual DbSet<Care_UP.Models.SpgatewayOutputDataModel> SpgatewayOutputDataModel { get; set; }
    }
}
