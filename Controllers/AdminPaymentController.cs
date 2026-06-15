namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;
 
[Route("api/admin/payments")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminPaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IPayerRepository _payerRepository;
 
    public AdminPaymentController(IPaymentService paymentService, IPayerRepository payerRepository)
    {
        _paymentService = paymentService;
        _payerRepository = payerRepository;
    }
 
   
    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _paymentService.GetAllPaymentsAsync();
        return Ok(result);
    }
 
  
    [HttpPost("search")]
    public async Task<IActionResult> SearchPayments([FromBody] SearchPaymentDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        var result = await _paymentService.SearchPaymentsAsync(searchDto);
        return Ok(result);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto paymentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _paymentService.CreatePaymentAsync(paymentDto);
        return Ok(AppConstants.RecordCreated);
    }
 
    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdatePayment(int paymentId, [FromBody] PaymentCreateDto paymentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _paymentService.UpdatePaymentAsync(paymentId, paymentDto);
        return Ok(AppConstants.RecordUpdated);
    }
 
    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> DeletePayment(int paymentId)
    {
        await _paymentService.DeletePaymentAsync(paymentId);
        return Ok(AppConstants.RecordDeleted);
    }
 
    
    [HttpGet("payers")]
    public async Task<IActionResult> GetPayers()
    {
        var payers = await _payerRepository.GetAllPayersAsync();
        var result = payers.Select(p => new { p.PayerID, p.PayerName });
        return Ok(result);
    }
}
 
 