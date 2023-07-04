using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class PdfContractController : Controller
    {
        private readonly IPdfContractService _pdfContractService;

        public PdfContractController(IPdfContractService pdfContractService)
        {
            _pdfContractService = pdfContractService;
        }

        [HttpGet("uploaded/{userId}")]
        public async Task<IActionResult> GetAllUploadedContracts(string userId, [FromQuery] SearchParameters searchParameters)
        {
            var contracts = await _pdfContractService.GetAllUploadedContractsAsync(userId, searchParameters);
            if (contracts == null)
            {
                return NotFound();
            }
            return Ok(contracts);
        }

        [HttpGet("received/{userId}")]
        public async Task<IActionResult> GetAllReceivedContracts(string userId, [FromQuery] SearchParameters searchParameters)
        {
            var contracts = await _pdfContractService.GetAllReceivedContractsAsync(userId, searchParameters);
            if (contracts == null)
            {
                return NotFound();
            }
            return Ok(contracts);
        }

        [HttpGet("unsigned/{userId}")]
        public async Task<IActionResult> GetUnsignedContractsQty(string userId)
        {
            var contractsQty = await _pdfContractService.GetUnsignedContractsQty(userId);
            return Ok(contractsQty);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdfContract([FromForm] PdfContractUploadReq contractUploadRequest)
        {
            if (contractUploadRequest.UserDTO == null)
                return BadRequest();
                        
            var pdfContractInfo = await _pdfContractService.UploadPdfContractAsync(contractUploadRequest);
            if (pdfContractInfo == null)
                return BadRequest();

            return Ok(pdfContractInfo);
        }

        [HttpPost("sendcontract")]
        public async Task<IActionResult> SendPdfContractForSign([FromForm] PdfContractUpdateRequest contractUpdateRequest)
        {
            if (contractUpdateRequest.PdfContractInfo == null || contractUpdateRequest.File == null)
                return BadRequest();

            await _pdfContractService.UpdatePdfContractAsync(contractUpdateRequest);
            return Ok();
        }

        [HttpGet("id/{fileId}")]
        public async Task<IActionResult> GetContractById(string fileId)
        {
            var file = await _pdfContractService.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpDelete("id/{fileId}")]
        public async Task<IActionResult> DeleteContractById(string fileId)
        {
            await _pdfContractService.DeleteByIdAsync(fileId);
            return Ok();
        }
    }
}
