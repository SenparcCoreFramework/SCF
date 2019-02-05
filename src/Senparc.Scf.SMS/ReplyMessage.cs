using System;

namespace Senparc.Scf.SMS
{
    public interface IReplyMessage
    {
        int State { get; set; }
        int Id { get; set; }
        string PhoneNumber { get; set; }
        DateTime DateTime { get; set; }
    }

    public class ReplyMessage : IReplyMessage
    {
        public int State { get; set; }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateTime { get; set; }
    }
}
