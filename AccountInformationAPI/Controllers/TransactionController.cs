using AccountInformationAPI.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.ContextRepositoryClasses;
using TransactionRepositoryLib.Connection;

namespace AccountInformationAPI.Controllers
{
    [ApiController]
    [Route("cashflowapi/[controller]")]
    public class TransactionController : Controller
    {
        private readonly IMapper _autoMapper;
        private readonly ITransactionRepositoryConnection _transactionRepositoryConnection;

        public TransactionController(IMapper mapper, ITransactionRepositoryConnection repo) 
        { 
            _autoMapper = mapper;
            _transactionRepositoryConnection = repo;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<RoleReadDto>> CreateTransactionAsync([FromBody] TransactionCreateDto transactionDto)
        {
            var transaction = _autoMapper.Map<Transaction>(transactionDto);

            if (transaction == null)
            {
                return BadRequest(transactionDto);
            }

            await _transactionRepositoryConnection.Repository.CreateTransactionAsync(transaction);

            return NoContent();
        }

        [HttpGet("{id}"), ActionName("GetTransactionByIdAsync")]
        public async Task<ActionResult<TransactionReadDto>> GetTransactionByIdAsync(int id)
        {
            Transaction transaction = await _transactionRepositoryConnection.Repository.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(_autoMapper.Map<TransactionReadDto>(transaction));
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<TransactionUpdateDto>> UpdateTransactionAsync([FromBody] TransactionUpdateDto transactionUpdatedDto, int id)
        {
            Transaction transaction = _autoMapper.Map<Transaction>(transactionUpdatedDto);

            await _transactionRepositoryConnection.Repository.UpdateTransactionAsync(transaction, id);

            return Ok(transaction);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTransactionAsync(int id)
        {
            await _transactionRepositoryConnection.Repository.DeleteTransactionAsync(id);

            return NoContent();
        }

        [HttpGet("All")]
        public async Task<IEnumerable<TransactionReadDto>> GetAllTransactionAsync()
        {
            var transactions = await _transactionRepositoryConnection.Repository.GetAllTransactionsAsync();

            return _autoMapper.Map<IEnumerable<TransactionReadDto>>(transactions);
        }
    }
}
