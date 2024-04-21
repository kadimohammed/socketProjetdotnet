using Microsoft.EntityFrameworkCore;
using Socket_Projet_Server.Factory;
using Socket_Projet_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Repository
{
    public class UsersRepository
    {
        

        public static Utilisateur GetUserByTelephoneAndPassword(string telephone, string password)
        {
            Utilisateur utilisateur = null;

            using (var context = ContextFactory.getContext())
            {
                utilisateur = context.Utilisateurs
                .Include(u => u.Contacts)
                .ThenInclude(c => c.ContactUser)
                .Include(u => u.MessagesSent)
                .Include(u => u.MessagesReceived)
                .FirstOrDefault(u => u.Telephone == telephone && u.Password == password);

            }
            return utilisateur;
        }

        public static Utilisateur GetUserByTelephone(string telephone)
        {
            Utilisateur utilisateur = null;

            using (var context = ContextFactory.getContext())
            {
                utilisateur = context.Utilisateurs
                .Include(u => u.Contacts)
                .ThenInclude(c => c.ContactUser)
                .Include(u => u.MessagesSent)
                .Include(u => u.MessagesReceived)
                .FirstOrDefault(u => u.Telephone == telephone);

            }
            return utilisateur;
        }


        public static Utilisateur GetUserById(int Id)
        {
            Utilisateur utilisateur = null;

            using (var context = ContextFactory.getContext())
            {
                utilisateur = context.Utilisateurs
                .Include(u => u.Contacts)
                .ThenInclude(c => c.ContactUser)
                .Include(u => u.MessagesSent)
                .Include(u => u.MessagesReceived)
                .FirstOrDefault(u => u.Id == Id);

            }
            return utilisateur;
        }


        public static bool DoesUserExist(string telephone)
        {
            bool userExists = false;

            using (var context = ContextFactory.getContext())
            {
                userExists = context.Utilisateurs.Any(u => u.Telephone == telephone);
            }

            return userExists;
        }



        public static bool InsererUtilisateurAvecImage(string telephone, string fullname, string password, string photoPath, string infos)
        {
            if (!File.Exists(photoPath))
            {
                Console.WriteLine("Le fichier image spécifié n'existe pas.");
                return false;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(photoPath);

                using (var context = ContextFactory.getContext())
                {
                    var utilisateur = new Utilisateur
                    {
                        Telephone = telephone,
                        FullName = fullname,
                        Password = password,
                        Photo = imageBytes,
                        Infos = infos
                    };

                    context.Utilisateurs.Add(utilisateur);
                    context.SaveChanges();

                    Console.WriteLine("L'utilisateur a été ajouté avec succès.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion de l'utilisateur : " + ex.Message);
                return false;
            }
        }



        public static string AddContact(int utilisateurId, int contactUserId)
        {
            using (var context = ContextFactory.getContext())
            {
                try
                {
                    Utilisateur utilisateur = context.Utilisateurs.FirstOrDefault(u => u.Id == utilisateurId);
                    Utilisateur contactUser = context.Utilisateurs.FirstOrDefault(u => u.Id == contactUserId);

                    if (utilisateur == null || contactUser == null)
                    {
                        return "Utilisateur N'exist pas";
                    }

                    if (contactUser == null)
                    {
                        return "Numero de telephone N'exist pas";
                    }

                    if (utilisateurId == contactUserId)
                    {
                        return "Vous ne pouvez pas ajouter votre propre numéro de téléphone en tant que contact.";
                    }

                    bool contactExists = context.Contacts.Any(c => c.UtilisateurId == utilisateurId && c.ContactUserId == contactUserId);

                    if (contactExists)
                    {
                        return "Ce Contact Déja Ajouter";
                    }

                    Contact newContact = new Contact
                    {
                        UtilisateurId = utilisateurId,
                        ContactUserId = contactUserId
                    };

                    context.Contacts.Add(newContact);
                    context.SaveChanges();

                    return "Contact bien Ajouter";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite lors de l'ajout du contact : " + ex.Message);
                    return "Une erreur s'est produite lors de l'ajout du contact";
                }
            }
        }





    }
}
