﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Socket_Projet_Server.Models;

#nullable disable

namespace Socket_Projet_Server.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Socket_Projet_Server.Models.Contact", b =>
                {
                    b.Property<int>("UtilisateurId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("ContactUserId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("UtilisateurId", "ContactUserId");

                    b.HasIndex("ContactUserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Socket_Projet_Server.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("Audio")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Video")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Socket_Projet_Server.Models.Utilisateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Infos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Utilisateurs");
                });

            modelBuilder.Entity("Socket_Projet_Server.Models.Contact", b =>
                {
                    b.HasOne("Socket_Projet_Server.Models.Utilisateur", "ContactUser")
                        .WithMany()
                        .HasForeignKey("ContactUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Socket_Projet_Server.Models.Utilisateur", "Utilisateur")
                        .WithMany("Contacts")
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ContactUser");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("Socket_Projet_Server.Models.Message", b =>
                {
                    b.HasOne("Socket_Projet_Server.Models.Utilisateur", "Receiver")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Socket_Projet_Server.Models.Utilisateur", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Socket_Projet_Server.Models.Utilisateur", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");
                });
#pragma warning restore 612, 618
        }
    }
}
