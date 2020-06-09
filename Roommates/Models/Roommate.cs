using System;

namespace Roommates.Models
{
    // C# representation of the Roommate table
    public class Roommate
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int RentPortion { get; set; }
        public DateTime MovedInDate { get; set; }
        public Room Room { get; set; }
        public override string ToString()
        {
            return $@"
---------------------------------
{Id}) {Firstname} {Lastname}
Rent Portion: {RentPortion}
Moved In: {MovedInDate}
Lives in the {Room.Name}
";
        }
    }
}