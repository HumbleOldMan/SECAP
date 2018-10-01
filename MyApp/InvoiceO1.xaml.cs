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

namespace MyApp
{
    /// <summary>
    /// Interaction logic for InvoiceO1.xaml
    /// </summary>
    public partial class InvoiceO1 : Window
    {
        string[] _monthsArray = new string[11] { "Januaray", "Febuary", "March", "April", "May", "June", "July", "August", "September", "November", "December" };
        float[] _numbersCalc = new float[5];
        string _FullName; 
        string _FirstName;
        string _ABN;
        string _Directory = null;
        string _ID = null;

        List<WorkO1> Roomrentals = new List<WorkO1>();

        public InvoiceO1(
            string FirstName,
            string LastName,
            string ABN,
            string Day,
            string Date,
            string Cost,
            string Day1,
            string Date1,           /// this is the data being passed from page1
            string Cost1,
            string Day2,
            string Date2,
            string Cost2,
            string Day3,
            string Date3,
            string Cost3,
            string Day4,
            string Date4,
            string Cost4

            )
        {
            RelativePathway();
            InitializeComponent();
            _FullName = FirstName + " " + LastName;           ///update the variable
            _FirstName = FirstName;
            _ABN = ABN;

            Roomrentals.Add(new WorkO1()
            {
                Day = Day,
                Date = Date,
                Cost = Cost
            });
            Roomrentals.Add(new WorkO1()
            {
                Day = Day1,
                Date = Date1,
                Cost = Cost1

            });
            Roomrentals.Add(new WorkO1()
            {
                Day = Day2,
                Date = Date2,
                Cost = Cost2

            });     ///this adds the data being passed to an array list WorkO1
            Roomrentals.Add(new WorkO1()
            {
                Day = Day3,
                Date = Date3,
                Cost = Cost3

            });
            Roomrentals.Add(new WorkO1()
            {
                Day = Day4,
                Date = Date4,
                Cost = Cost4
            });
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight; ///this is used to center the window upon launch
        }

        public System.Windows.SizeToContent SizeToContent { get; set; }     ///i Dont think this needs to be here but it is

        private void ContentRenderedEvent(object sender, EventArgs e)   ///this will run once the window is rendered quite usefull
        {
            this.MinWidth = this.Width;     ///this allows me to force the size of the window on all monitors, the values are set in the xaml
            this.MinHeight = this.Height;
            fillingWork();
            Calculations();     /// this just calls the functions
            ID();

            Image.Source = new BitmapImage(
    new Uri(_Directory + @"\\storage\\logo.jpg"));      ///this sets the image source for the page - the logo

            lblName1.Content = _FullName;
            lblABN.Content = "ABN: " + _ABN;


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to create this invoice?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                createImage();          ///this just runs after the window is loaded and confirms if they want the thing to saved
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        ///just a function to set the relative pathway 
        public void RelativePathway()       
            {
                string[] pathway = new string[100];
                _Directory = System.AppDomain.CurrentDomain.BaseDirectory;
                string[] words = _Directory.Split('\\');
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
                        _Directory = words[counter];
                        counter++;
                        test = true;

                    }
                    else
                    {
                        _Directory = _Directory + '\\' + words[counter];
                        counter++;
                    }
                }
            }

