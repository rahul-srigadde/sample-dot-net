using System;
using MediatR;

namespace sample_dot_net.Business.Logic
{
    public class GetDocumentBytes : IRequest<byte[]>
    {
        public readonly string Url;
        public GetDocumentBytes(string url)
        {
            Url = url;
        }
        public class GetUserDocumentsHandler : IRequestHandler<GetDocumentBytes, byte[]>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<GetUserDocumentsHandler> _logger;
            private readonly IHttpClientFactory _factory;

            public GetUserDocumentsHandler(IMediator mediator, ILogger<GetUserDocumentsHandler> logger, IHttpClientFactory factory)
            {
                _mediator = mediator;
                _logger = logger;
                _factory = factory;
            }

            public async Task< byte[]> Handle(GetDocumentBytes request, CancellationToken cancellationToken)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, request.Url);
                req.Headers.Add("User-Agent", "InvestorVision LP API");
                req.Headers.Add("Accept", "application/json");

                var client = _factory.CreateClient();

                var response = await client.SendAsync(req);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsByteArrayAsync();

                    return responseStream;
                }
                return new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            }
        }
    }
}

