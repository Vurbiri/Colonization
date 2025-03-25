//Assets\Colonization\Scripts\Data\Converters\RoadsConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Roads.Converter))]
    public partial class Roads
    {
        sealed public class Converter : JsonConverter<Roads>
        {
            public override bool CanRead => false;

            public override Roads ReadJson(JsonReader reader, Type objectType, Roads existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, Roads value, JsonSerializer serializer)
            {
                int count = value._roadsLists.Count;
                writer.WriteStartArray();
                for (int i = 0; i < count; i++)
                    Road.Converter.WriteJsonArray(writer, value._roadsLists[i]);
                writer.WriteEndArray();
            }

            public static void ReadFromArray(Roads roads, int[][][] array, Crossroads crossroads)
            {
                for (int i = 0; i < array.Length; i++)
                    CreateRoad(array[i], crossroads);

                #region Local: CreateRoad(...)
                //=================================
                void CreateRoad(int[][] keys, Crossroads crossroads)
                {
                    int count = keys.Length;
                    if (count < 2) return;

                    Crossroad start = crossroads[new(keys[0])];
                    for (int i = 1; i < count; i++)
                    {
                        foreach (var link in start.Links)
                        {
                            if (link.Contains(new(keys[i])))
                            {
                                roads.Build(link.SetStart(start));
                                start = link.End;
                                break;
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }
}
