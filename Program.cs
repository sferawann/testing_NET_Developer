using System;
using System.Collections.Generic;

namespace SistemParkir
{
    class Program
    {
        static List<ParkingLot> parkingLots;

        static void Main(string[] args)
        {
            parkingLots = new List<ParkingLot>();

            Console.WriteLine("Welcome to the Parking System!");

            while (true)
            {
                Console.WriteLine("Please enter a command:");
                string command = Console.ReadLine();

                string[] commandParts = command.Split(' ');
                string action = commandParts[0];

                switch (action.ToLower())
                {
                    case "create_parking_lot":
                        int lotCount = Convert.ToInt32(commandParts[1]);
                        CreateParkingLot(lotCount);
                        break;
                    case "park":
                        string registrationNumber = commandParts[1];
                        string color = commandParts[2];
                        string vehicleType = commandParts[3];
                        ParkVehicle(registrationNumber, color, vehicleType);
                        break;
                    case "leave":
                        int slotNumber = Convert.ToInt32(commandParts[1]);
                        LeaveParkingLot(slotNumber);
                        break;
                    case "status":
                        PrintParkingLotStatus();
                        break;
                    case "type_of_vehicles":
                        string vehicleTypeFilter = commandParts[1];
                        PrintVehicleCountByType(vehicleTypeFilter);
                        break;
                    case "registration_numbers_for_vehicles_with_odd_plate":
                        PrintVehiclesWithOddPlate();
                        break;
                    case "registration_numbers_for_vehicles_with_event_plate":
                        PrintVehiclesWithEvenPlate();
                        break;
                    case "registration_numbers_for_vehicles_with_colour":
                        string colorFilter = commandParts[1];
                        PrintVehiclesWithColor(colorFilter);
                        break;
                    case "slot_numbers_for_vehicles_with_colour":
                        string colorFilter2 = commandParts[1];
                        PrintSlotNumbersWithColor(colorFilter2);
                        break;
                    case "slot_number_for_registration_number":
                        string registrationNumberFilter = commandParts[1];
                        PrintSlotNumberWithRegistrationNumber(registrationNumberFilter);
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }

        static void CreateParkingLot(int lotCount)
        {
            for (int i = 1; i <= lotCount; i++)
            {
                parkingLots.Add(new ParkingLot(i));
            }

            Console.WriteLine($"Created a parking lot with {lotCount} slots");
        }

        static void ParkVehicle(string registrationNumber, string color, string vehicleType)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            Vehicle vehicle = new Vehicle(registrationNumber, color, vehicleType);

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsAvailable)
                {
                    lot.Park(vehicle);
                    Console.WriteLine($"Allocated slot number: {lot.Number}");
                    return;
                }
            }

            Console.WriteLine("Sorry, parking lot is full");
        }

        static void LeaveParkingLot(int slotNumber)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            ParkingLot lot = parkingLots.Find(l => l.Number == slotNumber);

            if (lot != null)
            {
                lot.Leave();
                Console.WriteLine($"Slot number {slotNumber} is free");
            }
            else
            {
                Console.WriteLine("Invalid slot number");
            }
        }

        static void PrintParkingLotStatus()
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();
                    Console.WriteLine($"{lot.Number}\t{vehicle.RegistrationNumber}\t{vehicle.VehicleType}\t{vehicle.Color}");
                }
            }
        }

        static void PrintVehicleCountByType(string vehicleType)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            int count = 0;

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    if (vehicle.VehicleType.ToLower() == vehicleType.ToLower())
                    {
                        count++;
                    }
                }
            }

            Console.WriteLine(count);
        }

        static void PrintVehiclesWithOddPlate()
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            List<string> oddPlateNumbers = new List<string>();

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    int lastDigit = vehicle.RegistrationNumber[vehicle.RegistrationNumber.Length - 1] - '0';

                    if (lastDigit % 2 != 0)
                    {
                        oddPlateNumbers.Add(vehicle.RegistrationNumber);
                    }
                }
            }

            Console.WriteLine(string.Join(", ", oddPlateNumbers));
        }

        static void PrintVehiclesWithEvenPlate()
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            List<string> evenPlateNumbers = new List<string>();

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    int lastDigit = vehicle.RegistrationNumber[vehicle.RegistrationNumber.Length - 1] - '0';

                    if (lastDigit % 2 == 0)
                    {
                        evenPlateNumbers.Add(vehicle.RegistrationNumber);
                    }
                }
            }

            Console.WriteLine(string.Join(", ", evenPlateNumbers));
        }

        static void PrintVehiclesWithColor(string color)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            List<string> vehiclesWithColor = new List<string>();

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    if (vehicle.Color.ToLower() == color.ToLower())
                    {
                        vehiclesWithColor.Add(vehicle.RegistrationNumber);
                    }
                }
            }

            Console.WriteLine(string.Join(", ", vehiclesWithColor));
        }

        static void PrintSlotNumbersWithColor(string color)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            List<int> slotNumbersWithColor = new List<int>();

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    if (vehicle.Color.ToLower() == color.ToLower())
                    {
                        slotNumbersWithColor.Add(lot.Number);
                    }
                }
            }

            Console.WriteLine(string.Join(", ", slotNumbersWithColor));
        }

        static void PrintSlotNumberWithRegistrationNumber(string registrationNumber)
        {
            if (parkingLots.Count == 0)
            {
                Console.WriteLine("Parking lot has not been created yet");
                return;
            }

            foreach (ParkingLot lot in parkingLots)
            {
                if (lot.IsOccupied)
                {
                    Vehicle vehicle = lot.GetParkedVehicle();

                    if (vehicle.RegistrationNumber.ToLower() == registrationNumber.ToLower())
                    {
                        Console.WriteLine(lot.Number);
                        return;
                    }
                }
            }

            Console.WriteLine("Not found");
        }
    }

    class ParkingLot
    {
        public int Number { get; private set; }
        public bool IsOccupied { get; private set; }
        public Vehicle ParkedVehicle { get; private set; }

        public ParkingLot(int number)
        {
            Number = number;
            IsOccupied = false;
            ParkedVehicle = null;
        }

        public bool IsAvailable
        {
            get { return !IsOccupied; }
        }

        public void Park(Vehicle vehicle)
        {
            ParkedVehicle = vehicle;
            IsOccupied = true;
        }

        public void Leave()
        {
            ParkedVehicle = null;
            IsOccupied = false;
        }

        public Vehicle GetParkedVehicle()
        {
            return ParkedVehicle;
        }
    }

    class Vehicle
    {
        public string RegistrationNumber { get; private set; }
        public string Color { get; private set; }
        public string VehicleType { get; private set; }

        public Vehicle(string registrationNumber, string color, string vehicleType)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            VehicleType = vehicleType;
        }
    }
}
