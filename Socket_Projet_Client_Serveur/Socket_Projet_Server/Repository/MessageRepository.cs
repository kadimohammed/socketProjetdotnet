using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Factory;
using Socket_Projet_Server.Models;

namespace Socket_Projet_Server.Repository
{
    public class MessageRepository
    {
        public static string AddMessage(MessageEnvoyerCL message)
        {
            using (var context = ContextFactory.getContext())
            {
                try
                {
                    Message m = new Message();
                    m.Content = message.Content.ToString();
                    m.SendDate = message.SendDate;
                    m.SenderId = message.SenderId;
                    m.ReceiverId = message.ReceiverId;

                    context.Messages.Add(m);
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
