using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegisterNewClient
{
    public class HttpGZipClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            var response = await base.SendAsync(request, cancellationToken);
                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    await response.Content.LoadIntoBufferAsync();
                    response.Content = new HttpGZipContent(await response.Content.ReadAsStreamAsync());
                }
            return response;
        }        

        public override bool SupportsAutomaticDecompression
        {
            get
            {
                return true;
            }
        }
    }

    internal sealed class HttpGZipContent : HttpContent
    {
        public readonly GZipStream m_stream;
        public HttpGZipContent(Stream deflatedStream)
        {
            m_stream = new GZipStream(deflatedStream, CompressionMode.Decompress);
            Xamarin.Forms.Device.OnPlatform(Default: () =>
            {
                this.Headers.ContentLength = m_stream.BaseStream.Length;
            }, WinPhone: () =>
             {

             });
        }



        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return m_stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = m_stream.BaseStream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            m_stream.Dispose();
            base.Dispose(disposing);
        }
    }
}
