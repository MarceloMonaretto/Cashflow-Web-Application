    using AccountInformationAPI.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.ContextRepositoryClasses;
using RoleRepositoryLib.Connection;

namespace AccountInformationAPI.Controllers
{
    [ApiController]
    [Route("cashflowapi/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMapper _autoMapper;
        private readonly IRoleRepositoryConnection _roleRepositoryConnection;

        public RoleController(IMapper mapper, IRoleRepositoryConnection repo)
        {
            _autoMapper = mapper;
            _roleRepositoryConnection = repo;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<RoleReadDto>> CreateRoleAsync([FromBody] RoleCreateDto roleDto)
        {
            var role = _autoMapper.Map<Role>(roleDto);

            if (role == null)
            {
                return BadRequest(roleDto);
            }

            await _roleRepositoryConnection.Repository.CreateRoleAsync(role);

            var roleReadDto = _autoMapper.Map<RoleReadDto>(role);

            return NoContent();
        }

        [HttpGet("{roleName}"), ActionName("GetRoleByNameAsync")]
        public async Task<ActionResult<AccountReadDto>> GetRoleByNameAsync(string roleName)
        {
            Role role = await _roleRepositoryConnection.Repository.GetRoleByNameAsync(roleName);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(_autoMapper.Map<RoleReadDto>(role));
        }

        [HttpPut("Update/{roleName}")]
        public async Task<ActionResult<AccountUpdateDto>> UpdateRoleAsync([FromBody] AccountUpdateDto roleUpdatedDto)
        {
            Role role = _autoMapper.Map<Role>(roleUpdatedDto);

            await _roleRepositoryConnection.Repository.UpdateRoleAsync(role);

            return Ok(role);
        }

        [HttpDelete("Delete/{roleName}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleName)
        {
            await _roleRepositoryConnection.Repository.DeleteRoleByNameAsync(roleName);

            return NoContent();
        }

        [HttpGet("All")]
        public async Task<IEnumerable<RoleReadDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepositoryConnection.Repository.GetAllRolesAsync();

            return _autoMapper.Map<IEnumerable<RoleReadDto>>(roles);
        }
    }
}
