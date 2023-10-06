using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PetShopOnline.Models
{
    public partial class DTB_PETSHOPContext : DbContext
    {
        public DTB_PETSHOPContext()
        {
        }

        public DTB_PETSHOPContext(DbContextOptions<DTB_PETSHOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<BlogDetail> BlogDetails { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Gallery> Galleries { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<PictureGallery> PictureGalleries { get; set; } = null!;
        public virtual DbSet<PictureProduct1> PictureProducts1 { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ReasonCancel> ReasonCancels { get; set; } = null!;
        public virtual DbSet<Ship> Ships { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = DTB_PETSHOP;uid=sa;pwd=asd123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(10)
                    .HasColumnName("EmployeeID")
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Accounts_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Accounts_Employees");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_Accounts_Shipper");
            });

            modelBuilder.Entity<BlogDetail>(entity =>
            {
                entity.HasKey(e => e.BlogId);

                entity.Property(e => e.BlogId).HasColumnName("BlogID");

                entity.Property(e => e.FeaturedImageUrl)
                    .HasMaxLength(1000)
                    .HasColumnName("FeaturedImageURL");

                entity.Property(e => e.Heading).HasMaxLength(1000);

                entity.Property(e => e.PageTitle).HasMaxLength(1000);

                entity.Property(e => e.PublishedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Picture)
                    .HasMaxLength(200)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.DepartmentType).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(10)
                    .HasColumnName("EmployeeID")
                    .IsFixedLength();

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.FirstName).HasMaxLength(10);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employees_Departments");
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("Gallery");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(10)
                    .HasColumnName("EmployeeID")
                    .IsFixedLength();

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderStatus).HasMaxLength(20);

                entity.Property(e => e.ShippedDate).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("Order Details");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order Details_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order Details_Products");
            });

            modelBuilder.Entity<PictureGallery>(entity =>
            {
                entity.HasKey(e => e.PicId);

                entity.ToTable("PictureGallery");

                entity.Property(e => e.PicId).ValueGeneratedNever();

                entity.Property(e => e.Caption).HasMaxLength(50);

                entity.Property(e => e.Picture)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.PictureGalleries)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_PictureGallery_Gallery");
            });

            modelBuilder.Entity<PictureProduct1>(entity =>
            {
                entity.HasKey(e => e.PictureId);

                entity.ToTable("PictureProducts");

                entity.Property(e => e.Picture)
                    .HasMaxLength(200)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.Property(e => e.UpdateDate).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasMany(d => d.Pictures)
                    .WithMany(p => p.Products)
                    .UsingEntity<Dictionary<string, object>>(
                        "PictureProduct",
                        l => l.HasOne<PictureProduct1>().WithMany().HasForeignKey("PictureId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Picture_Products_PictureProducts"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Picture_Products_Products"),
                        j =>
                        {
                            j.HasKey("ProductId", "PictureId");

                            j.ToTable("Picture_Products");
                        });
            });

            modelBuilder.Entity<ReasonCancel>(entity =>
            {
                entity.HasKey(e => e.ReasonId);

                entity.ToTable("Reason_Cancel");

                entity.Property(e => e.ReasonId).ValueGeneratedNever();

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ReasonCancels)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Reason_Cancel_Orders");
            });

            modelBuilder.Entity<Ship>(entity =>
            {
                entity.ToTable("Ship");

                entity.Property(e => e.Freight).HasColumnType("money");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PhoneContact)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShipContact).HasMaxLength(30);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Ships)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Ship_Orders");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Ships)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_Ship_Shipper");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.District).HasMaxLength(60);

                entity.Property(e => e.Name).HasMaxLength(60);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Province).HasMaxLength(60);

                entity.Property(e => e.Wards).HasMaxLength(60);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
