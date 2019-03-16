// ReSharper disable CheckNamespace
namespace System.Collections.Generic
{
    internal static class KeyValuePairExtensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> item, out TKey Key, out TValue Value)
        {
            Key = item.Key;
            Value = item.Value;
        }
    }
}
