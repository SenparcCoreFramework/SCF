using System;

namespace Senparc.Core.Cache.Lock
{
    public class RedisCacheLock<T> : BaseCacheLock where T : class/*, new()*/
    {
        private Redlock.CSharp.Redlock _dlm;
        private Redlock.CSharp.Lock _lockObject;

        private IRedisStrategy _redisStragegy;

        public RedisCacheLock(IRedisStrategy stragegy, string resourceName, string key, int retryCount, TimeSpan retryDelay)
            : base(stragegy, resourceName, key, retryCount, retryDelay)
        {
            _redisStragegy = stragegy;
        }

        public override bool Lock(string resourceName)
        {
            return Lock(resourceName, 0, new TimeSpan());
        }

        public override bool Lock(string resourceName, int retryCount, TimeSpan retryDelay)
        {
            if (retryCount != 0)
            {
                _dlm = new Redlock.CSharp.Redlock(retryCount, retryDelay, _redisStragegy._client);
            }
            else if (_dlm == null)
            {
                _dlm = new Redlock.CSharp.Redlock(_redisStragegy._client);
            }

            var successfull = _dlm.Lock(resourceName, new TimeSpan(0, 0, 10), out _lockObject);
            return successfull;
        }

        public override void UnLock(string resourceName)
        {
            if (_lockObject != null)
            {
                _dlm.Unlock(_lockObject);
            }
        }
    }
}
