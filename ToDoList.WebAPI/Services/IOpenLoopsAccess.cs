using ToDoList.WebAPI.Models;

namespace ToDoList.WebAPI.Services
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