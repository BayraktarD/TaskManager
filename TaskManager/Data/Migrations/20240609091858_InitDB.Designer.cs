﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManager.Data;

#nullable disable

namespace TaskManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240609091858_InitDB")]
    partial class InitDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

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

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

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

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.AttachmentType", b =>
                {
                    b.Property<Guid>("IdAttachmentType")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdAttachmentType")
                        .HasName("PK__Attachme__9A39EABC3EF21011");

                    b.ToTable("AttachmentTypes");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Department", b =>
                {
                    b.Property<Guid>("IdDepartment")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Department1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Department");

                    b.HasKey("IdDepartment")
                        .HasName("PK__Departme__DF1E6E4BCB87D873");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Employee", b =>
                {
                    b.Property<Guid>("IdEmployee")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanAssignTasks")
                        .HasColumnType("bit");

                    b.Property<bool>("CanCreateProfiles")
                        .HasColumnType("bit");

                    b.Property<bool>("CanCreateTasks")
                        .HasColumnType("bit");

                    b.Property<bool>("CanDeleteProfiles")
                        .HasColumnType("bit");

                    b.Property<bool>("CanDeleteTasks")
                        .HasColumnType("bit");

                    b.Property<bool>("CanModifyProfiles")
                        .HasColumnType("bit");

                    b.Property<bool>("CanModifyTasks")
                        .HasColumnType("bit");

                    b.Property<Guid>("Department")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("JobTitle")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdEmployee")
                        .HasName("PK__tmp_ms_x__B7C926384DDF13EA");

                    b.HasIndex("Department");

                    b.HasIndex("JobTitle");

                    b.HasIndex(new[] { "UserId" }, "UniqueUserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.JobTitle", b =>
                {
                    b.Property<Guid>("IdJobTitle")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("JobTitle1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("JobTitle");

                    b.HasKey("IdJobTitle")
                        .HasName("PK__tmp_ms_x__A427B021DACEA8F8");

                    b.ToTable("JobTitles");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Task", b =>
                {
                    b.Property<Guid>("IdTask")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignedToId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("EditableEndDate")
                        .HasColumnType("bit");

                    b.Property<bool>("EditableStartDate")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("FinishedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("HasAttachments")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SolutionDetails")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("TaskDetails")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("TaskName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdTask")
                        .HasName("PK__tmp_ms_x__9FCAD1C5DEBBCF60");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ModifiedById");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.TaskAttachment", b =>
                {
                    b.Property<Guid>("IdAttachment")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdTask")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Attachment")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("AttachmentName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("IdAttachment", "IdTask")
                        .HasName("PK__tmp_ms_x__95730B08BC22CDBC");

                    b.HasIndex("IdTask");

                    b.ToTable("TaskAttachment", (string)null);
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
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
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

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Employee", b =>
                {
                    b.HasOne("TaskManager.Models.DBObjects.Department", "DepartmentNavigation")
                        .WithMany("Employees")
                        .HasForeignKey("Department")
                        .IsRequired()
                        .HasConstraintName("FK_Users_Departments");

                    b.HasOne("TaskManager.Models.DBObjects.JobTitle", "JobTitleNavigation")
                        .WithMany("Employees")
                        .HasForeignKey("JobTitle")
                        .IsRequired()
                        .HasConstraintName("FK_Users_JobTitles");

                    b.Navigation("DepartmentNavigation");

                    b.Navigation("JobTitleNavigation");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Task", b =>
                {
                    b.HasOne("TaskManager.Models.DBObjects.Employee", "AssignedTo")
                        .WithMany("TaskAssignedTos")
                        .HasForeignKey("AssignedToId")
                        .IsRequired()
                        .HasConstraintName("FK_Tasks_Users_AssignedTo");

                    b.HasOne("TaskManager.Models.DBObjects.Employee", "CreatedBy")
                        .WithMany("TaskCreatedBies")
                        .HasForeignKey("CreatedById")
                        .IsRequired()
                        .HasConstraintName("FK_Tasks_Users_CreatedBy");

                    b.HasOne("TaskManager.Models.DBObjects.Employee", "ModifiedBy")
                        .WithMany("TaskModifiedBies")
                        .HasForeignKey("ModifiedById")
                        .HasConstraintName("FK_Tasks_Users_ModifiedBy");

                    b.Navigation("AssignedTo");

                    b.Navigation("CreatedBy");

                    b.Navigation("ModifiedBy");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.TaskAttachment", b =>
                {
                    b.HasOne("TaskManager.Models.DBObjects.Task", "IdTaskNavigation")
                        .WithMany("TaskAttachments")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_TaskAttachment_Tasks");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Employee", b =>
                {
                    b.Navigation("TaskAssignedTos");

                    b.Navigation("TaskCreatedBies");

                    b.Navigation("TaskModifiedBies");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.JobTitle", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("TaskManager.Models.DBObjects.Task", b =>
                {
                    b.Navigation("TaskAttachments");
                });
#pragma warning restore 612, 618
        }
    }
}
