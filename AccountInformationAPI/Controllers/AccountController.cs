using AccountInformationAPI.Data;
using AccountInformationAPI.Dtos;
using AccountInformationAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AccountInformationAPI.Controllers
{
    [ApiController]
    [Route("accountapi/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepo _repo;

        public AccountController(IMapper mapper, IAccountRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<AccountReadDto>> CreateCurrencyAsync([FromBody] AccountCreateDto accountDto)
        {
            var currency = _mapper.Map<AccountModel>(accountDto);

            if (currency == null)
            {
                return BadRequest(accountDto);
            }

            await _repo.CreateAccountAsync(currency);
            _repo.SaveChanges();

            var currencyReadDto = _mapper.Map<AccountReadDto>(currency);

            return CreatedAtAction(nameof(GetAccountByIdAsync), new { currencyReadDto.Id }, currencyReadDto);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<AccountReadDto>> GetCurrenciesAsync()
        {
            var accounts = await _repo.GetAllAccountsAsync();

            return _mapper.Map<IEnumerable<AccountReadDto>>(accounts);
        }

        [HttpGet("{id}"), ActionName("GetAccountByIdAsync")]
        public async Task<ActionResult<AccountReadDto>> GetAccountByIdAsync(int id)
        {
            AccountModel account = await _repo.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccountReadDto>(account));
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<AccountUpdateDto>> UpdateCurrencyAsync([FromBody] AccountUpdateDto accountUpdateDto, int id)
        {
            AccountModel account = _mapper.Map<AccountModel>(accountUpdateDto);

            await _repo.UpdateAccountAsync(account, id);
            _repo.SaveChanges();

            return Ok(account);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAccountAsync(int id)
        {
            await _repo.DeleteAccountAsync(id);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}
