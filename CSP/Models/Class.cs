namespace CSP.Models
{
    public class Class
    {
        public Class(){}
        public Class(Group group, Subject subject, Room room, Teacher teacher, Time time)
        {
            Group = group;
            Subject = subject;
            Room = room;
            Teacher = teacher;
            Time = time;
        }

        public Group Group { get; set; }
        public Subject Subject { get; set; }
        public Room Room { get; set; }
        public Teacher Teacher { get; set; }
        public Time Time { get; set; }
        
    }
}