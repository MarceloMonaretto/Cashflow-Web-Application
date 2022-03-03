using AccountRepositoryLib.Connection;
using AccountInformationAPI.Dtos;
using AccountModelsLib.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AccountInformationAPI.Controllers
{
    [ApiController]
    [Route("accountapi/[controller]")]
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
        public async Task<ActionResult<AccountReadDto>> CreateCurrencyAsync([FromBody] AccountCreateDto accountDto)
        {
            var currency = _autoMapper.Map<AccountModel>(accountDto);

            if (currency == null)
            {
                return BadRequest(accountDto);
            }

            await _accountRepositoryConnection.Repository.CreateAccountAsync(currency);

            var currencyReadDto = _autoMapper.Map<AccountReadDto>(currency);

            return CreatedAtAction(nameof(GetAccountByIdAsync), new { currencyReadDto.Id }, currencyReadDto);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<AccountReadDto>> GetCurrenciesAsync()
        {
            var accounts = await _accountRepositoryConnection.Repository.GetAllAccountsAsync();

            return _autoMapper.Map<IEnumerable<AccountReadDto>>(accounts);
        }

        [HttpGet("{id}"), ActionName("GetAccountByIdAsync")]
        public async Task<ActionResult<AccountReadDto>> GetAccountByIdAsync(int id)
        {
            AccountModel account = await _accountRepositoryConnection.Repository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(_autoMapper.Map<AccountReadDto>(account));
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<AccountUpdateDto>> UpdateCurrencyAsync([FromBody] AccountUpdateDto accountUpdateDto, int id)
        {
            AccountModel account = _autoMapper.Map<AccountModel>(accountUpdateDto);

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
