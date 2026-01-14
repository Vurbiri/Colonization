using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

//[assembly: AlwaysLinkAssembly]

namespace Vurbiri
{
	public static class JsonConverters
	{
		private static readonly ContractResolver s_resolver = new();
		private static readonly List<JsonConverter> s_converters = new()
		{
			new ColorConverter(), new Color32Converter(),
			new Vector2Converter(), new Vector3Converter(), new Vector2IntConverter(), new Vector3IntConverter(),
		};

        [Impl(256)] public static void Add(JsonConverter converter) => s_converters.Add(converter);
        [Impl(256)] public static void Add(params JsonConverter[] converters) => s_converters.AddRange(converters);

        [Impl(256)] public static bool Remove(JsonConverter converter) => s_converters.Remove(converter);


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Init()
		{
			JsonConvert.DefaultSettings = GetJsonSerializerSettings;
			Log.Info("[JsonConverters] Initialized");

            static JsonSerializerSettings GetJsonSerializerSettings() => new() { ContractResolver = s_resolver };
        }

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		private static void Init_Ed()
		{
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				Init();
		}
#endif

		// ********************* Nested **************************
		sealed private class ContractResolver : DefaultContractResolver
		{
			protected override JsonContract CreateContract(Type objectType)
			{
				var contract = base.CreateContract(objectType);

				for (int i = s_converters.Count - 1; i >= 0; --i)
				{
					if (s_converters[i].CanConvert(objectType))
					{
						contract.Converter = s_converters[i];
						break;
					}
				}
				return contract;
			}
		}
		//********************************************************
	}
}
