﻿using Socket_Projet_Server.Models;
using Microsoft.EntityFrameworkCore;


namespace Socket_Projet_Server.Models
{
    public class MyContext : DbContext
    {

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-20AGV06\\SQLEXPRESS;initial catalog=SocketsProject;integrated security=True;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasKey(c => new { c.UtilisateurId, c.ContactUserId }); // Définir une clé composite

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ContactUser)
                .WithMany()
                .HasForeignKey(c => c.ContactUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }










    }
}