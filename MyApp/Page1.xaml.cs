using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChoETL;
using System.Globalization;
using System.IO;
using System.Windows.Shell;
using System.Threading;
using Newtonsoft.Json;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        string Directory;
        List<Option2> peopleO2;
        List<Option1> peopleO1;
        List<WorkO2> WorkDays = new List<WorkO2>();
        List<WorkO1> RoomRentals = new List<WorkO1>();      /// a bunch of array lists to handle all the records
        int counter = 0;
        int RoomRentalCounter = 0;
        int halfDayCounter = 0;

        public Page1()
        {
            InitializeComponent();
            RelativePathway();
            readJSON();
            comboBoxStuff();
        }
           
        //uses streamreader to read the list of staff from a json file
        public void readJSON()
        {
            using (StreamReader r2 = new StreamReader(Directory + @"\\storage\\Staff-Option2.JSON"))
            {
                string jsony2 = r2.ReadToEnd();                                                             
                peopleO2 = JsonConvert.DeserializeObject<List<Option2>>(jsony2);           //this creates the array of records using the Option2 class 
            }

            foreach(var person in peopleO2)
            {
                cmbOption2.Items.Add(person.FirstName);         //this just adds the Psychologists first name to the combo box so they can be selected
            }

            using (StreamReader r1 = new StreamReader(Directory + @"\\storage\\Staff-Option1.JSON"))
            {
                string jsony1 = r1.ReadToEnd();
                peopleO1 = JsonConvert.DeserializeObject<List<Option1>>(jsony1);                ///this is just a mirror for the code above but for Option 1
            }

            foreach (var person in peopleO1)
            {
                cmbOption1.Items.Add(person.FirstName);
            }
        }

        public void comboBoxStuff()
        {
            for(int i = 0; i < 11; i++)
            {
                cmbFullClient.Items.Add(i);
                cmbSmallClient.Items.Add(i);            //just adds the number 1-10 to the combo boxs of Option2
            }
        }

        /// <summary>
        /// the next two buttons handle the passing of data to each invoice template
        /// they pass data rather inefficiently but it works so maybe one day when the program grows i'll tidy it up by rewriting it
        /// </summary>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOption2.SelectedItem == null)
            {
                MessageBox.Show("Please choose a Pyschologist");        ///this just checks to see if a Psych is picked
                return;
            }

            if (WorkDays.Count != 5)
            {
                for(int i = 5 - WorkDays.Count; i > 0; i--)
                {
                    WorkDays.Add(new WorkO2()
                    {
                        smallClient = "",                   ///This little fella fills the array of lists to five even if 5 havnt been chose
                        amtSmallClient = "",                ///making it easier to handle how many days are chose on the template
                        fullClient = "",
                        amtFullClient = "",
                        Date = ""

                    });
                }
            }


            string selectedPerson = cmbOption2.SelectedItem.ToString();
            string selectedFirstName = null;
            string selectedLastName = null;         ///defines all the variables prior because you can declare variables in an for/if and use them outside it o.O
            string selectedABN = null;
            string selectedPercentage = null;

            foreach (var item in peopleO2)
            {
                if (selectedPerson == item.FirstName)
                {
                    selectedFirstName = item.FirstName;
                    selectedLastName = item.LastName;           /// finds the chosen Psych so that the required infomation can be passed
                    selectedABN = item.ABN;
                    selectedPercentage = item.Percentage;
                }
            }

            InvoiceO2 inv2 = new InvoiceO2(
                selectedFirstName, 
                selectedLastName, 
                selectedABN, 
                selectedPercentage,
                WorkDays[0].smallClient,
                WorkDays[0].amtSmallClient,
                WorkDays[0].fullClient,
                WorkDays[0].amtFullClient,
                WorkDays[0].Date,
                WorkDays[1].smallClient,
                WorkDays[1].amtSmallClient,
                WorkDays[1].fullClient,
                WorkDays[1].amtFullClient,
                WorkDays[1].Date,
                WorkDays[2].smallClient,            ///rather ineffictivly passes data to the next window but it works
                WorkDays[2].amtSmallClient,         ///by calling the next window with this data, i think o.O
                WorkDays[2].fullClient,
                WorkDays[2].amtFullClient,
                WorkDays[2].Date,
                WorkDays[3].smallClient,
                WorkDays[3].amtSmallClient,
                WorkDays[3].fullClient,
                WorkDays[3].amtFullClient,
                WorkDays[3].Date,
                WorkDays[4].smallClient,
                WorkDays[4].amtSmallClient,
                WorkDays[4].fullClient,
                WorkDays[4].amtFullClient,
                WorkDays[4].Date
                ); ///add fields to this to pass data
          
         inv2.Show();
            counter = 0;



            for (int i = WorkDays.Count - 1; i >= 0; i--)
            {   
                WorkDays.RemoveAt(i);               ///clears the work record array so that this whole thing can be executed multiple times without restarting
            }
            lblWork.Content = "";
            txtAmtFull.Text = "130.00";
            txtAmtSmall.Text = "84.80";
        }

        /// <summary>
        /// basically the exact same as the button above so read the documentation above because im lazy.
        /// </summary>

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cmbOption1.SelectedItem == null)
            {
                MessageBox.Show("Please choose a Pyschologist");
                return;
            }

            if (RoomRentals.Count != 5)
            {
                for (int i = 5 - RoomRentals.Count; i > 0; i--)
                {
                    RoomRentals.Add(new WorkO1()
                    {
                        Day = "",
                        Date = "",
                        Cost = ""

                    });
                }
            }

            string selectedPerson = cmbOption1.SelectedItem.ToString();
            string selectedFirstName = null;
            string selectedLastName = null;
            string selectedABN = null;

            foreach (var item in peopleO1)
            {
                if (selectedPerson == item.FirstName)
                {
                    selectedFirstName = item.FirstName;
                    selectedLastName = item.LastName;
                    selectedABN = item.ABN;
                }
            }

            InvoiceO1 O1 = new InvoiceO1(
                selectedFirstName,
                selectedLastName,
                selectedABN,
                RoomRentals[0].Day,
                RoomRentals[0].Date,
                RoomRentals[0].Cost,
                RoomRentals[1].Day,
                RoomRentals[1].Date,
                RoomRentals[1].Cost,
                RoomRentals[2].Day,
                RoomRentals[2].Date,
                RoomRentals[2].Cost,
                RoomRentals[3].Day,
                RoomRentals[3].Date,
                RoomRentals[3].Cost,
                RoomRentals[4].Day,
                RoomRentals[4].Date,
                RoomRentals[4].Cost
                ); ///add fields to this to pass data

            O1.Show();

            for (int i = RoomRentals.Count - 1; i >= 0; i--)
            {
                 RoomRentals.RemoveAt(i);
             }
            lblWorkO1.Content = "";
            RoomRentalCounter = 0;
        }

        /// <summary>
        /// another instance of relative pathway so that it can run whereever the files are
        /// </summary>
        public void RelativePathway()
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
                if (test == false)
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


        /// <summary>
        /// this is just the classes used to structure the records of the array lists
        /// </summary>
        public class WorkO2
        {
            public string smallClient { get; set; }
            public string amtSmallClient { get; set; }
            public string fullClient { get; set; }
            public string amtFullClient { get; set; }
            public string Date { get; set; }

        }
        public class WorkO1
        {
            public string Day { get; set; }
            public string Date { get; set; }
            public string Cost { get; set; }
        }
        public class Option2
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ABN { get; set; }
            public string Percentage { get; set; }
        }
        public class Option1
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ABN { get; set; }

        }


        /// <summary>
        /// The next three buttons add the work days to the arrays and to lables to be viewed
        /// </summary>


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int x = int.Parse(txtAmtFull.Text, CultureInfo.InvariantCulture.NumberFormat);
                int y = int.Parse(txtAmtSmall.Text, CultureInfo.InvariantCulture.NumberFormat);

                // If an exception occurs in the following code, then the finally will be executed
                // and the exception will be throw
            }
            catch (Exception ex)
            {
                // I WANT THIS CODE TO RUN EVENTUALLY REGARDLESS AN EXCEPTION OCCURED OR NOT
                MessageBox.Show("You need to enter a numeric value for the cost :3");
                return;
            }

            if (workDate.SelectedDate == null){
                MessageBox.Show("You'll need to choose a date");
                return;
            }

            var fullDateTime = workDate.SelectedDate.ToString();
            var dayMonthYear = fullDateTime.Split(" ");
           
            if(WorkDays.Count == 5)
            {
                MessageBox.Show("You can only have 5 days");
                return;
            }

            if (cmbSmallClient.SelectedItem == null || cmbFullClient.SelectedItem == null)
            {
                MessageBox.Show("You need to select a value for both client types");
                return;
            }

            WorkDays.Add(new WorkO2() {

                smallClient = cmbSmallClient.SelectedItem.ToString(),
                amtSmallClient = txtAmtSmall.Text,
                fullClient = cmbFullClient.SelectedItem.ToString(),
                amtFullClient = txtAmtFull.Text,
                Date = dayMonthYear[0].ToString(),

                });

            var dateParts = dayMonthYear[0].Split("/");
            string monthDay = dateParts[0] + "/" + dateParts[1];

            if (lblWork.Content == "")
            {
                lblWork.Content = monthDay + "      " + WorkDays[0].fullClient + " x clients @ $" + WorkDays[0].amtFullClient + " " + WorkDays[0].smallClient + " x clients @ $" + WorkDays[0].amtSmallClient;
                counter++;
            }
            else
            {
                lblWork.Content = lblWork.Content + Environment.NewLine + monthDay + "      " + WorkDays[counter].fullClient + " x clients @ $" + WorkDays[counter].amtFullClient + " " + WorkDays[counter].smallClient + " x clients @ $" + WorkDays[counter].amtSmallClient;
                counter++;
            }
        }

        private void btnHalfDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x = int.Parse(txtHalfDayCost.Text, CultureInfo.InvariantCulture.NumberFormat);

                // If an exception occurs in the following code, then the finally will be executed
                // and the exception will be throw
            }
            catch (Exception ex)
            {
                // I WANT THIS CODE TO RUN EVENTUALLY REGARDLESS AN EXCEPTION OCCURED OR NOT
                MessageBox.Show("You need to enter a numeric value for the cost :3");
                txtHalfDayCost.Text = "150";
                txtFullDayCost.Text = "200";
                return;
            }



            if (workDateO1.SelectedDate == null)
            {
                MessageBox.Show("You'll need to choose a date");
                return;
            }

            if (RoomRentals.Count == 5)
            {
                MessageBox.Show("You can only have 5 days");
                return;
            }


            var fullDateTime = workDateO1.SelectedDate.ToString();
            var dayMonthYear = fullDateTime.Split(" ");
            var dateParts = dayMonthYear[0].Split("/");
            string monthDay = dateParts[0] + "/" + dateParts[1];

            RoomRentals.Add(new WorkO1()
            {
                Day = "half day",
                Date = dayMonthYear[0],
                Cost = txtHalfDayCost.Text
            });


            if (lblWorkO1.Content == "")
            {
                lblWorkO1.Content = monthDay + "      " + "Room Rental (half day)" + "  $" + RoomRentals[RoomRentalCounter].Cost;
            }
            else
            {
                lblWorkO1.Content = lblWorkO1.Content + Environment.NewLine + monthDay + "      " + "Room Rental (half day)" + "  $" + RoomRentals[RoomRentalCounter].Cost;
            }
            RoomRentalCounter++;
        }

        private void btnFullDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x = int.Parse(txtFullDayCost.Text, CultureInfo.InvariantCulture.NumberFormat);

                // If an exception occurs in the following code, then the finally will be executed
                // and the exception will be throw
            }
            catch (Exception ex)
            {
                // I WANT THIS CODE TO RUN EVENTUALLY REGARDLESS AN EXCEPTION OCCURED OR NOT
                MessageBox.Show("You need to enter a numeric value for the cost :3");
                txtHalfDayCost.Text = "150";
                txtFullDayCost.Text = "200";
                return;
            }



            if (workDateO1.SelectedDate == null)
            {
                MessageBox.Show("You'll need to choose a date");
                return;
            }

            if (RoomRentals.Count == 5)
            {
                MessageBox.Show("You can only have 5 days");
                return;
            }

            var fullDateTime = workDateO1.SelectedDate.ToString();
            var dayMonthYear = fullDateTime.Split(" ");
            var dateParts = dayMonthYear[0].Split("/");
            string monthDay = dateParts[0] + "/" + dateParts[1];

            RoomRentals.Add(new WorkO1()
            {
                Day = "full day",
                Date = dayMonthYear[0],
                Cost = txtFullDayCost.Text
            });

            if (lblWorkO1.Content == "")
            {
                lblWorkO1.Content = monthDay + "      " + "Room Rental (full day)" + "    $" + RoomRentals[RoomRentalCounter].Cost;
            }
            else
            {
                lblWorkO1.Content = lblWorkO1.Content + Environment.NewLine + monthDay + "      " + "Room Rental (full day)" + "    $" + RoomRentals[RoomRentalCounter].Cost;
            }
            RoomRentalCounter++;
        }


        /// <summary>
        /// These buttons are just a clear method to wipe the current work days
        /// </summary>

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            for (int i = RoomRentals.Count - 1; i >= 0; i--)
            {
                RoomRentals.RemoveAt(i);
            }
            lblWorkO1.Content = "";
            txtFullDayCost.Text = "200";
            txtHalfDayCost.Text = "150";
            RoomRentalCounter = 0;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = WorkDays.Count - 1; i >= 0; i--)
            {
                WorkDays.RemoveAt(i);               ///clears the work record array so that this whole thing can be executed multiple times without restarting
            }
            lblWork.Content = "";
            txtAmtFull.Text = "130.00";
            txtAmtSmall.Text = "84.80";
            RoomRentalCounter = 0;
        }
    }
}
