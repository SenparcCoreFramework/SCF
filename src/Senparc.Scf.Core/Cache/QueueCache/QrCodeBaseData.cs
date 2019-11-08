using System;

namespace Senparc.Scf.Core.Cache
{
    /// <summary>
    /// 二维码缓存[暂未使用]
    /// </summary>
    [Serializable]
    public class QrCodeBaseData
    {
        /// <summary>
        /// 即SceneId的字
        /// </summary>
        public string Key { get; set; }
        public int SceneId { get; set; }
        public string Ticket { get; set; }
        public DateTime ExpireTime { get; set; }
        public Guid Guid { get; set; }
        /// <summary>
        /// 验证通过
        /// </summary>
        public bool CheckPassed { get; set; }
        public string UserName { get; set; }

        public QrCodeBaseData(int sceneId, int expireSeconds, string ticket, Guid guid)
        {
            SceneId = sceneId;
            Key = sceneId.ToString();
            Ticket = ticket;
            ExpireTime = DateTime.Now.AddSeconds(expireSeconds - 5);
            Guid = guid;
        }
    }
}
