
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualBasic;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;


namespace MoscowCitySilicon
{    
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] menuItems =
            {
                "1. Вывести список гостей",
                "2. Забронировать номер",
                "3. Список свободных номеров",
                "4. Вывести информацию о номерах",
                "5. Расписание питания",
                "0. Выход"
            };
            int selectedItem = 0;

            string path = "guests.json";
            string room_path = "rooms.json";
            int luxury = 3;
            int standart = 5;
            int economy = 7;

            List<List<string>> guests = new List<List<string>>();
            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                string json = JsonConvert.SerializeObject(guests, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            else
            {
                string file = File.ReadAllText(path);
                guests = JsonConvert.DeserializeObject<List<List<string>>>(file);
            }

            List<int> rooms = new List<int>();
            if (!File.Exists(room_path) || new FileInfo(room_path).Length == 0)
            {
                rooms.Add(luxury);
                rooms.Add(standart);
                rooms.Add(economy);

                string json = JsonConvert.SerializeObject(rooms, Formatting.Indented);
                File.WriteAllText(room_path, json);
            }
            else
            {
                string file2 = File.ReadAllText(room_path);
                rooms = JsonConvert.DeserializeObject<List<int>>(file2);
            }


            do
            {
                Console.Clear();
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedItem)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine($"{menuItems[i]}");
                    Console.ResetColor();
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedItem = (selectedItem > 0) ? selectedItem - 1 : menuItems.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedItem = (selectedItem < menuItems.Length - 1) ? selectedItem + 1 : 0;
                }
                else if (key == ConsoleKey.Enter)
                {
                    switch (selectedItem)
                    {
                        case 0:
                            ShowInfo(path, guests);
                            break;
                        case 1:
                            Book(path, luxury, standart, economy, guests, rooms);
                            break;
                        case 2: 
                            CheckFreeRooms(room_path, rooms);
                            break;
                        case 3:
                            RoomInfo();
                            break;
                        case 4:
                            FoodMenu();
                            break;
                        case 5: 
                            return;
                        default:
                            Console.WriteLine("Неверный ввод");
                            break;
                    }

                    Console.Clear();
                }
            } while (true);
        }

        public static void ShowInfo(string path, List<List<string>> guests)
        {
            Console.Clear();
            try
                {
                string jsonFromFile = File.ReadAllText(path);
                List<List<string>> getGuests = JsonConvert.DeserializeObject<List<List<string>>>(jsonFromFile);
                
                var getLuxury = guests.Where(list => list[2] == "Luxury").ToList();
                var getStandart = guests.Where(list => list[2] == "Standart").ToList();
                var getEconomy = guests.Where(list => list[2] == "Economy").ToList();
                var youngerThan30 = guests.Where(list => Convert.ToInt32(list[1]) < 30).ToList();
                var olderThan30 = guests.Where(list =>  Convert.ToInt32(list[1]) > 30).ToList();

                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Вывести всех гостей");
                Console.WriteLine("2. Вывести гостей с номером класса 'Luxury'");
                Console.WriteLine("3. Вывести гостей с номером класса 'Standart'");
                Console.WriteLine("4. Вывести гостей с номером класса 'Economy'");
                Console.WriteLine("5. Вывести гостей младше 30 лет");
                Console.WriteLine("6. Вывести гостей старше 30 лет\n");
                Console.WriteLine("0. Назад");

                int choice = 0;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некорректный выбор.");
                    return;
                } 
            
                switch (choice) 
                {
                    case 1:
                        Console.WriteLine("Все гости:");
                        foreach (var guest in guests)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break;
                    case 2:
                        Console.WriteLine("Гости с классом 'Luxury':");
                        foreach (var guest in getLuxury)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break;
                    case 3:
                        Console.WriteLine("Гости с классом 'Standart':");
                        foreach (var guest in getStandart)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break;
                    case 4:
                        Console.WriteLine("Гости с классом 'Economy':");
                        foreach (var guest in getEconomy)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break; 
                    case 5:
                        Console.WriteLine("Гости младше 30 лет:");
                        foreach (var guest in youngerThan30)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break; 
                    case 6:
                        Console.WriteLine("Гости старше 30 лет:");
                        foreach (var guest in olderThan30)
                        {
                            Console.WriteLine(string.Join(", ", guest));
                        }
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
                Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
                
                Console.WriteLine("\n\n");
            }catch (ArgumentNullException) {
                Console.WriteLine("Гостей нет");
                return;
            } catch (FormatException) {
                Console.WriteLine("Некорректный выбор.");
                return; 
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Некорректный выбор.");
                return;
            } catch (NullReferenceException) {
                Console.WriteLine("Некорректный выбор.");
                return;
            } catch (ArgumentOutOfRangeException) {
                Console.WriteLine("Некорректный выбор.");                
                return;
            }
        }
    
        public static void Book(string path, int luxury, int standart, int economy, 
                                List<List<string>> guests, List<int> rooms)
        {       
            if (luxury > 0 || standart > 0 || economy > 0)
            {
                Console.WriteLine("Введите имя");
                string name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Некорректный ввод");
                    return;
                }
                if (2 > name.Length || name.Length > 30)
                {
                    Console.WriteLine("Некорректный ввод");
                    return;
                }
                if ("0123456789" .Contains(name))
                {
                    Console.WriteLine("Некорректный ввод");
                    return;
                }
                
                Console.WriteLine("Введите возраст");
                string age = Console.ReadLine();
                if (!int.TryParse(age, out int ageInt))
                {
                    Console.WriteLine("Некорректный ввод");
                    return;
                }
                if (ageInt < 18 || ageInt > 100)
                {
                    Console.WriteLine("Некорректный ввод");
                    return;
                }

                Console.WriteLine("Выберите класс номера");
                if (luxury > 0)
                {
                    Console.WriteLine("1. Luxury");
                }
                if (standart > 0)
                {
                    Console.WriteLine("2. Standart");
                }
                if (economy > 0)
                {
                    Console.WriteLine("3. Economy");
                }
                
                switch (Console.ReadLine())
                {
                    case "1":
                        if (luxury > 0)
                        {
                            Console.WriteLine("Вы забронировали номер класса 'Luxury'\n");
                            guests.Add(new List<string> { name, age, "Luxury" });
                            rooms[0] -= 1;
                        }
                        else
                        {
                            Console.WriteLine("Извините, номера Luxury не доступны.");
                        }
                        break;

                    case "2":
                        if (standart > 0)
                        {
                            Console.WriteLine("Вы забронировали номер класса 'Standart'\n");
                            guests.Add(new List<string> { name, age, "Standart"});
                            rooms[1] -= 1;
                        }
                        else
                        {
                            Console.WriteLine("Извините, номера Standart не доступны.");
                        }
                        break;

                    case "3":
                        if (economy > 0)
                        {
                            Console.WriteLine("Вы забронировали номер класса 'Economy'\n");
                            guests.Add(new List<string> { name, age, "Economy" });
                            rooms[2] -= 1;
                        }
                        else
                        {
                            Console.WriteLine("Извините, номера Economy не доступны.");
                        }
                        break;

                    default:
                        Console.WriteLine("Некорректный выбор, попробуйте еще раз.");
                        break;
                }

                string json = JsonConvert.SerializeObject(guests, Formatting.Indented);    
                string json2 = JsonConvert.SerializeObject(rooms, Formatting.Indented);
                File.WriteAllText(path, json);
                File.WriteAllText("rooms.json", json2);
            } else {Console.WriteLine("Свободных номеров нет");}

            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();

        }

        public static void CheckFreeRooms(string room_path, List<int> rooms)
        {
            string jsonFromFile = File.ReadAllText(room_path);

            Console.WriteLine($"Свободных номеров с классом 'Luxury': {rooms[0]}");
            Console.WriteLine($"Свободных номеров с классом 'Standart': {rooms[1]}");
            Console.WriteLine($"Свободных номеров с классом 'Economy': {rooms[2]}");

            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
        public static void RoomInfo()
        {
            Console.WriteLine("Номер 'Luxury' включает в себя: \n" +
                "1. 3х разовое питание по системе 'Шведский стол'\n" +
                "2. Посещение бассейна и СПА зон, включающие в себя тайский массаж всего тела, грязевые ванны, золотое обертование.\n" +
                "3. Личная ассистентка, которая поможет скрасить вам досуг:)\n" +
                "4. Номер с панорамными окнами,3мя спальными комнатами и 2 уборными комнатами\n");

            Console.WriteLine("Номер 'Standart' включает в себя: \n" +
                "1. 3х разовое питание по системе 'Шведский стол'\n" +
                "2. Посещение бассейна\n" +
                "3. Номер с 2мя спальными комнатами и панорамными окнами\n");
                
            Console.WriteLine("Номер 'Economy' включает в себя: \n" +
                "1. Завтрак\n" +
                "2. Номер с просторной спальной комнатой\n\n");

            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
                
        }
        
        public static void FoodMenu()
        {
            Console.WriteLine("1.Краб с черной икрой - свежие крабы, подаются с черной икрой, лимонным соком и зеленью.\n" +
                "2.Стейк вагю - высококачественное мясо с мраморностью, приготовленное с минимальным количеством специй.\n" +
                "3.Фуа-гра на тостах - утренний деликатес, подается с джемом из инжира.\n" +
                "4.Лобстер с чесночным маслом - запеченные лобстеры, подаются с ароматным чесночным маслом.\n" +
                "5.Царская рыба с трюфелями - свежая рыба макси-класса, запеченная\n\n");
            
            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}