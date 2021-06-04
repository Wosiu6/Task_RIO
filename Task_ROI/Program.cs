
using CommandLineParser;
using CommandLineParser.Arguments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_ROI
{
    class Program
    {
        static int tableWidth = 73;

        public static List<Model.Task> CurrentTasks = new List<Model.Task>();
        public static List<Model.Task> AllTasks = new List<Model.Task>();

        static void Main(string[] args)
        {
            ReadMainCommand();
        }

        public static void ReadMainCommand()
        {

            var input = Console.ReadLine();

            var FirstCommand = input.Split(' ')[0];

            while (FirstCommand != "-stop")
            {
                switch (FirstCommand)
                {
                    case ("-stop"):
                        {
                            System.Environment.Exit(1);
                            break;
                        }
                    case ("-add"):
                        {
                            AddTask(input);
                            break;
                        }
                    case ("-showCurrent"):
                        {
                            ShowTasks(CurrentTasks); //showing current tasks (filtered)
                            break;
                        }
                    case ("-showAll"):
                        {
                            ShowTasks(AllTasks);
                            break;
                        }
                    case ("-save"):
                        {
                            Save(input); //Saving to file
                            break;
                        }
                    case ("-load"):
                        {
                            Load(input); //Not implemented load functionality
                            break;
                        }
                    case ("-filter"):
                        {
                            Filter(input);
                            break;
                        }
                    case ("-complete"):
                        {
                            Complete(input);
                            break;
                        }
                    case ("-sort"):
                        {
                            Sort(input);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Command not recognised");
                            break;
                        }
                }
                input = Console.ReadLine();
                FirstCommand = input.Split(' ')[0];
            }
        }

        private static void Sort(string input)
        {
            Arguments cmdline = new Arguments(input.Split(' '));

            if (cmdline["t"] != null)
                AllTasks = AllTasks.OrderBy(x => x.Title)
                    .ToList();
            if (cmdline["d"] != null)
                AllTasks = AllTasks.OrderBy(x => x.Date)
                    .ToList();

            Console.WriteLine("Sorted list using parameters provided (-t/-d)");
        }

        private static void Complete(string input)
        {
            Arguments cmdline = new Arguments(input.Split(' '));

            if (cmdline["title"] != null && cmdline["date"] != null)
            {
                AllTasks
                    .Where(x => x.Date == DateTime.Parse(cmdline["date"]))
                    .Where(x => x.Title == cmdline["title"])
                    .Where(x => x.Done == false)
                    .ToList()
                    .ForEach(x=>x.Done=true);

                Console.WriteLine("Tasks " + cmdline["date"] + " "+ cmdline["title"] + " updated");
            }
            else
            {
                if (cmdline["title"] != null)
                {
                    AllTasks
                        .Where(x => x.Title == cmdline["title"])
                        .Where(x => x.Done == false)
                        .ToList()
                        .ForEach(x => x.Done = true);

                    Console.WriteLine("Tasks " + cmdline["title"] + " updated");
                }

                if (cmdline["date"] != null)
                {
                    AllTasks
                        .Where(x => x.Date == DateTime.Parse(cmdline["date"]))
                        .Where(x => x.Done == false)
                        .ToList()
                        .ForEach(x => x.Done = true);

                    Console.WriteLine("Tasks " + cmdline["date"] + " updated");
                }
            }
        }


        private static void Save(string input)
        {
            var path = "SavedList.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            TextWriter tw = new StreamWriter(path);

            foreach (String s in AllTasks.Select(x=>x.ToString()))
                tw.WriteLine(s);

            tw.Close();

            System.Diagnostics.Process.Start(path);
        }
        private static void Load(string input)
        {
            Console.WriteLine("Not yet implemented");
        }
        private static void Filter(string input)
        {
            Arguments cmdline = new Arguments(input.Split(' '));

            if (cmdline["title"] != null && cmdline["date"] != null)
            {
                CurrentTasks = AllTasks
                    .Where(x => x.Date == DateTime.Parse(cmdline["date"]))
                    .Where(x => x.Title == cmdline["title"])
                    .ToList();
            }
            else
            {
                if (cmdline["title"] != null)
                {
                    CurrentTasks = AllTasks
                        .Where(x => x.Title == cmdline["title"])
                        .ToList();
                }

                if (cmdline["date"] != null)
                {
                    CurrentTasks = AllTasks
                        .Where(x => x.Date == DateTime.Parse(cmdline["date"]))
                        .ToList();
                }
            }

            Console.WriteLine("Tasks filtered into Current Tasks (use -showCurrent to display)");
        }

        private static void ShowTasks(List<Model.Task> Tasks)
        {
            PrintLine();
            PrintRow("Title", "Date", "Done");
            PrintLine();

            foreach (Model.Task t in Tasks)
            {
                PrintRow(t.Title, t.Date.ToString("dd/MM/yyyy"), t.Done.ToString());
            }

            PrintLine();
        }

        private static void AddTask(string input)
        {
            Arguments cmdline = new Arguments(input.Split(' '));



            if (cmdline["title"] != "" && cmdline["date"] != "")
            {

                try
                {
                    string title = cmdline["title"];
                    DateTime date = DateTime.Parse(cmdline["date"]);

                    AllTasks.Add(new Model.Task(title, date));
                    Console.WriteLine("Tasks " + cmdline["date"] + " " + cmdline["title"] + " added");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error whilst parsing date");
                }
            }
            else
                Console.WriteLine("You have to provide date and title");
        }


        #region consoleUtil
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
        #endregion
    }
}
