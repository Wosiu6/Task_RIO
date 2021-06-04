using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_ROI.Model
{
    public class Task
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }

        public Task(string title)
        {
            Title = title;
        }

        public Task(string title, DateTime date)
        {
            Title = title;
            Date = date;
        }

        public override string ToString()
        {
            return Title + " " + Date.ToString("dd/MM/yyyy") + " " + Done;
        }
    }
}
