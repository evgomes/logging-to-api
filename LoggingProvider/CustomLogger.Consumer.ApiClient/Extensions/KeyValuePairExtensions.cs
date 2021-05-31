using System.Collections.Generic;

namespace CustomLogger.Consumer.ApiClient.Extensions
{
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Checks if a KeyValuePair is null.
        /// 
        /// Reference: https://stackoverflow.com/questions/1641392/the-default-for-keyvaluepair
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="keyValuePair">KeyValuePair to compare.</param>
        /// <returns>Indication if the value is null.</returns>
        public static bool IsNull<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair)
        {
            return keyValuePair.Equals(new KeyValuePair<TKey, TValue>());
        }
    }
}
