using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PowerCreator.LiveClient.Infrastructure
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
        public static T DeserializeObject<T>(IEnumerable<string> inputContents)
        {
            if (!inputContents.Any()) return default(T);
            return JsonConvert.DeserializeObject<T>($"[{inputContents.Aggregate((item1, item2) => string.Format($"{item1},{item2}"))}]");
        }
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
