using Microsoft.AspNetCore.Mvc;
using System.Net;
using sample_dot_net.Business.Logic;
using MediatR;

namespace sample_dot_net.Controllers;


[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{

    private readonly ILogger<DocumentController> _logger;
    private readonly IHttpClientFactory _factory;
    private readonly IMediator _mediator;

    public DocumentController(ILogger<DocumentController> logger, IHttpClientFactory factory, IMediator mediator)
    {
        _logger = logger;
        _factory = factory;
        _mediator = mediator;
    }
    
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(FileResultViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllBytes(string url)
    {
        byte[] bytes = await _mediator.Send(new GetDocumentBytes(url));

        var base64file = Convert.ToBase64String(bytes);

        var result = new FileResultViewModel()
        {
            DataUrl = "data:application/pdf;base64",
            DocumentContent = base64file.Trim(),
            FileName = "2024-04-08_TestDocPdf.pdf",
            MimeType = "application/pdf"
        };
       
        return Ok(result);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFile(string url)
    {
        byte[] bytes = await _mediator.Send(new GetDocumentBytes(url));

        string filePath = "test_file.pdf";

        return File(bytes, "application/pdf", filePath);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(FileResultViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllBytesWithPdf(string url)
    {
        byte[] bytes = await _mediator.Send(new GetDocumentBytes(url));

        bytes = DocumentUtility.PdfPutWatermark(bytes, "Rahul", "Srigadde", 22);

        var base64file = Convert.ToBase64String(bytes);

        var result = new FileResultViewModel()
        {
            DataUrl = "data:application/pdf;base64",
            DocumentContent = base64file.Trim(),
            FileName = "2024-04-08_TestDocPdf.pdf",
            MimeType = "application/pdf"
        };

        return Ok(result);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFileWithPdf(string url)
    {
        byte[] bytes = await _mediator.Send(new GetDocumentBytes(url));

        bytes = DocumentUtility.PdfPutWatermark(bytes, "Rahul", "Srigadde", 22);

        string filePath = "test_file.pdf";

        return File(bytes, "application/pdf", filePath);
    }


}

