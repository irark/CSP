namespace CSP.Models
{
    public class Time
    {
        public Time(string day, int number)
        {
            Day = day;
            Number = number;
        }

        public string Day { get; set; }
        public int Number { get; set; }
        public static bool operator==(Time first, Time second)
        {
            return first.Number == second.Number && first.Day == second.Day;
        }

        public static bool operator !=(Time first, Time second)
        {
            return !(first == second);
        }

    }
}