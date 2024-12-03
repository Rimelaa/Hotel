using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;
using System.IO;




namespace WindowsFormsApp2
{
    public partial class MainForm : Form
    {
        private List<List<string>> guests;
        private List<int> rooms;
        
        public MainForm()
        {
            InitializeData();

            listBoxMenu.Items.AddRange(new string[]
            {
                "1. Вывести список гостей",
                "2. Забронировать номер",
                "3. Список свободных номеров",
                "4. Вывести информацию о номерах",
                "5. Расписание питания",
                "0. Выход"
            });

            private Button btnLoad;
            private ComboBox comboBoxOptions;
            private ListBox listBoxGuests;
            private List<List<string>> guests;
            private List<int> rooms;
        }

        private void InitializeData()
        {
            string path = "guests.json";
            string room_path = "rooms.json";
            int luxury = 3;
            int standart = 5;
            int economy = 7;

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
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBoxMenu.SelectedIndex;
            if (selectedIndex == -1) return;

            switch (selectedIndex)
            {
                case 0:
                    ShowInfo();
                    break;
                case 1:
                    Book();
                    break;
                case 2:
                    CheckFreeRooms();
                    break;
                case 3:
                    RoomInfo();
                    break;
                case 4:
                    FoodMenu();
                    break;
                case 5:
                    this.Close();
                    break;
            }
        }

        private void ShowInfo()
        {
            this.Hide();
            ShowInfoForm form = new ShowInfoForm();
            form.Show();
        }

        private void Book()
        {
            this.Hide();
            BookForm form = new BookForm();
            form.Show();
        }

        private void CheckFreeRooms()
        {
            this.Hide();
            CheckForm form = new CheckForm();
            form.Show();
        }

        private void RoomInfo()
        {
            this.Hide();
            RoomInfoForm form = new RoomInfoForm();
            form.Show();
        }

        private void FoodMenu()
        {
            this.Hide();
            FoodForm form = new FoodForm();
            form.Show();
        }
    }
}











public class MainForm : Form
{
    private Button btnLoad;
    private ComboBox comboBoxOptions;
    private ListBox listBoxGuests;
    private List<List<string>> guests;

    public MainForm()
    {
        btnLoad.Click += BtnLoad_Click;

        comboBoxOptions = new ComboBox { Top = 50, Left = 10, Width = 200 };
        comboBoxOptions.Items.AddRange(new string[]
        {
            "Выбрать действие",
            "1. Все гости",
            "2. Гости с классом 'Luxury'",
            "3. Гости с классом 'Standart'",
            "4. Гости с классом 'Economy'",
            "5. Гости старше 30 лет",
            "6. Гости младше 30 лет"
        });
        comboBoxOptions.SelectedIndexChanged += ComboBoxOptions_SelectedIndexChanged;

        listBoxGuests = new ListBox { Top = 90, Left = 10, Width = 400, Height = 200 };

        Controls.Add(btnLoad);
        Controls.Add(comboBoxOptions);
        Controls.Add(listBoxGuests);
    }

    private void BtnLoad_Click(object sender, EventArgs e)
    {
        // Путь к файлу JSON, где хранятся данные о гостях
        string path = "guests.json";
        try
        {
            string jsonFromFile = File.ReadAllText(path);
            guests = JsonConvert.DeserializeObject<List<List<string>>>(jsonFromFile);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
        }
    }

    private void ComboBoxOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (guests == null) return;

        listBoxGuests.Items.Clear();
        switch (comboBoxOptions.SelectedIndex)
        {
            case 0:
                DisplayGuests(guests);
                break;
            case 1:
                DisplayGuests(guests.Where(list => list.Count > 2 && list[2] == "Luxury").ToList());
                break;
            case 2:
                DisplayGuests(guests.Where(list => list.Count > 2 && list[2] == "Standart").ToList());
                break;
            case 3:
                DisplayGuests(guests.Where(list => list.Count > 2 && list[2] == "Economy").ToList());
                break;
            case 4:
                DisplayGuests(guests.Where(list => list.Count > 1 && Convert.ToInt32(list[1]) > 30).ToList());
                break;
            case 5:
                DisplayGuests(guests.Where(list => list.Count > 1 && Convert.ToInt32(list[1]) < 30).ToList());
                break;
            default:
                listBoxGuests.Items.Add("Некорректный выбор");
                break;
        }
    }

    private void DisplayGuests(List<List<string>> guestList)
    {
        foreach (var guest in guestList)
        {
            listBoxGuests.Items.Add(string.Join(", ", guest));
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.Run(new MainForm());
    }
}

