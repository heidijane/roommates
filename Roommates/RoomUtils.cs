using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates.Models
{
    public class RoomUtils
    {
        
        public static void ListAllRooms(List<Room> rooms)
        {
            Console.WriteLine("All Rooms:");
            foreach (Room room in rooms)
            {
                Console.WriteLine(room);
            }
        }

        public static bool isValidRoom(int roomId, List<Room> rooms)
        {
            try
            {
                Room tryRoom = rooms.First(room => room.Id == roomId);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Room findRoom(int roomId, List<Room> rooms)
        {
            return rooms.First(room => room.Id == roomId);
        }
        
    }
}
