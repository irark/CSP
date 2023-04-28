namespace CSP.Models.CSP
{
    public class Variable
    {
        public Variable(int id, Time time, Group group)
        {
            Time = time;
            Id = id;
            Group = group;
        }

        public int Id { get; set; }
        public Time Time { get; set; }
        public Group Group { get; set; }
        
        public static bool operator <(Variable first, Variable second)
        {
            return first.Id < second.Id;
        }

        public static bool operator >(Variable first, Variable second)
        {
            return first.Id > second.Id;
        }
    }
}