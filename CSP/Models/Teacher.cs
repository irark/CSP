namespace CSP.Models
{
    public class Teacher
    {
        public string Name { get; set; }

        public Teacher(string name)
        {
            Name = name;
        }
        public static bool operator==(Teacher first, Teacher second)
        {
            return first.Name == second.Name;
        }

        public static bool operator !=(Teacher first, Teacher second)
        {
            return !(first == second);
        }
    }
}