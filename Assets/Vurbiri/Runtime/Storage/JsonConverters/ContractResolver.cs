using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Vurbiri
{
	sealed public class ContractResolver : DefaultContractResolver
    {
        private static readonly ContractResolver s_instance = new();
        
        private readonly List<JsonConverter> _converters = new()
        {
            new ColorConverter(), new Color32Converter(),
            new Vector2Converter(), new Vector3Converter(), new Vector2IntConverter(), new Vector3IntConverter(),
        };

        public static ContractResolver Instance => s_instance;

        public static void Add(JsonConverter converter) => s_instance._converters.Add(converter);
        public static void Add(params JsonConverter[] converters) => s_instance._converters.AddRange(converters);

        private ContractResolver() { }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            for (int i = _converters.Count - 1; i >= 0; i--)
            {
                if (_converters[i].CanConvert(objectType))
                {
                    contract.Converter = _converters[i];
                    break;
                }
            }
            return contract;
        }
    }
}
