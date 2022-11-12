using ToDoList.WebAPI.Models;

namespace ToDoList.WebAPI.Contracts
{
    public class GetOpenLoopsResponse
    {
        public OpenLoop[]? OpenCases { get; set; }
    }
}