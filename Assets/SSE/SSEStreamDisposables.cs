using System.IO;
using System.Net.Http;

namespace Assets.SSE
{
    public class SSEStreamDisposables
    {
        public HttpRequestMessage Request;
        public HttpResponseMessage Response;
        public Stream Stream;
        public StreamReader StreamReader;
    }
}
