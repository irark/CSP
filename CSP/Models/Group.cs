using System.Collections.Generic;

namespace CSP.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Subject> Subjects { get; set; } = new();
        public int Size { get; set; }
        private static int _counter;
        public Group()
        {
            Id = _counter++;
        }

        public Group(string name, List<Subject> subjects, int size) : base()
        {
            Name = name;
            Subjects = subjects;
            Size = size;
        }
        public static bool operator==(Group first, Group second)
        {
            return first.Id == second.Id;
        }

        public static bool operator !=(Group first, Group second)
        {
            return !(first == second);
        }

        public static bool operator <(Group first, Group second)
        {
            return first.Id < second.Id;
        }

        public static bool operator >(Group first, Group second)
        {
            return first.Id > second.Id;
        }
    }
}