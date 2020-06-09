using System;
using System.Collections.Generic;
using System.Linq;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            Console.WriteLine("Roommate Manager");

            while (true)
            {
                int selection = Menu();
                switch (selection)
                {
                    case 1:
                        List<Room> allRooms = roomRepo.GetAll();
                        Console.WriteLine("All Rooms:");
                        foreach(Room room in allRooms)
                        {
                            Console.WriteLine(room);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Add a Room");
                        Console.WriteLine("-----------------------------");
                        Console.Write("Name of Room> ");
                        string newRoomName = Console.ReadLine();
                        while( newRoomName != "")
                        {
                            Console.Write("How many people can live in this room? ");
                            int maxOInput = Int32.Parse(Console.ReadLine());
                            while (maxOInput < 1)
                            {
                                Console.WriteLine();
                                Console.Write("Max occupancy has to be greater than 0. ");
                                Console.WriteLine();
                                Console.Write("How many people can live in this room? ");
                                maxOInput = Int32.Parse(Console.ReadLine());
                            }

                            //make the new room object
                            Room newRoom = new Room()
                            {
                                Name = newRoomName,
                                MaxOccupancy = maxOInput
                            };

                            try
                            {
                                roomRepo.Insert(newRoom);
                                Console.WriteLine("New room has been added!");
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("New room could not be inserted, please try again.");
                                break;
                            }
                            
                        }

                        break;
                    case 3:
                        Console.WriteLine("Edit a Room");
                        Console.WriteLine("-----------------------------");
                        List<Room> editRooms = roomRepo.GetAll();
                        Room editRoom = null;
                        foreach (Room room in editRooms)
                        {
                            Console.WriteLine(room);
                        }

                        int roomId;
                        Console.WriteLine();
                        Console.WriteLine("ID of the room you want to edit: ");
                        string choice = Console.ReadLine();
                        while (editRoom == null)
                        {
                            try
                            {
                                roomId = int.Parse(choice);
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Selection. Please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to edit: ");
                                choice = Console.ReadLine();
                            }
                            roomId = int.Parse(choice);
                            try
                            {
                                editRoom = editRooms.First(room => room.Id == roomId);
                            }
                            catch
                            {
                                Console.WriteLine("No matching ID found, please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to edit: ");
                                choice = Console.ReadLine();
                            }            

                            
                        }
                        //Id checks out, now edit the room
                        Console.WriteLine($"Editing {editRoom.Name}");
                        Console.WriteLine();
                        Console.Write("Room Name: ");
                        string newName = Console.ReadLine();
                        //if user leaves blank, don't change it
                        if (newName == "")
                        {
                            newName = editRoom.Name;
                        }

                        Console.Write("How many people can live in this room? ");
                        int newOccupancy = Int32.Parse(Console.ReadLine());
                        while (newOccupancy < 1)
                        {
                            Console.WriteLine();
                            Console.Write("Max occupancy has to be greater than 0. ");
                            Console.WriteLine();
                            Console.Write("How many people can live in this room? ");
                            newOccupancy = Int32.Parse(Console.ReadLine());
                        }

                        //everything checks out, create a new room object and update the database
                        editRoom.Name = newName;
                        editRoom.MaxOccupancy = newOccupancy;

                        roomRepo.Update(editRoom);
                        Console.WriteLine("Room Updated!");

                        break;
                    case 0:
                        Console.WriteLine("Thank you for using Roommate Manager!");
                        return;
                    default:
                        throw new Exception("Something went wrong...invalid selection");
                }
            }

        }

        static int Menu()
        {
            int selection = -1;

            while (selection < 0 || selection > 4)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine();

                Console.WriteLine("Main Menu");
                Console.WriteLine(" 1) List All Rooms");
                Console.WriteLine(" 2) Add a Room");
                Console.WriteLine(" 3) Edit a Room");
                Console.WriteLine(" 4) Delete a Room");
                Console.WriteLine(" 0) Exit");

                Console.Write("> ");
                string choice = Console.ReadLine();
                try
                {
                    selection = int.Parse(choice);
                }
                catch
                {
                    Console.WriteLine("Invalid Selection. Please try again.");
                }
                Console.WriteLine();
            }

            return selection;
        }
    }
}