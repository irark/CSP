using System;
using System.Collections.Generic;
using CSP.Models;
using CSP.Models.CSP;
using Microsoft.AspNetCore.Components;

namespace CSP.Pages
{
    public partial class Index : ComponentBase
    {
        public List<string> Days { get; set; } = new() {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};

        public List<Teacher> Teachers { get; set; } = new();

        public List<Room> Rooms { get; set; } = new();

        public List<Subject> Subjects { get; set; } = new();
        public List<Group> Groups { get; set; } = new();
        public Dictionary<Group, Dictionary<string, List<Class>>> ScheduleList { get; set; } = new();

        public int TeachersPerSubject { get; set; } = 4;
        public int SubjectsPerGroup { get; set; } = 7;
        public int LessonsPerDay { get; set; } = 3;
        public Method Method { get; set; }
        protected override void OnInitialized()
        {
        }

        private void Solve()
        {
            
            Teachers = new()
            {
                new Teacher("Vergunova"),
                new Teacher("Trokhymchuk"),
                new Teacher("Koval"),
                new Teacher("Fedorus"),
                new Teacher("Hlybovets"),
                new Teacher("Krak"),
                new Teacher("Pashko"),
                new Teacher("Anikushin"),
            };
            Rooms = new()
            {
                new Room(215, 30),
                new Room(505, 35),
                new Room(42, 27),
                new Room(39, 100),
                new Room(303, 25),
                new Room(27, 35),
            };
            Subjects = new()
            {
                new Subject("Information technologies of management", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Pattern recognition and scene analysis", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Operating systems with time sharing", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Intelligent systems", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Problems of ai", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Natural human-computer interfaces", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Neural networks and neurocomputation", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Databases", GetRandomTeacher(TeachersPerSubject)),
                new Subject("Mathematical analysis", GetRandomTeacher(TeachersPerSubject)),
            };
            Groups = new()
            {
                new Group("TK-41", GetRandomSubject(SubjectsPerGroup), 20),
                new Group("TK-42", GetRandomSubject(SubjectsPerGroup), 30),
                new Group("TTP-41", GetRandomSubject(SubjectsPerGroup), 26)
            };
            
            var csp = new CSPSolver(Method.MRVHeuristic);
            csp.SetVariables(Groups, Days, LessonsPerDay);
            csp.SetDomains(Rooms);
           ScheduleList = csp.Solve().ScheduleList ?? new Dictionary<Group, Dictionary<string, List<Class>>>();
        }

        public List<Teacher> GetRandomTeacher(int len)
        {
            var res = new List<Teacher>();
            var random = new Random();
            for (int i = 0; i < len; )
            {
                var newTeacher = Teachers[random.Next(Teachers.Count)];
                if(res.Contains(newTeacher))
                    continue;
                res.Add(newTeacher);
                i++;
            }

            Shuffle(res);

            return res;
        }

        public List<Subject> GetRandomSubject(int len)
        {
            var res = new List<Subject>();
            var random = new Random();
            for (int i = 0; i < len; )
            {
                var newSubject = Subjects[random.Next(Subjects.Count)];
                if(res.Contains(newSubject))
                    continue;
                res.Add(newSubject);
                i++;
            }

            Shuffle(res);
            return res;
        }
        
        private static Random rng = new Random();

        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}