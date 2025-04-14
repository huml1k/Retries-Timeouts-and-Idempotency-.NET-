using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace APIGateway.Parser
{
    public class JsonParser
    {
        public static T LoadFromFile<T>(string filePath) 
        {
            using (StreamReader reader = new StreamReader(filePath)) 
            {
                string json = reader.ReadToEnd();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        public static T Deserialize<T>(object jsonObject) 
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(jsonObject));
        }
    }
}
