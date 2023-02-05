using System;
using Newtonsoft.Json;

namespace AdsPush.Abstraction
{
    /// <summary>
    /// Applies conversion for notification system based format to bool types.
    /// </summary>
    public class BoolToStringConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            writer.WriteValue((bool)value ? 1 : 0);
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1";
        }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
