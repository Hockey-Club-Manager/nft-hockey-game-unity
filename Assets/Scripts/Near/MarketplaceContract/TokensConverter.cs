using System;
using System.Collections.Generic;
using Near.Models.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract
{
    public class TokensConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<NFT>));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var tokens = data["data"]?["tokens"];

            List<NFT> result = new List<NFT>();

            if (tokens == null)
            {
                return result;
            }
            
            foreach (var jToken in tokens)
            {
                var json = jToken.ToString();
                var token = JsonConvert.DeserializeObject<NFT>(json, new PlayerConverter());
                result.Add(token);
            }

            return result;
        }
    }
}