using RoleRepositoryLib.Repositories;

namespace RoleRepositoryLib.Connection
{
    public interface IRoleRepositoryConnection
    {
        IRoleRepository Repository { get; }
    }
}