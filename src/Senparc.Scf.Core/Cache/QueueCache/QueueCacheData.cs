using System;

namespace Senparc.Scf.Core.Cache
{
    [Serializable]
    public class QueueCacheData<T>
    {
        public string Key { get; set; }

        public DateTime LastActiveTime { get; set; }

        public T Data { get; set; }

        public QueueCacheData(string key)
        {
            Key = key;
            LastActiveTime = DateTime.Now;
        }
    }
}