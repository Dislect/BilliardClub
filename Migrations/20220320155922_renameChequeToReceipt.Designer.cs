// <auto-generated />
using System;
using BilliardClub.App_Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BilliardClub.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220320155922_renameChequeToReceipt")]
    partial class renameChequeToReceipt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BilliardClub.Models.CartFoodItem", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FoodItemid")
                        .HasColumnType("int");

                    b.Property<string>("cartId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("quantity")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("FoodItemid");

                    b.ToTable("CartFoodItems");
                });

            modelBuilder.Entity("BilliardClub.Models.CartPoolTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PoolTableid")
                        .HasColumnType("int");

                    b.Property<string>("cartId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("numberHours")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("reservationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("PoolTableid");

                    b.ToTable("CartPoolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.FoodItem", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("picturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("price")
                        .HasColumnType("bigint");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("BilliardClub.Models.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("orderDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("receipt")
                        .HasColumnType("float");

                    b.Property<string>("userId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BilliardClub.Models.OrderFoodItem", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("foodItemid")
                        .HasColumnType("int");

                    b.Property<int?>("orderid")
                        .HasColumnType("int");

                    b.Property<long>("quantity")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("foodItemid");

                    b.HasIndex("orderid");

                    b.ToTable("OrderFoodItems");
                });

            modelBuilder.Entity("BilliardClub.Models.OrderPoolTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("numberHours")
                        .HasColumnType("bigint");

                    b.Property<int?>("orderid")
                        .HasColumnType("int");

                    b.Property<int?>("poolTableid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("orderid");

                    b.HasIndex("poolTableid");

                    b.ToTable("OrderPoolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.PoolTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("tableRotationid")
                        .HasColumnType("int");

                    b.Property<int>("tableX")
                        .HasColumnType("int");

                    b.Property<int>("tableY")
                        .HasColumnType("int");

                    b.Property<int?>("typeTableid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("tableRotationid");

                    b.HasIndex("typeTableid");

                    b.ToTable("PoolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.Status", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("BilliardClub.Models.StatusTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("dateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dateStart")
                        .HasColumnType("datetime2");

                    b.Property<int?>("poolTableid")
                        .HasColumnType("int");

                    b.Property<int?>("statusid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("poolTableid");

                    b.HasIndex("statusid");

                    b.ToTable("StatusTables");
                });

            modelBuilder.Entity("BilliardClub.Models.TableRotation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("rotationAngle")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("TableRotations");
                });

            modelBuilder.Entity("BilliardClub.Models.TypeTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("price")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("TypeTables");
                });

            modelBuilder.Entity("BilliardClub.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("firstName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("lastName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BilliardClub.Models.CartFoodItem", b =>
                {
                    b.HasOne("BilliardClub.Models.FoodItem", "FoodItem")
                        .WithMany()
                        .HasForeignKey("FoodItemid");

                    b.Navigation("FoodItem");
                });

            modelBuilder.Entity("BilliardClub.Models.CartPoolTable", b =>
                {
                    b.HasOne("BilliardClub.Models.PoolTable", "PoolTable")
                        .WithMany()
                        .HasForeignKey("PoolTableid");

                    b.Navigation("PoolTable");
                });

            modelBuilder.Entity("BilliardClub.Models.Order", b =>
                {
                    b.HasOne("BilliardClub.Models.User", "user")
                        .WithMany("orders")
                        .HasForeignKey("userId");

                    b.Navigation("user");
                });

            modelBuilder.Entity("BilliardClub.Models.OrderFoodItem", b =>
                {
                    b.HasOne("BilliardClub.Models.FoodItem", "foodItem")
                        .WithMany("orders")
                        .HasForeignKey("foodItemid");

                    b.HasOne("BilliardClub.Models.Order", "order")
                        .WithMany("foodItems")
                        .HasForeignKey("orderid");

                    b.Navigation("foodItem");

                    b.Navigation("order");
                });

            modelBuilder.Entity("BilliardClub.Models.OrderPoolTable", b =>
                {
                    b.HasOne("BilliardClub.Models.Order", "order")
                        .WithMany("poolTables")
                        .HasForeignKey("orderid");

                    b.HasOne("BilliardClub.Models.PoolTable", "poolTable")
                        .WithMany("orders")
                        .HasForeignKey("poolTableid");

                    b.Navigation("order");

                    b.Navigation("poolTable");
                });

            modelBuilder.Entity("BilliardClub.Models.PoolTable", b =>
                {
                    b.HasOne("BilliardClub.Models.TableRotation", "tableRotation")
                        .WithMany("poolTables")
                        .HasForeignKey("tableRotationid");

                    b.HasOne("BilliardClub.Models.TypeTable", "typeTable")
                        .WithMany("poolTables")
                        .HasForeignKey("typeTableid");

                    b.Navigation("tableRotation");

                    b.Navigation("typeTable");
                });

            modelBuilder.Entity("BilliardClub.Models.StatusTable", b =>
                {
                    b.HasOne("BilliardClub.Models.PoolTable", "poolTable")
                        .WithMany("statusTables")
                        .HasForeignKey("poolTableid");

                    b.HasOne("BilliardClub.Models.Status", "status")
                        .WithMany()
                        .HasForeignKey("statusid");

                    b.Navigation("poolTable");

                    b.Navigation("status");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BilliardClub.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BilliardClub.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BilliardClub.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BilliardClub.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BilliardClub.Models.FoodItem", b =>
                {
                    b.Navigation("orders");
                });

            modelBuilder.Entity("BilliardClub.Models.Order", b =>
                {
                    b.Navigation("foodItems");

                    b.Navigation("poolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.PoolTable", b =>
                {
                    b.Navigation("orders");

                    b.Navigation("statusTables");
                });

            modelBuilder.Entity("BilliardClub.Models.TableRotation", b =>
                {
                    b.Navigation("poolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.TypeTable", b =>
                {
                    b.Navigation("poolTables");
                });

            modelBuilder.Entity("BilliardClub.Models.User", b =>
                {
                    b.Navigation("orders");
                });
#pragma warning restore 612, 618
        }
    }
}
