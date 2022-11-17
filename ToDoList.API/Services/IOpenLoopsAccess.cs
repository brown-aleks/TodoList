using ToDoList.API.Models;

namespace ToDoList.API.Services
{
    public interface IOpenLoopsAccess
    {
        Task<Guid> AddAsync(OpenLoop newOpenCases);
        Task<Guid> DeleteAsync(Guid id);
        OpenLoop[] Get();
        OpenLoop? Get(Guid guid);
        Task<Guid> UpdateAsync(OpenLoop openLoop);
    }
}