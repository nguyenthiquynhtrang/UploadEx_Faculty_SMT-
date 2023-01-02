﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NguyenThiQuynhTrangBTH2.Data;

#nullable disable

namespace NguyenThiQuynhTrangBTH2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221116140459_Create_Table_Customer")]
    partial class CreateTableCustomer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Customer", b =>
                {
                    b.Property<string>("CustomerID")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Employee", b =>
                {
                    b.Property<string>("EmployeeID")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Faculty", b =>
                {
                    b.Property<string>("FacultyID")
                        .HasColumnType("TEXT");

                    b.Property<string>("FacultyName")
                        .HasColumnType("TEXT");

                    b.HasKey("FacultyID");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Person", b =>
                {
                    b.Property<string>("PersonID")
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonName")
                        .HasColumnType("TEXT");

                    b.HasKey("PersonID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Student", b =>
                {
                    b.Property<string>("StudentID")
                        .HasColumnType("TEXT");

                    b.Property<string>("FacultyID")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentName")
                        .HasColumnType("TEXT");

                    b.HasKey("StudentID");

                    b.HasIndex("FacultyID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("NguyenThiQuynhTrangBTH2.Models.Student", b =>
                {
                    b.HasOne("NguyenThiQuynhTrangBTH2.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyID");

                    b.Navigation("Faculty");
                });
#pragma warning restore 612, 618
        }
    }
}
