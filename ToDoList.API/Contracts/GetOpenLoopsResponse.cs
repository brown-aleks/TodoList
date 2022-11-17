using ToDoList.API.Models;

namespace ToDoList.API.Contracts
{
    public class GetOpenLoopsResponse
    {
        public OpenLoop[]? OpenCases { get; set; }
    }
}