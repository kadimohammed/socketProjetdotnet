namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class MessageEnvoyerCL
    {
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public byte[]? Image { get; set; }
        public byte[]? Audio { get; set; }
        public byte[]? Video { get; set; }
    }
}
