using System.Collections.Generic;

namespace Senparc.SMS
{
    public class ReplyMessageCollection
    {
        public List<IReplyMessage> MsgCollection { get; set; }

        public ReplyMessageCollection()
        {
            MsgCollection=new List<IReplyMessage>();
        }
    }
}
