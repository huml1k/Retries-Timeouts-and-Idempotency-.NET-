using System.Net.Http;
using System.Text;

namespace APIGateway.Routes
{
    public class Destination
    {
        public string Uri { get; set; }

        public bool RequiresAuthentication { get; set; }


        private readonly IHttpClientFactory _httpClientFactory;

        public Destination(
            string uri,
            bool requiresAuthentication,
            IHttpClientFactory httpClientFactory)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
            _httpClientFactory = httpClientFactory;
        }

        private string CreateDestinationUri(HttpRequest request)
        {
            var requestPath = request.Path.ToString();
            var basePath = '/' + requestPath.Split('/')[1];
            var endpoint = requestPath.Substring(basePath.Length).TrimStart('/');

            return $"{Uri.TrimEnd('/')}/{endpoint}{request.QueryString}";
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequest request)
        {
            request.EnableBuffering();

            string requestContent;
            using (var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096,
                leaveOpen: true))
            {
                requestContent = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            using var client = _httpClientFactory.CreateClient();

            var mediaType = request.ContentType?.Split(';')[0].Trim();
            mediaType ??= "text/plain";

            var newRequest = new HttpRequestMessage(
                new HttpMethod(request.Method),
                CreateDestinationUri(request))
            {
                Content = new StringContent(requestContent, Encoding.UTF8, mediaType)
            };

            foreach (var header in request.Headers)
            {
                if (!newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    newRequest.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            return await client.SendAsync(newRequest);
        }
    }
}
