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
                    .FirstOrDefault(u => u.Telephone == telephone && u.Password == password);
            }
            return utilisateur;
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

    }
}
