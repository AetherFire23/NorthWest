using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HttpStuff
{
    public class SSEStream : IDisposable // IHTTPClient one day to swap between 
    {
        // dont use Using and dispose manually.
        private readonly Stream _stream;
        public SSEStream(Stream stream)
        {
                
        }

        public void Dispose()
        {
            
        }
    }
}
