using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Vurbiri.International
{
    [Serializable, JsonConverter(typeof(Converter))]
    public struct FileIdAndKey
	{
		public int id;
		public string key;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileIdAndKey(int id, string key)
		{
			this.id = id;
			this.key = key;
		}

        #region Nested Json Converter
        //***************************************************************************
        sealed public class Converter : AJsonConverter<FileIdAndKey>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<string[]>(reader);

                int id;
                try { id = Int32.Parse(data[0], NumberStyles.None); } catch { id = 0; }

                return new FileIdAndKey(id, data[1]);
            }

            protected override void WriteJson(JsonWriter writer, FileIdAndKey value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(value.id.ToString());
                writer.WriteValue(value.key);
                writer.WriteEndArray();
            }
        }
        #endregion
    }
}
