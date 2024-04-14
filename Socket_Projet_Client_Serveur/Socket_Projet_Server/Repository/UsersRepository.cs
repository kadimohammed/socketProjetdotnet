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
    internal class UsersRepository
    {
        public static Utilisateur GetUserByTelephoneAndPassword(string telephone, string password)
        {
            Utilisateur utilisateur = null;

            using (SqlConnection connection = Connexion.InitConnection())
            {
                string query = "SELECT * FROM Utilisateurs WHERE telephone = @Telephone AND password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Telephone", telephone);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            utilisateur = new Utilisateur
                            {
                                UserID = Convert.ToInt32(reader["userID"]),
                                Telephone = reader["telephone"].ToString(),
                                FullName = reader["fullname"].ToString(),
                                Password = reader["password"].ToString(),
                                Photo = (byte[])reader["photo"], // Récupération de la colonne photo comme un tableau d'octets
                                Infos = reader["infos"].ToString() // Récupération de la colonne infos comme une chaîne de caractères
                            };
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur lors de la sélection de l'utilisateur : " + ex.Message);
                    }
                }
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

                using (SqlConnection connection = Connexion.InitConnection())
                {
                    string query = @"INSERT INTO Utilisateurs (telephone, fullname, password, photo, infos)
                                 VALUES (@Telephone, @Fullname, @Password, @Photo, @Infos)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Telephone", telephone);
                        command.Parameters.AddWithValue("@Fullname", fullname);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Photo", imageBytes);
                        command.Parameters.AddWithValue("@Infos", infos);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("L'utilisateur a été ajouté avec succès.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Aucune ligne n'a été insérée.");
                            return false;
                        }
                    }
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
