using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternCommand
{
    public class Elevator
    {
        private static Elevator _instance;

        public static Elevator GetInstance() => _instance ??= new Elevator();

        public int CurrentFloor { get; private set; } = 1;
        public bool DoorIsOpened { get; private set; } = false;

        private Elevator() { }

        public void MoveTo(int floor)
        {
            if (Building.GetInstance().Floors.ContainsKey(floor))
            {
                Console.WriteLine($"Лифт едет с {CurrentFloor} на {floor}");
                //Console.WriteLine($"Комнаты на {floor} этаже: {Building.GetInstance().Floors[floor].Rooms[0]}, {Building.GetInstance().Floors[floor].Rooms[1]}");
                CurrentFloor = floor;
            }
            else
            {
                Console.WriteLine($"Этажа {floor} не существует");
            }
        }

        public void OpenDoor()
        {
            DoorIsOpened = true;
        }
        public void CloseDoor()
        {
            DoorIsOpened = false;
        }
    }

    public struct Floor
    {
        public int FloorNumber { get; }
        public string[] Rooms { get; } = new string[2];

        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
            Rooms = ["Комната " + (floorNumber * 2 - 1), "Комната " + floorNumber * 2];
        }
    }

    public class Building
    {
        private static Building _instance;

        public static Building GetInstance() => _instance ??= new Building();

        public Dictionary<int, Floor> Floors { get; } = new Dictionary<int, Floor>();
        public Elevator Elevator => Elevator.GetInstance();

        private Building()
        {
            InitializeFloors();
        }

        private void InitializeFloors()
        {
            for (int i = 1; i < 11; i++)
            {
                Floors.Add(i, new Floor(i));
            }
        }
    }
}
