
namespace GreenFlux.Charging.Groups
{
    using System;
    using System.Threading.Tasks;

    public interface IGroupsStore
    {
        Task<Group> GetGroup(Guid id);

        Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options);

        Task UpdateGroup(Guid groupId, CreateOrUpdateGroupOptions options);

        Task RemoveGroup(Guid groupId);
    }
}
