using System.Text;

namespace APIGateway.Routes
{
    public class Destionation
    {
        public string Uri { get; set; }

        public Destionation(string uri) 
        {
            Uri = uri;
        }

        private Destionation() 
        {
            Uri = "/";
        }

        private string CreateDestinationUri(HttpRequest request) 
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            var endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1) 
            {
                endpoint = endpointSplit[1];
            }

            return Uri + endpoint + queryString;
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequest request) 
        {
            string requestContent;
            using (Stream recieveStream = request.Body) 
            {
                using (StreamReader readStream = new StreamReader(recieveStream, encoding: Encoding.UTF8)) 
                {
                    requestContent = readStream.ReadToEnd();
                }
            }

            HttpClient client = new HttpClient();
            HttpRequestMessage newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
            HttpResponseMessage response = await client.SendAsync(newRequest);

            return response;
        }
    }
}
