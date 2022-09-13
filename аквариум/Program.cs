using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();
            aquarium.StartProgram();
        }
    }

    class Aquarium
    {
        private List<Fish> _fishList = new List<Fish>();
        private Player player = new Player();
        private Random _random = new Random();
        private Fish _aquariumFish = new Fish();
        public Aquarium()
        {
            int maxAmountFish = 5;
            int maxHealth = 100;

            for (int i = 0; i < maxAmountFish; i++)
            {
                _fishList.Add(new Fish(_random.Next(maxHealth)));
            }
        }
        public void StartProgram()
        {
            bool isWork = true;

            while (isWork && IsLive())
            {
                ShowAllFish();
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("аквариум с еще живыми рыбками \nX - достать рыбку, F - положить рыбку");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    ExitGame(key, ref isWork);
                    player.PutFish(key, ref _fishList);
                    player.TakeFish(key, ref _aquariumFish, ref _fishList);
                }

                LiveCycle();

                Console.ReadLine();
                Console.Clear();
            }
        }

        public bool IsLive()
        {
            return _fishList.Count > 0;
        }

        public void LiveCycle()
        {
            if(_fishList.Count > 0)
            {
                for (int i = 0; i < _fishList.Count; i ++)
                {
                    if (_fishList[i].Health > 0)
                    {
                        _fishList[i].RemoveCycle(_fishList[i]);
                    }
                    else
                    {
                        Console.WriteLine($"Рыбка №{_fishList[i].IdNumber} состарилась и умерла");
                        _fishList.Remove(_fishList[i]);
                        System.Threading.Thread.Sleep(1500);
                    }
                }
            }
        }

        static void ExitGame(ConsoleKeyInfo charkey, ref bool escape)
        {
            switch (charkey.Key)
            {
                case ConsoleKey.Escape:
                    escape = false;
                    break;
            }
        }

        public void ShowAllFish()
        {
            foreach (Fish fish in _fishList)
            {
                fish.ShowInfo();
            }
        }
    }

    class Player
    {
        private List<Fish> _package = new List<Fish>();
        private Random _random = new Random();
        public void TakeFish(ConsoleKeyInfo charkey, ref Fish fish, ref List<Fish> aquariumFish)
        {
            switch (charkey.Key)
            {
                case ConsoleKey.F:
                    _package.Add(fish);
                    aquariumFish.Remove(fish);
                    Console.WriteLine("Вы достали рыбку из аквариума"); 
                    break;
            }
        }

        public void PutFish(ConsoleKeyInfo charkey, ref List<Fish> fishList)
        {
            int randomIndex = _random.Next(_package.Count);
            switch (charkey.Key)
            {
                case ConsoleKey.X:
                    if (_package.Count > 0)
                    {
                        fishList.Add(_package[randomIndex]);
                        _package.RemoveAt(randomIndex);
                        Console.WriteLine("Вы скинули рыбку из своего пакетика в аквариум");
                        
                    }
                    else
                    {
                        Console.WriteLine("У Вас больше нет рыбы, чтобы положить в аквариум");
                    }
                    break;
            }
        }
    }

    class Fish
    {
        static private int _idNumber;
        public Fish(int health)
        {
            IdNumber = ++_idNumber;
            Health = health;
        }
        public Fish() { }

        public int IdNumber { get; private set; }
        public int Health { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Рыба №{IdNumber} <==<, осталось циклов жить {Health}");
        }

        public void RemoveCycle(Fish fish)
        {
            fish.Health -= 1;
        }
    }
}