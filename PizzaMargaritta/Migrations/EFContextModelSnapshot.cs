﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PizzaMargaritta.Entities;

namespace PizzaMargaritta.Migrations
{
    [DbContext(typeof(EFContext))]
    partial class EFContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PizzaMargaritta.Entities.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("tblAdmins");
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.BasketPizza", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count_in");

                    b.Property<int>("Pizza_id");

                    b.Property<int>("User_id");

                    b.HasKey("id");

                    b.HasIndex("Pizza_id");

                    b.HasIndex("User_id");

                    b.ToTable("BasketPizza");
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("PizzaID");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("PizzaID");

                    b.HasIndex("UserID");

                    b.ToTable("tblOrders");
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.Pizza", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Image")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.ToTable("tblPizzas");
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsBanned");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("tblUsers");
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.BasketPizza", b =>
                {
                    b.HasOne("PizzaMargaritta.Entities.Pizza", "pizza")
                        .WithMany()
                        .HasForeignKey("Pizza_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PizzaMargaritta.Entities.User", "user")
                        .WithMany("BasketPizzas")
                        .HasForeignKey("User_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PizzaMargaritta.Entities.Order", b =>
                {
                    b.HasOne("PizzaMargaritta.Entities.Pizza", "Pizza")
                        .WithMany("Orders")
                        .HasForeignKey("PizzaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PizzaMargaritta.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
