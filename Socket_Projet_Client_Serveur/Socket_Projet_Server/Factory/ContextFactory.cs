using Socket_Projet_Server.Models;

namespace Socket_Projet_Server.Factory
{
    public class ContextFactory
    {
        public static MyContext getContext()
        {
            return new MyContext();
        }
    }
}
