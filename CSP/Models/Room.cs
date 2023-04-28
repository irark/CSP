namespace CSP.Models
{
    public class Room
    {
        public int Number { get; set; }
        public int RoomCapacity { get; set; }

        public Room(int number, int capacity)
        {
            Number = number;
            RoomCapacity = capacity;
        }

        public static bool operator==(Room first, Room second)
        {
            return first.RoomCapacity == second.RoomCapacity && first.Number == second.Number;
        }

        public static bool operator !=(Room first, Room second)
        {
            return !(first == second);
        }
    }
}