using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Octopus.Client.Model
{
    public class PackageReferenceByActionNameResource
    {
        public string PackageReferenceName { get; set; }
        public string Name { get; set; }

        public PackageReferenceByActionNameResource()
        {

        }

        public PackageReferenceByActionNameResource(string actionName)
        {
            Name = actionName;
        }

        public PackageReferenceByActionNameResource(string actionName, string packageReferenceName)
        {
            PackageReferenceName = packageReferenceName;
            Name = actionName;
        }

        public class PackageReferenceResourceConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var propertyValue = (PackageReferenceByActionNameResource)value;

                if (propertyValue.PackageReferenceName == null)
                {
                    writer.WriteValue(propertyValue.Name);
                    return;
                }

                // https://stackoverflow.com/questions/12314438/self-referencing-loop-in-json-net-jsonserializer-from-custom-jsonconverter-web
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(Name));
                writer.WriteValue(propertyValue.Name);
                writer.WritePropertyName(nameof(PackageReferenceName));
                writer.WriteValue(propertyValue.PackageReferenceName);
                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Null:
                        return null;

                    // If it is an object we assume it's a SensitiveValue
                    case JsonToken.StartObject:
                        {
                            var jo = JObject.Load(reader);
                            return new PackageReferenceByActionNameResource(
                                   jo.GetValue(nameof(Name))?.ToObject<string>(),
                                   jo.GetValue(nameof(PackageReferenceName))?.ToObject<string>());
                        }

                    // Otherwise treat it as a string
                    default:
                        return new PackageReferenceByActionNameResource(Convert.ToString(reader.Value));
                }

            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(PackageReferenceByActionNameResource).IsAssignableFrom(objectType);
            }
        }
    }
}
