using System.Collections.Generic;
using Containers;

namespace Tools.Extensions
{
    public static class CollectionExtensions
    {
        public static bool ContainsById<T>(this IEnumerable<T> collection, BaseInfo info) where T : BaseInfo
        {
            foreach (var baseInfo in collection)
            {
                if (baseInfo.id == info.id)
                    return true;
            }

            return false;
        }
        
        public static bool TryGetValueById<T>(this IEnumerable<T> collection, BaseInfo info, out T value) where T : BaseInfo
        {
            value = null;
            foreach (var baseInfo in collection)
            {
                if (baseInfo.id == info.id)
                {
                    value = baseInfo;
                    return true;
                }
            }

            return false;
        }
    }
}