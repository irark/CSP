using System.Collections.Generic;

namespace CSP.Models
{
    public class Subject
    {
        public string Name { get; set; }
        public List<Teacher> Teachers { get; set; } = new();

        public Subject(string name, List<Teacher> teachers)
        {
            Name = name;
            Teachers = teachers;
        }
    }
}