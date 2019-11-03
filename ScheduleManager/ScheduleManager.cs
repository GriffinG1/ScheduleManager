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
        private static Label[] week1;
        private static Label[] week2;
        private static Label[] week3;
        private static Label[] week4;
        private static Label[] week5;

        public ScheduleManager()
        {
            InitializeComponent();

            week1 = new Label[] { week1Label, week1Sunday, week1Monday, week1Tuesday, week1Wednesday,
                                  week1Thursday, week1Friday, week1Saturday, week1Total };
            week2 = new Label[] { week2Label, week2Sunday, week2Monday, week2Tuesday, week2Wednesday,
                                  week2Thursday, week2Friday, week2Saturday, week2Total };
            week3 = new Label[] { week3Label, week3Sunday, week3Monday, week3Tuesday, week3Wednesday,
                                  week3Thursday, week3Friday, week3Saturday, week3Total };
            week4 = new Label[] { week4Label, week4Sunday, week4Monday, week4Tuesday, week4Wednesday,
                                  week4Thursday, week4Friday, week4Saturday, week4Total };
            week5 = new Label[] { week5Label, week5Sunday, week5Monday, week5Tuesday, week5Wednesday,
                                  week5Thursday, week5Friday, week5Saturday, week5Total };

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
            fileContents = new Dictionary<String, Dictionary<String, String>>();
            for (int i = 1; i < loadedFile.Length-2; i += 5)
            {
                String date = loadedFile[i].Substring(3);
                date = date.Substring(0, date.Length - 4);
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
                fileContents.Add(date, weekContents);
            }
            UpdateDisplay();
        }

        public static void UpdateDisplay()
        {
            String[] keyIndex = fileContents.Keys.ToArray();

            week1[0].Text = keyIndex[0];
            week2[0].Text = keyIndex[1];
            week3[0].Text = keyIndex[2];
            week4[0].Text = keyIndex[3];
            if (fileContents.Keys.Count == 4)
            {
                foreach (Label l in week5)
                {
                    l.Enabled = false;
                }
            }
            else week5[0].Text = keyIndex[4];


        }
    }
}