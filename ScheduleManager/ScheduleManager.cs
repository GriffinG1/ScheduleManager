using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace ScheduleManager
{
    public partial class ScheduleManager : Form
    {

        private static String[] loadedFile;
        private static Dictionary<String, Dictionary<String, String>> fileContents;

        public ScheduleManager()
        {
            InitializeComponent();
            InitializeData();
        }

        public static void InitializeData()
        {
            if (!File.Exists("data/" + System.DateTime.Today.Year.ToString() + "/" +
                             System.DateTime.Today.ToString("MMMM") + ".json"))
            {
                List<DateTime> sundays = GetSundays();
                Dictionary<String, Dictionary<String, String>> template = new Dictionary<String, Dictionary<String, String>>();
                Dictionary<String, String> dayContent = new Dictionary<String, String>
                {
                    { "startTime", "No Value" },
                    { "endTime", "No Value" },
                    { "totalTime", "No Value" }
                };
                foreach (DateTime dt in sundays)
                {
                    String date = dt.Date.ToString("MM/dd/yyyy");
                    template.Add(date, dayContent);
                }
                var json = JsonConvert.SerializeObject(template, Formatting.Indented);
                File.WriteAllText("data/" + System.DateTime.Today.Year.ToString() + "/" +
                                  System.DateTime.Today.ToString("MMMM") + ".json", json);
            }
            loadedFile = File.ReadAllLines("data/" + System.DateTime.Today.Year.ToString() + "/" +
                                           System.DateTime.Today.ToString("MMMM") + ".json");
            LoadData();
        }

        public static List<DateTime> GetSundays() // https://forums.asp.net/post/1828187.aspx
        {
            List<DateTime> lstSundays = new List<DateTime>();
            int intMonth = DateTime.Now.Month;
            int intYear = DateTime.Now.Year;
            int intDaysThisMonth = DateTime.DaysInMonth(intYear, intMonth);
            DateTime oBeginnngOfThisMonth = new DateTime(intYear, intMonth, 1);
            for (int i = 1; i < intDaysThisMonth + 1; i++)
            {
                if (oBeginnngOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                {
                    lstSundays.Add(new DateTime(intYear, intMonth, i));
                }
            }
            return lstSundays;
        }

        public static void LoadData()
        {
            fileContents = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 1; i < loadedFile.Length-2; i += 5)
            {
                String startTime = loadedFile[i + 1].Substring(18);
                startTime = startTime.Substring(0, startTime.Length - 2);
                String endTime = loadedFile[i + 2].Substring(16);
                endTime = endTime.Substring(0, endTime.Length - 2);
                String totalTime = loadedFile[i + 3].Substring(18);
                totalTime = totalTime.Substring(0, totalTime.Length - 1);
                Dictionary<String, String> weekContents = new Dictionary<String, String>
                {
                    { "startTime", startTime },
                    { "endTime", endTime },
                    { "totalTime", totalTime }
                };
                fileContents.Add(loadedFile[i], weekContents);
            }
        }
    }
}