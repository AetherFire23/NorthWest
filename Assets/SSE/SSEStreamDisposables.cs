using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
