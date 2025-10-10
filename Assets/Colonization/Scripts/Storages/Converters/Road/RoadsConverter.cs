using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    public partial class Roads
    {
        sealed public class Converter : AJsonConverter<Roads>
        {
            private readonly Roads _roads;
            private readonly Crossroads _crossroads;

            public Converter() { }
            public Converter(Roads roads, Crossroads crossroads)
            {
                _roads = roads;
                _crossroads = crossroads;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Key[][] keys = serializer.Deserialize<Key[][]>(reader);
                for (int i = 0; i < keys.Length; i++)
                    CreateRoad(keys[i]);

                return _roads;

                #region Local: CreateRoad(...)
                //=================================
                void CreateRoad(Key[] keys)
                {
                    int count = keys.Length;
                    if (count < 2) return;

                    Crossroad start = _crossroads[keys[0]];
                    for (int i = 1; i < count; i++)
                    {
                        foreach (var link in start.Links)
                        {
                            if (link.Contains(keys[i]))
                            {
                                _roads.Build(start.Type, link);
                                start = link.GetOtherCrossroad(start.Type);
                                break;
                            }
                        }
                    }
                }
                #endregion
            }

            protected override void WriteJson(JsonWriter writer, Roads roads, JsonSerializer serializer)
            {
                //if (writer is JsonTextWriter jsonTextWriter)
                //{
                //jsonTextWriter.QuoteChar = '\'';
                //jsonTextWriter.QuoteName = false;
                //jsonTextWriter.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                //}

                int count = roads._roadsLists.Count;
                writer.WriteStartArray();
                for (int i = 0; i < count; i++)
                    Road.Converter.WriteJsonArray(writer, roads._roadsLists[i]);
                writer.WriteEndArray();
            }
        }
    }
}
