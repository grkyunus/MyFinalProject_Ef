using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        IMemoryCache _memoryCache;   // Constructor ile enjecte etmek işe yaramaz. Sisteme eklemek için CoreModule.cs eklenebilir.

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            // alt kısımda sadece key istiyoruz. value istemediğimizi belirtymek için "out _" kullanılabilir.
            return _memoryCache.TryGetValue(key,out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            //var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            
            // Normalde hoca yukarıdaki 2 satırı kullandı ancak 6 sürümünde hata alındı ve alt taraftaki kod satırı eklendiğinde sorunsuz çalıştı 
            // Core not eğer eklenmedi ise eklemeli ve iki kod arasındaki temel fark karşılaştırmak için not almayı unutma!!!

            dynamic cacheEntriesCollection = null;
            var cacheEntriesFieldCollectionDefinition = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var caacheEntriesPropetyCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            if (cacheEntriesFieldCollectionDefinition != null)
            {
                var coherentStateValueCollection = cacheEntriesFieldCollectionDefinition.GetValue(_memoryCache);
                var entriesCollectionValueCollection = coherentStateValueCollection.GetType().GetProperty
                    (
                        "EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance
                    );
                cacheEntriesCollection = entriesCollectionValueCollection.GetValue(coherentStateValueCollection)!;
            }
            // ****************
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}

// Burdaki olay sadece memorycache eklemek değil farklı bir sistem kullanabilmek için kendimize uyarlıyoruz.
// Adapter Pattern yöntemi kullanıldı yani eklenen sistem göre biz çalışmıyacağız, sistem bize göre çalışacak.
