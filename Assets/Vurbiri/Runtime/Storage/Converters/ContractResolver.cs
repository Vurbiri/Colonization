using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Vurbiri
{
	public class ContractResolver : DefaultContractResolver
    {
        private static readonly ContractResolver _instance = new();

        internal static List<JsonConverter> s_converters = new()
        {
            new ColorConverter(),
        };

        public static ContractResolver Instance => _instance;

        public static void Add(JsonConverter converter) => s_converters.Add(converter);
        public static void Add(params JsonConverter[] converters) => s_converters.AddRange(converters);

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            for (int i = s_converters.Count - 1; i >= 0; i--)
            {
                if (s_converters[i].CanConvert(objectType))
                {
                    contract.Converter = s_converters[i];
                    return contract;
                }
            }
            return contract;
        }


#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        internal static void Init()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new ContractResolver()
            };
        }
#endif
    }
}
