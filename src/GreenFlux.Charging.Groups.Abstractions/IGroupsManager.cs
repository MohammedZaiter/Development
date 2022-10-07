
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Threading.Tasks;

    public interface IGroupsManager
    {
        Task<ReturnResult<Group>> GetGroupById(Guid id);

        Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options);

        Task<ReturnResult> UpdateGroup(Guid id, CreateOrUpdateGroupOptions options);

        Task<ReturnResult> RemoveGroup(Guid id);
    }
}
