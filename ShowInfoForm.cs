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
    public partial class ShowInfoForm : Form
    {
        public ShowInfoForm()
        {
            InitializeComponent();
        }

        
        private void ShowInfoForm_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText("info.json");
            var info = JsonConvert.DeserializeObject<Info>(json);
            this.Text = info.Name;
            this.label1.Text = info.Name;
            this.label2.Text = info.Description;
            this.label3.Text = info.Version;
        }

        private void GetGuestBox(object sender, EventArgs e)
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
    }
}
