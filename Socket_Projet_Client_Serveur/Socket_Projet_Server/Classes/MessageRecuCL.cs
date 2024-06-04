namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class MessageRecuCL
    {
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
