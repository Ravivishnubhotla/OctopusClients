using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Octopus.Client.Model
{
    public class PackageReferenceByActionIdResource
    {
        public string PackageReferenceName { get; set; }
        public string ActionId { get; set; }

        public PackageReferenceByActionIdResource()
        {

        }

        public PackageReferenceByActionIdResource(string actionId)
        {
            ActionId = actionId;
        }

        public PackageReferenceByActionIdResource(string actionId, string packageReferenceName)
        {
            PackageReferenceName = packageReferenceName;
            ActionId = actionId;
        }

        public class PackageReferenceResourceConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var propertyValue = (PackageReferenceByActionIdResource) value;

                if (propertyValue.PackageReferenceName == null)
                {
                    writer.WriteValue(propertyValue.ActionId);
                    return;
                }

                // https://stackoverflow.com/questions/12314438/self-referencing-loop-in-json-net-jsonserializer-from-custom-jsonconverter-web
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(ActionId));
                writer.WriteValue(propertyValue.ActionId);
                writer.WritePropertyName(nameof(PackageReferenceName));
                writer.WriteValue(propertyValue.PackageReferenceName);
                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Null:
                        return null;

                    // If it is an object we assume it's a SensitiveValue
                    case JsonToken.StartObject:
                    {
                        var jo = JObject.Load(reader);
                        return new PackageReferenceByActionIdResource(
                            jo.GetValue(nameof(ActionId))?.ToObject<string>(),
                            jo.GetValue(nameof(PackageReferenceName))?.ToObject<string>());
                    }

                    // Otherwise treat it as a string
                    default:
                        return new PackageReferenceByActionIdResource(Convert.ToString(reader.Value));
                }

            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(PackageReferenceByActionIdResource).IsAssignableFrom(objectType);
            }
        }
    }
}
