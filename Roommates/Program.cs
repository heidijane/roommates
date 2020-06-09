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
                    //LIST ALL THE ROOMS//
                    case 1:
                        List<Room> allRooms = roomRepo.GetAll();
                        RoomUtils.ListAllRooms(allRooms);
                        break;

                    //ADD A NEW ROOM//
                    case 2:
                        Console.WriteLine("Add a Room");
                        Console.WriteLine("-----------------------------");
                        Console.Write("Name of Room> ");
                        string newRoomName = Console.ReadLine();
                        while( newRoomName != "")
                        {
                            //max room occupancy
                            int newRoomOccupancy = 0;
                            Console.Write("How many people can live in this room? ");
                            string newRoomOccupancyInput = Console.ReadLine();

                            while (newRoomOccupancy == 0)
                            {
                                //make sure that max occupancy is an integer
                                while (!NumberUtils.isInt(newRoomOccupancyInput))
                                {
                                    Console.WriteLine("Max occupancy has to be an integer. Please try again...");
                                    Console.WriteLine();
                                    Console.Write("How many people can live in this room? ");
                                    newRoomOccupancyInput = Console.ReadLine();
                                }

                                //make sure number is at least one
                                while (!NumberUtils.isBetween(Int32.Parse(newRoomOccupancyInput), 1))
                                {
                                    Console.WriteLine();
                                    Console.Write("Max occupancy has to be greater than 0. ");
                                    Console.WriteLine();
                                    Console.Write("How many people can live in this room? ");
                                    newRoomOccupancyInput = Console.ReadLine();
                                }

                                newRoomOccupancy = Int32.Parse(newRoomOccupancyInput);
                            }

                            //make the new room object
                            Room newRoom = new Room()
                            {
                                Name = newRoomName,
                                MaxOccupancy = newRoomOccupancy
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

                    //EDIT ROOM//
                    case 3:
                        Console.WriteLine("Edit a Room");
                        Console.WriteLine("-----------------------------");
                        List<Room> editRooms = roomRepo.GetAll();
                        Room editRoom = null;
                        foreach (Room room in editRooms)
                        {
                            Console.WriteLine(room);
                        }

                        Console.WriteLine();
                        Console.WriteLine("ID of the room you want to edit: ");
                        string choice = Console.ReadLine();
                        while (editRoom == null)
                        {
                            while (!NumberUtils.isInt(choice))
                            {
                                Console.WriteLine("Invalid Selection. Please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to edit: ");
                                choice = Console.ReadLine();
                            }
                            while (!RoomUtils.isValidRoom(int.Parse(choice), editRooms))
                            {
                                Console.WriteLine("No matching ID found, please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to edit: ");
                                choice = Console.ReadLine();
                            }
                            editRoom = RoomUtils.findRoom(int.Parse(choice), editRooms);
                            
                        }
                        //room ID is valid, now let user input new data
                        Console.WriteLine($"Editing {editRoom.Name}");
                        Console.WriteLine();

                        Console.Write("Room Name: ");
                        string newName = Console.ReadLine();

                        //if user leaves blank, don't change it
                        if (newName == "")
                        {
                            newName = editRoom.Name;
                        }

                        //Edit max room occupancy
                        int newOccupancy = 0;
                        Console.Write("How many people can live in this room? ");
                        string newOccupancyInput = Console.ReadLine();

                        //if user leaves blank, don't change it
                        if (newOccupancyInput == "")
                        {
                            newOccupancy = editRoom.MaxOccupancy;
                        }

                        while (newOccupancy == 0)
                        {
                            //make sure that max occupancy is an integer
                            while (!NumberUtils.isInt(newOccupancyInput))
                            {
                                Console.WriteLine("Max occupancy has to be an integer. Please try again...");
                                Console.WriteLine();
                                Console.Write("How many people can live in this room? ");
                                newOccupancyInput = Console.ReadLine();
                            }

                            //make sure number is at least one
                            while (!NumberUtils.isBetween(Int32.Parse(newOccupancyInput), 1))
                            {
                                Console.WriteLine();
                                Console.Write("Max occupancy has to be greater than 0. ");
                                Console.WriteLine();
                                Console.Write("How many people can live in this room? ");
                                newOccupancyInput = Console.ReadLine();
                            }

                            newOccupancy = Int32.Parse(newOccupancyInput);
                        }

                        //everything checks out, create a new room object and update the database
                        editRoom.Name = newName;
                        editRoom.MaxOccupancy = newOccupancy;

                        roomRepo.Update(editRoom);
                        Console.WriteLine("Room Updated!");

                        break;
                    //DELETE ROOM//
                    case 4:
                        Console.WriteLine("Delete a Room");
                        Console.WriteLine("-----------------------------");
                        List<Room> deleteRooms = roomRepo.GetAll();
                        Room deleteRoom = null;
                        foreach (Room room in deleteRooms)
                        {
                            Console.WriteLine(room);
                        }

                        Console.WriteLine();
                        Console.WriteLine("ID of the room you want to delete: ");
                        string deleteChoice = Console.ReadLine();
                        while (deleteRoom == null)
                        {
                            while (!NumberUtils.isInt(deleteChoice))
                            {
                                Console.WriteLine("Invalid Selection. Please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to delete: ");
                                deleteChoice = Console.ReadLine();
                            }
                            while (!RoomUtils.isValidRoom(int.Parse(deleteChoice), deleteRooms))
                            {
                                Console.WriteLine("No matching ID found, please try again.");
                                Console.WriteLine();
                                Console.WriteLine("ID of the room you want to delete: ");
                                deleteChoice = Console.ReadLine();
                            }
                            deleteRoom = RoomUtils.findRoom(int.Parse(deleteChoice), deleteRooms);

                        }

                        //room is valid, confirm deletion
                        Console.Write($"Are you sure that you want to delete the {deleteRoom.Name} (y/n)? ");
                        string confirm = Console.ReadLine();
                        if (confirm == "y")
                        {
                            //go ahead and delete the room
                            roomRepo.Delete(deleteRoom.Id);
                            Console.WriteLine("Room was deleted!");
                            break;
                        } 
                        else
                        {
                            Console.WriteLine("Cancelling...");
                        }

                        break;
                    //LIST ALL ROOMMATES//
                    case 5:
                        List<Roommate> allRoommates = roommateRepo.GetAllWithRoom();
                        Console.WriteLine("All Roommmates:");
                        foreach (Roommate roommate in allRoommates)
                        {
                            Console.WriteLine(roommate);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Add a Roommate");
                        Console.WriteLine("-----------------------------");
                        Console.Write("First Name: ");
                        string newFirstName = Console.ReadLine();
                        while (newFirstName != "")
                        {

                            Console.Write("Last Name: ");
                            string newLastName = Console.ReadLine();
                            while (newLastName == "")
                            {
                                Console.WriteLine();
                                Console.WriteLine("New roommate must have a last name.");
                                Console.WriteLine();
                                Console.Write("Last Name: ");
                                newLastName = Console.ReadLine();
                            }

                            Console.Write("Rent Portion:  ");
                            int rentPortion = Int32.Parse(Console.ReadLine());
                            while (rentPortion < 0)
                            {
                                Console.WriteLine();
                                Console.Write("Rent portion has to be an integer. ");
                                Console.WriteLine();
                                Console.Write("Rent Portion: ");
                                rentPortion = Int32.Parse(Console.ReadLine());
                            }

                            List<Room> roomOptions = roomRepo.GetAll();
                            foreach (Room room in roomOptions)
                            {
                                Console.WriteLine(room);
                            }
                            Room newRoom = null;
                            int newRoomId;
                            Console.WriteLine();
                            Console.WriteLine($"ID of the room {newFirstName} lives in: ");
                            string roomChoice = Console.ReadLine();
                            while (newRoom == null)
                            {
                                try
                                {
                                    newRoomId = int.Parse(roomChoice);
                                }
                                catch
                                {
                                    Console.WriteLine("Invalid Selection. Please try again.");
                                    Console.WriteLine();
                                    Console.WriteLine($"ID of the room {newFirstName} lives in: ");
                                    deleteChoice = Console.ReadLine();
                                }
                                newRoomId = int.Parse(roomChoice);
                                try
                                {
                                    newRoom = roomOptions.First(room => room.Id == newRoomId);
                                }
                                catch
                                {
                                    Console.WriteLine("No matching ID found, please try again.");
                                    Console.WriteLine();
                                    Console.WriteLine($"ID of the room {newFirstName} lives in: ");
                                    roomChoice = Console.ReadLine();
                                }
                            }

                            //make the new roommate object
                            Roommate newRoommate = new Roommate()
                            {
                                Firstname = newFirstName,
                                Lastname = newLastName,
                                RentPortion = rentPortion,
                                MovedInDate = DateTime.Now,
                                Room = newRoom
                            };

                            try
                            {
                                roommateRepo.Insert(newRoommate);
                                Console.WriteLine("New roommate has been added!");
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("New room could not be inserted, please try again.");
                                break;
                            }

                        }

                           
                        break;
                    //EDIT ROOMMATE//
                    case 7:
                        break;
                    //DELETE ROOMMATE//
                    case 8:
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

            while (selection < 0 || selection > 8)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine();

                Console.WriteLine("Main Menu");
                Console.WriteLine(" 1) List All Rooms");
                Console.WriteLine(" 2) Add a Room");
                Console.WriteLine(" 3) Edit a Room");
                Console.WriteLine(" 4) Delete a Room");
                Console.WriteLine("-----------------------------");
                Console.WriteLine(" 5) List All Roommmates");
                Console.WriteLine(" 6) Add a Roommate");
                Console.WriteLine(" 7) Edit a Roommate");
                Console.WriteLine(" 8) Delete a Roommate");
                Console.WriteLine("-----------------------------");
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