        /// <summary>
        /// just some funtion that takes a screenshot a4 size and saves it as a png in the invoice file
        /// </summary>
        public void createImage()
            {

                    
                var w = 2480;
                var h = 3508;

                var screen = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

                var visual = new DrawingVisual();
                using (var context = visual.RenderOpen())
                {
                    context.DrawRectangle(new VisualBrush(screen),
                                          null,
                                          new Rect(new Point(), new Size(screen.Width, screen.Height)));
                }

                visual.Transform = new ScaleTransform(w / screen.ActualWidth, h / screen.ActualHeight);

                var rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(visual);

                var enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));

                int counter = 0;

                DateTime dateTime = DateTime.UtcNow.Date;
                var y = dateTime.ToString().Split(" ");
                var z = y[0].Split("/");
                string month = "";


                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(z[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        month = item;
                    }
                    counter++;
                }

                  using (var stm = File.Create(_Directory + @"\\storage\\Invoicing\\ROOM RENTAL - " + _FirstName + "\\" + _FirstName + " INVOICE " + _ID + " " + month + " " + z[1] + ".png"))
                 {
                     enc.Save(stm);
                 }

            }

        /// <summary>
        /// this is the function that sets the values of all the labels, its probably could be shortened as its a bit much
        /// </summary>
        public void fillingWork()
        {
            if (Roomrentals[0].Day == "")
            {
                lblRental.Visibility = Visibility.Hidden;
                lblDate.Visibility = Visibility.Hidden;
                lblFee.Visibility = Visibility.Hidden;
                lblDay.Visibility = Visibility.Hidden;

            }
            else
            {
                lblDay.Content = Roomrentals[0].Day;
                int Total = 0;
                Total = int.Parse(Roomrentals[0].Cost, CultureInfo.InvariantCulture.NumberFormat);



                lblFee.Content = "$" + Total.ToString();
                lblDay.Content = "(" + Roomrentals[0].Day + ")";


                _numbersCalc[0] = Total;

                var parts = Roomrentals[0].Date.Split("/");

                int counter = 0;
                string day = parts[0];

                char[] charArray = day.ToCharArray();
                string[] strArray = day.Select(x => x.ToString()).ToArray();
                if (strArray.Length == 2)
                {
                    if (strArray[1] == "1")
                    {
                        strArray[1] = "1st";
                    }
                    else if (strArray[1] == "2")
                    {
                        strArray[1] = "2nd";
                    }
                    else if (strArray[1] == "3")
                    {
                        strArray[1] = "3rd";
                    }
                    else
                    {
                        strArray[1] = strArray[1] + "th";
                    }
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        strArray[0] = "1st";
                    }
                    else if (strArray[0] == "2")
                    {
                        strArray[0] = "2nd";
                    }
                    else if (strArray[0] == "3")
                    {
                        strArray[0] = "3rd";
                    }
                    else
                    {
                        strArray[0] = strArray[0] + "th";
                    }
                }

                if(strArray[0] != "0")
                {
                    day = string.Join("", strArray);
                }
                else
                {
                    day = string.Join("", strArray.Skip(1));
                }

                if(parts[0] == "11")
                {
                    day = "11th";

                }
                if(parts[0] == "12")
                {
                    day = "12th";
                }
                if (parts[0] == "13")
                {
                    day = "13th";
                }

                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        lblDate.Content = item + " " + day;
                    }
                    counter++;
                }

            }
            if (Roomrentals[1].Day == "")
            {
                lblRental2.Visibility = Visibility.Hidden;
                lblDate2.Visibility = Visibility.Hidden;
                lblFee2.Visibility = Visibility.Hidden;
                lblDay2.Visibility = Visibility.Hidden;
            }
            else
            {
                lblDay2.Content = Roomrentals[1].Day;
                int Total = 0;

                Total = int.Parse(Roomrentals[1].Cost, CultureInfo.InvariantCulture.NumberFormat);

                lblFee2.Content = "$" + Total.ToString();
                lblDay2.Content = "(" + Roomrentals[1].Day + ")";

                _numbersCalc[1] = Total;

                var parts = Roomrentals[1].Date.Split("/");

                int counter = 0;
                string day = parts[0];

                char[] charArray = day.ToCharArray();
                string[] strArray = day.Select(x => x.ToString()).ToArray();
                if (strArray.Length == 2)
                {
                    if (strArray[1] == "1")
                    {
                        strArray[1] = "1st";
                    }
                    else if (strArray[1] == "2")
                    {
                        strArray[1] = "2nd";
                    }
                    else if (strArray[1] == "3")
                    {
                        strArray[1] = "3rd";
                    }
                    else
                    {
                        strArray[1] = strArray[1] + "th";
                    }
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        strArray[0] = "1st";
                    }
                    else if (strArray[0] == "2")
                    {
                        strArray[0] = "2nd";
                    }
                    else if (strArray[0] == "3")
                    {
                        strArray[0] = "3rd";
                    }
                    else
                    {
                        strArray[0] = strArray[0] + "th";
                    }
                }

                if (strArray[0] != "0")
                {
                    day = string.Join("", strArray);
                }
                else
                {
                    day = string.Join("", strArray.Skip(1));
                }

                if (parts[0] == "11")
                {
                    day = "11th";

                }
                if (parts[0] == "12")
                {
                    day = "12th";
                }
                if (parts[0] == "13")
                {
                    day = "13th";
                }

                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        lblDate2.Content = item + " " + day;
                    }
                    counter++;
                }

            }
            if (Roomrentals[2].Day == "")
            {
                lblRental3.Visibility = Visibility.Hidden;
                lblDate3.Visibility = Visibility.Hidden;
                lblFee3.Visibility = Visibility.Hidden;
                lblDay3.Visibility = Visibility.Hidden;
            }
            else
            {
                lblDay3.Content = Roomrentals[2].Day;
                int Total = 0;


                Total = int.Parse(Roomrentals[2].Cost, CultureInfo.InvariantCulture.NumberFormat);

                lblFee3.Content = "$" + Total.ToString();
                lblDay3.Content = "(" + Roomrentals[2].Day + ")";

                _numbersCalc[2] = Total;

                var parts = Roomrentals[2].Date.Split("/");

                int counter = 0;
                string day = parts[0];

                char[] charArray = day.ToCharArray();
                string[] strArray = day.Select(x => x.ToString()).ToArray();
                if (strArray.Length == 2)
                {
                    if (strArray[1] == "1")
                    {
                        strArray[1] = "1st";
                    }
                    else if (strArray[1] == "2")
                    {
                        strArray[1] = "2nd";
                    }
                    else if (strArray[1] == "3")
                    {
                        strArray[1] = "3rd";
                    }
                    else
                    {
                        strArray[1] = strArray[1] + "th";
                    }
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        strArray[0] = "1st";
                    }
                    else if (strArray[0] == "2")
                    {
                        strArray[0] = "2nd";
                    }
                    else if (strArray[0] == "3")
                    {
                        strArray[0] = "3rd";
                    }
                    else
                    {
                        strArray[0] = strArray[0] + "th";
                    }
                }

                if (strArray[0] != "0")
                {
                    day = string.Join("", strArray);
                }
                else
                {
                    day = string.Join("", strArray.Skip(1));
                }

                if (parts[0] == "11")
                {
                    day = "11th";

                }
                if (parts[0] == "12")
                {
                    day = "12th";
                }
                if (parts[0] == "13")
                {
                    day = "13th";
                }

                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        lblDate3.Content = item + " " + day;
                    }
                    counter++;
                }

            }
            if (Roomrentals[3].Day == "")
            {
                lblRental4.Visibility = Visibility.Hidden;
                lblDate4.Visibility = Visibility.Hidden;
                lblFee4.Visibility = Visibility.Hidden;
                lblDay4.Visibility = Visibility.Hidden;
            }
            else
            {
                lblDay4.Content = Roomrentals[3].Day;
                int Total = 0;

                Total = int.Parse(Roomrentals[3].Cost, CultureInfo.InvariantCulture.NumberFormat);




                lblFee4.Content = "$" + Total.ToString();
                lblDay4.Content = "(" + Roomrentals[3].Day + ")";

                _numbersCalc[3] = Total;

                var parts = Roomrentals[3].Date.Split("/");

                int counter = 0;
                string day = parts[0];

                char[] charArray = day.ToCharArray();
                string[] strArray = day.Select(x => x.ToString()).ToArray();
                if (strArray.Length == 2)
                {
                    if (strArray[1] == "1")
                    {
                        strArray[1] = "1st";
                    }
                    else if (strArray[1] == "2")
                    {
                        strArray[1] = "2nd";
                    }
                    else if (strArray[1] == "3")
                    {
                        strArray[1] = "3rd";
                    }
                    else
                    {
                        strArray[1] = strArray[1] + "th";
                    }
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        strArray[0] = "1st";
                    }
                    else if (strArray[0] == "2")
                    {
                        strArray[0] = "2nd";
                    }
                    else if (strArray[0] == "3")
                    {
                        strArray[0] = "3rd";
                    }
                    else
                    {
                        strArray[0] = strArray[0] + "th";
                    }
                }



                if (strArray[0] != "0")
                {
                    day = string.Join("", strArray);
                }
                else
                {
                    day = string.Join("", strArray.Skip(1));
                }

                if (parts[0] == "11")
                {
                    day = "11th";

                }
                if (parts[0] == "12")
                {
                    day = "12th";
                }
                if (parts[0] == "13")
                {
                    day = "13th";
                }

                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        lblDate4.Content = item + " " + day;
                    }
                    counter++;
                }

            }
            if (Roomrentals[4].Day == "")
            {
                lblRental5.Visibility = Visibility.Hidden;
                lblDate5.Visibility = Visibility.Hidden;
                lblFee5.Visibility = Visibility.Hidden;
                lblDay5.Visibility = Visibility.Hidden;
            }
            else
            {
                lblDay5.Content = Roomrentals[4].Day;
                int Total = 0;


                Total = int.Parse(Roomrentals[4].Cost, CultureInfo.InvariantCulture.NumberFormat);

                lblFee5.Content = "$" + Total.ToString();
                lblDay5.Content = "(" + Roomrentals[4].Day + ")";

                _numbersCalc[4] = Total;

                var parts = Roomrentals[0].Date.Split("/");

                int counter = 0;
                string day = parts[0];

                char[] charArray = day.ToCharArray();
                string[] strArray = day.Select(x => x.ToString()).ToArray();
                if (strArray.Length == 2)
                {
                    if (strArray[1] == "1")
                    {
                        strArray[1] = "1st";
                    }
                    else if (strArray[1] == "2")
                    {
                        strArray[1] = "2nd";
                    }
                    else if (strArray[1] == "3")
                    {
                        strArray[1] = "3rd";
                    }
                    else
                    {
                        strArray[1] = strArray[1] + "th";
                    }
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        strArray[0] = "1st";
                    }
                    else if (strArray[0] == "2")
                    {
                        strArray[0] = "2nd";
                    }
                    else if (strArray[0] == "3")
                    {
                        strArray[0] = "3rd";
                    }
                    else
                    {
                        strArray[0] = strArray[0] + "th";
                    }
                }

                if (strArray[0] != "0")
                {
                    day = string.Join("", strArray);
                }
                else
                {
                    day = string.Join("", strArray.Skip(1));
                }

                if (parts[0] == "11")
                {
                    day = "11th";

                }
                if (parts[0] == "12")
                {
                    day = "12th";
                }
                if (parts[0] == "13")
                {
                    day = "13th";
                }

                foreach (var item in _monthsArray)
                {

                    int x = int.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                    if (x == counter + 1)
                    {
                        lblDate5.Content = item + " " + day;
                    }
                    counter++;
                }

            }
        }

        /// <summary>
        /// this function grabs the names all the current invoices and finds the largest number then pluses one so we can correctly name the file.
        /// </summary>
        public void ID()
        {
            string[] pdfFiles = Directory.GetFiles(_Directory + "\\storage\\Invoicing\\ROOM RENTAL - " + _FirstName, "*.pdf")
                                     .Select(System.IO.Path.GetFileName)
                                     .ToArray();
            int pdflargestID = 0;

            foreach (var item in pdfFiles)
            {
                var pdfParts = item.Split(" ");
                if (pdfParts[2].ToInt32() > pdflargestID)
                {
                    int x = int.Parse(pdfParts[2], CultureInfo.InvariantCulture.NumberFormat);
                    pdflargestID = x;
                }
            }


            string[] pngFiles = Directory.GetFiles(_Directory + "\\storage\\Invoicing\\ROOM RENTAL - " + _FirstName, "*.png")
                                    .Select(System.IO.Path.GetFileName)
                                    .ToArray();

            int pnglargestID = 0;

            foreach (var item in pngFiles)
            {
                var pngParts = item.Split(" ");
                if (pngParts[2].ToInt32() > pnglargestID)
                {
                    int pngx = int.Parse(pngParts[2], CultureInfo.InvariantCulture.NumberFormat);
                    pnglargestID = pngx;
                }
            }

            int largestID = 0;

            if (pnglargestID > pdflargestID)
            {
                largestID = pnglargestID;
            }
            else
            {
                largestID = pdflargestID;
            }

            largestID++;
            if (largestID >= 10)
            {
                lblID.Content = "00" + largestID;
                _ID = "00" + largestID;
            }
            else
            {
                lblID.Content = "000" + largestID;
                _ID = "000" + largestID;
            }

        }


        /// <summary>
        /// Handles all the calculations for the lables
        /// </summary>
        public void Calculations()
        {
            float total = 0;
            foreach (float number in _numbersCalc)
            {
                total = total + number;         ///calculates the sub total
            }

            lblSubTotal.Content = "$" + total.ToString();

            double total2 = 0.1 * total + total;    ///this is the total plus gst
            double gst = total * 0.1;   ///calculates gst which is 10% through this option for some reason

            lblGST.Content = "$" + gst.ToString();



            lblInvTotal.Content = "$" + total2;

            DateTime dateTime = DateTime.UtcNow.Date;
            var y = dateTime.ToString().Split(" ");     ///just sets the current date to the top of file, idk why its here
            lblDateMain.Content = y[0];


        }

        /// <summary>
        /// allows for the creation of the array list of records/
        /// </summary>
        public class WorkO1
        {
            public string Day { get; set; }
            public string Date { get; set; }
            public string Cost { get; set; }
        }

    }
}
