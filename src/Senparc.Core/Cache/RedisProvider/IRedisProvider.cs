using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Senparc.Core.Cache.RedisProvider
{
    /// <summary>
    /// redis提供
    /// </summary>
    public interface IRedisProvider
    {
        long HashDelete(string redisKey, IEnumerable<RedisValue> hashField);
        bool HashDelete(string redisKey, string hashField);
        Task<long> HashDeleteAsync(string redisKey, IEnumerable<RedisValue> hashField);
        Task<bool> HashDeleteAsync(string redisKey, string hashField);
        bool HashExists(string redisKey, string hashField);
        Task<bool> HashExistsAsync(string redisKey, string hashField);
        RedisValue[] HashGet(string redisKey, RedisValue[] hashField, string value);
        RedisValue HashGet(string redisKey, string hashField);
        T HashGet<T>(string redisKey, string hashField);
        Task<IEnumerable<RedisValue>> HashGetAsync(string redisKey, RedisValue[] hashField, string value);
        Task<RedisValue> HashGetAsync(string redisKey, string hashField);
        Task<T> HashGetAsync<T>(string redisKey, string hashField);
        IEnumerable<RedisValue> HashKeys(string redisKey);
        Task<IEnumerable<RedisValue>> HashKeysAsync(string redisKey);
        void HashSet(string redisKey, IEnumerable<HashEntry> hashFields);
        bool HashSet(string redisKey, string hashField, string value);
        bool HashSet<T>(string redisKey, string hashField, T value);
        Task HashSetAsync(string redisKey, IEnumerable<HashEntry> hashFields);
        Task<bool> HashSetAsync(string redisKey, string hashField, string value);
        Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value);
        RedisValue[] HashValues(string redisKey);
        Task<IEnumerable<RedisValue>> HashValuesAsync(string redisKey);
        long KeyDelete(IEnumerable<string> redisKeys);
        bool KeyDelete(string redisKey);
        Task<long> KeyDeleteAsync(IEnumerable<string> redisKeys);
        Task<bool> KeyDeleteAsync(string redisKey);
        bool KeyExists(string redisKey);
        Task<bool> KeyExistsAsync(string redisKey);
        bool KeyExpire(string redisKey, TimeSpan? expiry);
        Task<bool> KeyExpireAsync(string redisKey, TimeSpan? expiry);
        bool KeyRename(string redisKey, string redisNewKey);
        Task<bool> KeyRenameAsync(string redisKey, string redisNewKey);
        string ListLeftPop(string redisKey);
        T ListLeftPop<T>(string redisKey);
        Task<string> ListLeftPopAsync(string redisKey);
        Task<T> ListLeftPopAsync<T>(string redisKey);
        long ListLeftPush(string redisKey, string redisValue);
        long ListLeftPush<T>(string redisKey, T redisValue);
        Task<long> ListLeftPushAsync(string redisKey, string redisValue);
        Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue);
        long ListLength(string redisKey);
        Task<long> ListLengthAsync(string redisKey);
        IEnumerable<RedisValue> ListRange(string redisKey);
        Task<IEnumerable<RedisValue>> ListRangeAsync(string redisKey);
        long ListRemove(string redisKey, string redisValue);
        Task<long> ListRemoveAsync(string redisKey, string redisValue);
        string ListRightPop(string redisKey);
        T ListRightPop<T>(string redisKey);
        Task<string> ListRightPopAsync(string redisKey);
        Task<T> ListRightPopAsync<T>(string redisKey);
        long ListRightPush(string redisKey, string redisValue);
        long ListRightPush<T>(string redisKey, T redisValue);
        Task<long> ListRightPushAsync(string redisKey, string redisValue);
        Task<long> ListRightPushAsync<T>(string redisKey, T redisValue);
        Task<bool> LockReleaseAsync(string key);
        Task<bool> LockReleaseAsync(string key, string lockToken);
        Task<bool> LockTakeAsync(string key, TimeSpan timeSpan, string lockToken);
        Task<bool> LockTakeAsync(string key, TimeSpan timeSpan);
        long Publish(RedisChannel channel, RedisValue message);
        long Publish<T>(RedisChannel channel, T message);
        Task<long> PublishAsync(RedisChannel channel, RedisValue message);
        Task<long> PublishAsync<T>(RedisChannel channel, T message);
        bool SortedSetAdd(string redisKey, string member, double score);
        bool SortedSetAdd<T>(string redisKey, T member, double score);
        Task<bool> SortedSetAddAsync(string redisKey, string member, double score);
        Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score);
        long SortedSetLength(string redisKey);
        bool SortedSetLength(string redisKey, string memebr);
        Task<long> SortedSetLengthAsync(string redisKey);
        IEnumerable<RedisValue> SortedSetRangeByRank(string redisKey);
        Task<IEnumerable<RedisValue>> SortedSetRangeByRankAsync(string redisKey);
        Task<bool> SortedSetRemoveAsync(string redisKey, string memebr);
        string StringGet(string redisKey, TimeSpan? expiry = null);
        T StringGet<T>(string redisKey, TimeSpan? expiry = null);
        Task<string> StringGetAsync(string redisKey, string redisValue, TimeSpan? expiry = null);
        Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null);
        bool StringSet(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs);
        bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null);
        bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null);
        Task<bool> StringSetAsync(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs);
        Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null);
        Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null);
        void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handle);
        Task SubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handle);
    }
}