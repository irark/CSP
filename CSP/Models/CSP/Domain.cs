namespace CSP.Models.CSP
{
    public class Domain
    {
        public Domain(Subject subject, Teacher teacher, Room room)
        {
            Subject = subject;
            Teacher = teacher;
            Room = room;
        }

        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public Room Room { get; set; }
    }
}