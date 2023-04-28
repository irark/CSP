using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP.Models.CSP
{
    public class CSPSolver
    {
        private List<Variable> _variablesStorage = new();
        private HashSet<Variable> _freeVariables = new();
        private List<Domain> _domainsStorage = new();
        private Dictionary<Variable, List<Domain>> _domains = new();
        private Dictionary<Variable, List<Domain>> _tmp = new();
        private Dictionary<Variable, List<Variable>> _variableNeighbours = new();
        public Method Method { get; set; }

        public CSPSolver(Method method)
        {
            Method = method;
        }

        public void SetVariables(List<Group> groups, List<string> days, int lessons_per_day)
        {
            var num = 0;
            foreach (var day in days)
            {
                for (var lesson = 1; lesson <= lessons_per_day; ++lesson)
                {
                    var start = num;
                    foreach (var newVariable in groups.Select(group => new Variable(num, new Time(day, lesson), group)))
                    {
                        _variablesStorage.Add(newVariable);
                        _freeVariables.Add(newVariable);
                        ++num;
                    }

                    var finish = num - 1;
                    for (var i = start; i <= finish; i++)
                    {
                        for (var j = start; j <= finish; j++)
                            if (i != j)
                            {
                                if (!_variableNeighbours.ContainsKey(_variablesStorage[i]))
                                    _variableNeighbours.Add(_variablesStorage[i], new());
                                _variableNeighbours[_variablesStorage[i]].Add(_variablesStorage[j]);
                            }
                    }
                }
            }
        }

        public void SetDomains(List<Room> rooms)
        {
            foreach (var variable in _variablesStorage)
            {
                foreach (var subject in variable.Group.Subjects)
                {
                    foreach (var teacher in subject.Teachers)
                    {
                        foreach (var newDomain in rooms.Select(room => new Domain(subject, teacher, room)))
                        {
                            _domainsStorage.Add(newDomain);
                            if (!_domains.ContainsKey(variable))
                                _domains.Add(variable, new());
                            _domains[variable].Add(newDomain);
                        }
                    }

                    Shuffle(_domains[variable]);
                }
            }
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

        public Schedule ConvertToSchedule()
        {
            var schedule = new Schedule();
            foreach (var (variable, domain) in _answer)
            {
                var num = variable.Time.Number;
                if (!schedule.ScheduleList.ContainsKey(variable.Group))
                    schedule.ScheduleList.Add(variable.Group, new());
                if (!schedule.ScheduleList[variable.Group].ContainsKey(variable.Time.Day))
                    schedule.ScheduleList[variable.Group].Add(variable.Time.Day, new());
                var sz = schedule.ScheduleList[variable.Group][variable.Time.Day].Count;
                if (num > sz)
                {
                    if (num > schedule.ScheduleList[variable.Group][variable.Time.Day].Capacity)
                        schedule.ScheduleList[variable.Group][variable.Time.Day].Capacity = num;
                    schedule.ScheduleList[variable.Group][variable.Time.Day]
                        .AddRange(Enumerable.Repeat(new Class(), num - sz));
                }

                schedule.ScheduleList[variable.Group][variable.Time.Day][num - 1] = new Class(variable.Group,
                    domain.Subject, domain.Room, domain.Teacher, variable.Time);
            }

            return schedule;
        }

        public Schedule Solve()
        {
            Backtracking();
            return ConvertToSchedule();
        }

        private Dictionary<Variable, Domain> _answer = new();
        private Dictionary<Variable, Domain> _current = new();

        private bool Backtracking()
        {
            if (_current.Count == _variablesStorage.Count)
            {
                _answer = _current;
                return true;
            }

            var variable = Method == Method.MRVHeuristic ? MRVHeuristic() : DegreeHeuristic();
            _freeVariables.RemoveWhere(i => i == variable);
            foreach (var domain in _domains[variable])
            {
                if (CheckConstraints(_current, variable, domain))
                {
                    _current[variable] = domain;
                    ForwardChecking(variable, domain);
                    var res = Backtracking();
                    if (res)
                        return true;
                    _current.Remove(variable);
                    UnRemoveInconsistentDomains(variable);
                }
            }

            _freeVariables.Add(variable);
            return false;
        }

        private Variable DegreeHeuristic()
        {
            var max = Int32.MinValue;
            Variable res = null;
            foreach (var variable in _freeVariables)
            {
                var cnt = 0;
                if (max >= _variableNeighbours[variable].Count)
                    continue;
                var k = 0;
                foreach (var variable2 in _variableNeighbours[variable])
                {
                    ++k;
                    if (!_current.ContainsKey(variable2))
                        ++cnt;
                    if (cnt + _variableNeighbours[variable].Count - k <= max)
                        break;
                }

                if (cnt <= max) continue;
                res = variable;
                max = cnt;
            }

            return res;
        }

        private Variable MRVHeuristic()
        {
            var min = Int32.MaxValue;
            Variable res = null;
            foreach (var variable in _freeVariables)
            {
                var cnt = _domains[variable].Count;
                if (cnt >= min) continue;
                res = variable;
                min = cnt;
            }

            return res;
        }

        private void ForwardChecking(Variable variable, Domain domain)
        {
            RemoveInconsistentDomains(variable, domain);
        }

        private void RemoveInconsistentDomains(Variable variable, Domain domain)
        {
            foreach (var variable2 in _variableNeighbours[variable])
            {
                if (!_current.ContainsKey(variable2))
                {
                    for (int i = 0; i < _domains[variable2].Count;)
                    {
                        var domain2 = _domains[variable2][i];
                        if (domain.Teacher == domain2.Teacher || domain.Room == domain2.Room)
                        {
                            var last = _domains[variable2][_domains[variable2].Count - 1];
                            (_domains[variable2][i], last) = (
                                last, _domains[variable2][i]);
                            if (!_tmp.ContainsKey(variable2))
                                _tmp.Add(variable2, new());
                            _tmp[variable2].Add(last);
                            _domains[variable2].RemoveAt(_domains[variable2].Count - 1);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
        }

        private void UnRemoveInconsistentDomains(Variable variable)
        {
            foreach (var variable2 in _variableNeighbours[variable])
            {
                for (int i = 0; i < _tmp[variable2].Count; i++)
                {
                    _domains[variable2].Add(_tmp[variable2][i]);
                }
            }
        }

        private bool CheckConstraints(Dictionary<Variable, Domain> current)
        {
            foreach (var (variable1, domain) in current)
            {
                foreach (var variable2 in _variableNeighbours[variable1])
                {
                    if (current.ContainsKey(variable2))
                    {
                        var domain2 = current[variable2];
                        if (domain.Teacher == domain2.Teacher || domain.Room == domain2.Room)
                            return false;
                    }
                }
            }

            return true;
        }

        private bool CheckConstraints(Dictionary<Variable, Domain> current, Variable variable, Domain domain)
        {
            foreach (var variable2 in _variableNeighbours[variable])
            {
                if (current.ContainsKey(variable2))
                {
                    var domain2 = current[variable2];
                    if (domain.Teacher == domain2.Teacher || domain.Room == domain2.Room)
                        return false;
                }
            }


            return true;
        }
    }
}