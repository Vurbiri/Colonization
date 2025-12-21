using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class PlayerColors
	{
        sealed public class Converter : AJsonConverter<PlayerColors>
        {
            private const int SIZE_ARRAY = 3;

            private readonly PlayerColors _colors;

            public Converter() { }
            public Converter(PlayerColors colors) => _colors = colors;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                const float F_MAX_BYTE = byte.MaxValue;

                var data = serializer.Deserialize<float[][]>(reader);

                float[] color;
                for (int i = 0; i < SIZE_ARRAY; ++i)
                {
                    color = data[i];
                    _colors._colors[i] = new(color[0] / F_MAX_BYTE, color[1] / F_MAX_BYTE, color[2] / F_MAX_BYTE);
                }

                return _colors;
            }

            protected override void WriteJson(JsonWriter writer, PlayerColors colors, JsonSerializer serializer)
            {
                Color color;

                writer.WriteStartArray();
                for (int i = 0; i < SIZE_ARRAY; ++i)
                {
                    writer.WriteStartArray();
                    {
                        color = colors._colors[i];

                        writer.WriteValue(color.r.ToByte());
                        writer.WriteValue(color.g.ToByte());
                        writer.WriteValue(color.b.ToByte());
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndArray();
            }
        }
    }
}
