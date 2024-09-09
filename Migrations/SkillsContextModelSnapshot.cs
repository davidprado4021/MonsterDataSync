﻿// <auto-generated />
using EFPractica01.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFPractica01.Migrations
{
    [DbContext(typeof(SkillsContext))]
    partial class SkillsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFPractica01.Models.Ranks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("SkillName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillsId");

                    b.ToTable("Ranks");
                });

            modelBuilder.Entity("EFPractica01.Models.Skills", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("EFPractica01.Models.Ranks", b =>
                {
                    b.HasOne("EFPractica01.Models.Skills", "Skills")
                        .WithMany("Ranks")
                        .HasForeignKey("SkillsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skills");
                });

            modelBuilder.Entity("EFPractica01.Models.Skills", b =>
                {
                    b.Navigation("Ranks");
                });
#pragma warning restore 612, 618
        }
    }
}
