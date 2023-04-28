using System.Collections.Generic;

namespace CSP.Models
{
    public class Schedule
    {
        public Schedule()
        {
            ScheduleList = new();
        }
        public Dictionary<Group, Dictionary<string, List<Class>>> ScheduleList { get; set; }
    }
}