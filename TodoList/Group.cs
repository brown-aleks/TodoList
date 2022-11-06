using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "NoName Group";
        public string Description { get; set; } = "No Description";
        public List<Guid> UsrsId { get; set; } = new List<Guid>();
        public List<string> ActionsKey { get; set; } = new List<string>();
    }
}
