using AccountRepositoryLib.Connection;
using AccountInformationAPI.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.ContextRepositoryClasses;

namespace AccountInformationAPI.Controllers
{
    [ApiController]
    [Route("cashflowapi/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMapper _autoMapper;
        private readonly IAccountRepositoryConnection _accountRepositoryConnection;

        public AccountController(IMapper mapper, IAccountRepositoryConnection repo)
        {
            _autoMapper = mapper;
            _accountRepositoryConnection = repo;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<AccountReadDto>> CreateAccountAsync([FromBody] AccountCreateDto accountDto)
        {
            var account = _autoMapper.Map<Account>(accountDto);

            if (account == null)
            {
                return BadRequest(accountDto);
            }

            await _accountRepositoryConnection.Repository.CreateAccountAsync(account);

            var accountReadDto = _autoMapper.Map<AccountReadDto>(account);

            return CreatedAtAction(nameof(GetAccountByIdAsync), new { accountReadDto.Id }, accountReadDto);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<AccountReadDto>> GetAccountsAsync()
        {
            var accounts = await _accountRepositoryConnection.Repository.GetAllAccountsAsync();

            return _autoMapper.Map<IEnumerable<AccountReadDto>>(accounts);
        }

        [HttpGet("{id}"), ActionName("GetAccountByIdAsync")]
        public async Task<ActionResult<AccountReadDto>> GetAccountByIdAsync(int id)
        {
            Account account = await _accountRepositoryConnection.Repository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(_autoMapper.Map<AccountReadDto>(account));
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<AccountUpdateDto>> UpdateAccountAsync([FromBody] AccountUpdateDto accountUpdateDto, int id)
        {
            Account account = _autoMapper.Map<Account>(accountUpdateDto);

            await _accountRepositoryConnection.Repository.UpdateAccountAsync(account, id);

            return Ok(account);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAccountAsync(int id)
        {
            await _accountRepositoryConnection.Repository.DeleteAccountAsync(id);

            return NoContent();
        }
    }
}
