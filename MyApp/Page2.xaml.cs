using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Common;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using ChoETL;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        List<Record> people = new List<Record>();
        string Directory;
        public Page2()
        {
           InitializeComponent();
            relativePathway();
          /// csv();
          /// writeJSON();
           readJSON();

        }
  

        public void csv()
        {
            using (StreamReader reader = File.OpenText(@"C:\\Users\\Leith\\Desktop\\Intake.txt"))
            {
                string line = reader.ReadLine(); //reads the next line available
                while (line != null) //while line is not nothing
                {
                    var parts = line.Split('='); // splits the string via the commas
                    int count = 0;
                    foreach(var part in parts)
                    {
                        var x = part;
                        x = part.Replace("\"", "");
                        parts[count] = x;
                        count++;
                    }


                    people.Add(new Record //creates a new record bases on the class Record
                    {
                        ID = parts[0],
                        Date = parts[1],
                        FirstName = parts[2],
                        LastName = parts[3],
                        DoB = parts[4],
                        PhoneNumber = parts[5],
                        Description = parts[6],
                        Availability = parts[7],
                        Fee = parts[8],
                        Pysch = parts[9],

                    });
                    line = reader.ReadLine(); //reads the next line then loops
                   

                }
            }
        }

        public void readJSON()
        {
            using (StreamReader r= new StreamReader(Directory + @"\\storage\\Intake.JSON"))
            {
                string json = r.ReadToEnd();
                people = JsonConvert.DeserializeObject<List<Record>>(json);
                this.grid2.ItemsSource = people;
            }
        }

        public void writeJSON()
        {
            List<Record> _data = new List<Record>();
            for(int i = 0; i < people.Count; i++)
            {
                _data.Add(new Record()      //loop this multiple tomes for ample csv
                {
                    ID = people[i].ID,
                    Date = people[i].Date,
                    FirstName = people[i].FirstName,
                    LastName = people[i].LastName,
                    DoB = people[i].DoB,
                    PhoneNumber = people[i].PhoneNumber,
                    Description = people[i].Description,
                    Availability = people[i].Availability,
                    Fee = people[i].Fee,
                    Pysch = people[i].Pysch,
                });
            }


            string json = JsonConvert.SerializeObject(_data.ToArray());         //Need to read a csv into an array to be made into a json using the code above

            //write string to file
            System.IO.File.WriteAllText(Directory + @"\\storage\\Intake.JSON", json);

        }


        public class Record
        {
            public string ID { get; set; }
            public string Date { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DoB { get; set; }         //what each record will contain
            public string PhoneNumber { get; set; }
            public string Description { get; set; }
            public string Availability { get; set; }
            public string Fee { get; set; }
            public string Pysch { get; set; }

        }


        public void relativePathway()
        {
            string[] pathway = new string[100];
            Directory = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] words = Directory.Split('\\');
            int lengthPathway = words.Length - 1;
            bool test = false;
            int counter = 0;

            words[lengthPathway] = null;
            lengthPathway = lengthPathway - 1;
            words[lengthPathway] = null;
            lengthPathway = lengthPathway - 1;
            words[lengthPathway] = null;

            while (words != null)
            {
                if (words[counter] == null)
                {
                    return;
                }
                if(test == false)
                {
                    Directory = words[counter];
                    counter++;
                    test = true;

                }
                else
                {
                    Directory = Directory + '\\' + words[counter];
                    counter++;
                }
            }
        }
    }
}
