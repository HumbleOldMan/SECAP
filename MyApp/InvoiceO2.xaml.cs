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


/// <summary>
/// This window is the Option 2 invoice that is created from passing data from Page1
/// Basically exactly the same as InvoiceO1 with bit more calculation and im to lazy to document it :/
/// but i will structure the code the same
/// </summary>



namespace MyApp
{
    public partial class InvoiceO2 : Window
    {
        string _FullName;     ///define the variable before
        string _FirstName;
        string _ABN;
        string _Percentage;
        string _Directory = null;
        string _ID = null;

        List<work> _WorkDays = new List<work>();
        string[] _monthsArray = new string[11] { "Januaray", "Febuary", "March", "April", "May", "June", "July", "August", "September", "November", "December" };
        float[] _numbersCalc = new float[5];




        public InvoiceO2(
            string Firstname,
            string LastName,
            string ABN,
            string Percent,
            string smallClient,
            string amtSmallClient,
            string fullClient,
            string amtFullClient,
            string Date,
            string smallClient1,
            string amtSmallClient1,
            string fullClient1,
            string amtFullClient1,
            string Date1,
            string smallClient2,
            string amtSmallClient2,
            string fullClient2,
            string amtFullClient2,
            string Date2,
            string smallClient3,
            string amtSmallClient3,
            string fullClient3,
            string amtFullClient3,
            string Date3,
            string smallClient4,
            string amtSmallClient4,
            string fullClient4,
            string amtFullClient4,
            string Date4
            ) ///add fields here to grap the data
        {
            InitializeComponent();
            _FullName = Firstname + " " + LastName;           ///update the variable
            _FirstName = Firstname;
            _ABN = ABN;
            _Percentage = Percent;

            _WorkDays.Add(new work()
            {
                smallClient = smallClient,
                amtSmallClient = amtSmallClient,
                fullClient = fullClient,
                amtFullClient = amtFullClient,
                Date = Date

            });
            _WorkDays.Add(new work()
            {
                smallClient = smallClient1,
                amtSmallClient = amtSmallClient1,
                fullClient = fullClient1,
                amtFullClient = amtFullClient1,
                Date = Date1

            });
            _WorkDays.Add(new work()
            {
                smallClient = smallClient2,
                amtSmallClient = amtSmallClient2,
                fullClient = fullClient2,
                amtFullClient = amtFullClient2,
                Date = Date2

            });
            _WorkDays.Add(new work()
            {
                smallClient = smallClient3,
                amtSmallClient = amtSmallClient3,
                fullClient = fullClient3,
                amtFullClient = amtFullClient3,
                Date = Date3

            });
            _WorkDays.Add(new work()
            {
                smallClient = smallClient4,
                amtSmallClient = amtSmallClient4,
                fullClient = fullClient4,
                amtFullClient = amtFullClient4,
                Date = Date4

            });

            RelativePathway();
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        }

        public System.Windows.SizeToContent SizeToContent { get; set; }

        private void ContentRenderedEvent(object sender, EventArgs e)
        {
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            fillingWork();
            Calculations();
            ID();

            Image.Source = new BitmapImage(
    new Uri(_Directory + @"\\storage\\logo.jpg"));


            lblPercentage.Content = _Percentage;
            lblName1.Content = _FullName;
            lblABN.Content = "ABN: " + _ABN;


            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to create this invoice?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                createImage();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

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

            using (var stm = File.Create(_Directory + @"\\storage\\Invoicing\\OPTION 2 - " + _FirstName + "\\" + _FirstName + " INVOICE " + _ID + " " + month + " " + z[1] + ".png"))
            {
                enc.Save(stm);
            }

        }

        /// <summary>
        /// Filling Work fills all days work that the Psych did and if the day is empty then its hidden, i didnt really do it the most efficient way. I'll
        /// </summary>

        public void fillingWork()
        {
            if (_WorkDays[0].smallClient == "")
            {
                lblFullClient.Visibility = Visibility.Hidden;
                lblAmtFullClient.Visibility = Visibility.Hidden;
                lblSmallClient.Visibility = Visibility.Hidden;
                lblAmtSmallClient.Visibility = Visibility.Hidden;
                lblDate.Visibility = Visibility.Hidden;
                lblClients.Visibility = Visibility.Hidden;
                lblMoreClients.Visibility = Visibility.Hidden;
                lblFee.Visibility = Visibility.Hidden;
            }
            else
            {
                lblFullClient.Content = _WorkDays[0].fullClient;
                lblAmtFullClient.Content = _WorkDays[0].amtFullClient;
                lblSmallClient.Content = _WorkDays[0].smallClient;
                lblAmtSmallClient.Content = _WorkDays[0].amtSmallClient;
                float Calc = float.Parse(_WorkDays[0].amtFullClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[0].fullClient, CultureInfo.InvariantCulture.NumberFormat);
                float Calc1 = float.Parse(_WorkDays[0].amtSmallClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[0].smallClient, CultureInfo.InvariantCulture.NumberFormat);
                float Total = Calc + Calc1;
                Math.Round(Convert.ToDecimal(Total), 2);
                lblFee.Content = "$" + Total.ToString();
                _numbersCalc[0] = Total;

                var parts = _WorkDays[0].Date.Split("/");

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
                        lblDate.Content = item + " " + day;
                    }
                    counter++;
                }

            }
            if (_WorkDays[1].smallClient == "")
            {
                lblFullClient2.Visibility = Visibility.Hidden;
                lblAmtFullClient2.Visibility = Visibility.Hidden;
                lblSmallClient2.Visibility = Visibility.Hidden;
                lblAmtSmallClient2.Visibility = Visibility.Hidden;
                lblDate2.Visibility = Visibility.Hidden;
                lblClients2.Visibility = Visibility.Hidden;
                lblMoreClients2.Visibility = Visibility.Hidden;
                lblFee2.Visibility = Visibility.Hidden;
            }
            else
            {
                lblFullClient2.Content = _WorkDays[1].fullClient;
                lblAmtFullClient2.Content = _WorkDays[1].amtFullClient;
                lblSmallClient2.Content = _WorkDays[1].smallClient;
                lblAmtSmallClient2.Content = _WorkDays[1].amtSmallClient;
                lblDate2.Content = _WorkDays[1].Date;
                float Calc = float.Parse(_WorkDays[1].amtFullClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[1].fullClient, CultureInfo.InvariantCulture.NumberFormat);
                float Calc1 = float.Parse(_WorkDays[1].amtSmallClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[1].smallClient, CultureInfo.InvariantCulture.NumberFormat);
                float Total = Calc + Calc1;
                Math.Round(Convert.ToDecimal(Total), 2);
                lblFee2.Content = "$" + Total.ToString();
                _numbersCalc[1] = Total;
                var parts = _WorkDays[1].Date.Split("/");

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
            if (_WorkDays[2].smallClient == "")
            {
                lblFullClient3.Visibility = Visibility.Hidden;
                lblAmtFullClient3.Visibility = Visibility.Hidden;
                lblSmallClient3.Visibility = Visibility.Hidden;
                lblAmtSmallClient3.Visibility = Visibility.Hidden;
                lblDate3.Visibility = Visibility.Hidden;
                lblClients3.Visibility = Visibility.Hidden;
                lblMoreClients3.Visibility = Visibility.Hidden;
                lblFee3.Visibility = Visibility.Hidden;
            }
            else
            {
                lblFullClient3.Content = _WorkDays[2].fullClient;
                lblAmtFullClient3.Content = _WorkDays[2].amtFullClient;
                lblSmallClient3.Content = _WorkDays[2].smallClient;
                lblAmtSmallClient3.Content = _WorkDays[2].amtSmallClient;
                lblDate3.Content = _WorkDays[2].Date;
                float Calc = float.Parse(_WorkDays[2].amtFullClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[2].fullClient, CultureInfo.InvariantCulture.NumberFormat);
                float Calc1 = float.Parse(_WorkDays[2].amtSmallClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[2].smallClient, CultureInfo.InvariantCulture.NumberFormat);
                float Total = Calc + Calc1;
                Math.Round(Convert.ToDecimal(Total), 2);
                lblFee3.Content = "$" + Total.ToString();
                _numbersCalc[2] = Total;
                var parts = _WorkDays[2].Date.Split("/");

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
            if (_WorkDays[3].smallClient == "")
            {
                lblFullClient4.Visibility = Visibility.Hidden;
                lblAmtFullClient4.Visibility = Visibility.Hidden;
                lblSmallClient4.Visibility = Visibility.Hidden;
                lblAmtSmallClient4.Visibility = Visibility.Hidden;
                lblDate4.Visibility = Visibility.Hidden;
                lblClients4.Visibility = Visibility.Hidden;
                lblMoreClients4.Visibility = Visibility.Hidden;
                lblFee4.Visibility = Visibility.Hidden;
            }
            else
            {
                lblFullClient4.Content = _WorkDays[3].fullClient;
                lblAmtFullClient4.Content = _WorkDays[3].amtFullClient;
                lblSmallClient4.Content = _WorkDays[3].smallClient;
                lblAmtSmallClient4.Content = _WorkDays[3].amtSmallClient;
                lblDate4.Content = _WorkDays[3].Date;
                float Calc = float.Parse(_WorkDays[3].amtFullClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[3].fullClient, CultureInfo.InvariantCulture.NumberFormat);
                float Calc1 = float.Parse(_WorkDays[3].amtSmallClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[3].smallClient, CultureInfo.InvariantCulture.NumberFormat);
                float Total = Calc + Calc1;
                Math.Round(Convert.ToDecimal(Total), 2);
                lblFee4.Content = "$" + Total.ToString();
                _numbersCalc[3] = Total;
                var parts = _WorkDays[3].Date.Split("/");

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
            if (_WorkDays[4].smallClient == "")
            {
                lblFullClient5.Visibility = Visibility.Hidden;
                lblAmtFullClient5.Visibility = Visibility.Hidden;
                lblSmallClient5.Visibility = Visibility.Hidden;
                lblAmtSmallClient5.Visibility = Visibility.Hidden;
                lblDate5.Visibility = Visibility.Hidden;
                lblClients5.Visibility = Visibility.Hidden;
                lblMoreClients5.Visibility = Visibility.Hidden;
                lblFee5.Visibility = Visibility.Hidden;
            }
            else
            {
                lblFullClient5.Content = _WorkDays[4].fullClient;
                lblAmtFullClient5.Content = _WorkDays[4].amtFullClient;
                lblSmallClient5.Content = _WorkDays[4].smallClient;
                lblAmtSmallClient5.Content = _WorkDays[4].amtSmallClient;
                lblDate5.Content = _WorkDays[4].Date;
                float Calc = float.Parse(_WorkDays[4].amtFullClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[4].fullClient, CultureInfo.InvariantCulture.NumberFormat);
                float Calc1 = float.Parse(_WorkDays[4].amtSmallClient, CultureInfo.InvariantCulture.NumberFormat) * float.Parse(_WorkDays[4].smallClient, CultureInfo.InvariantCulture.NumberFormat);
                float Total = Calc + Calc1;
                Math.Round(Convert.ToDecimal(Total), 2);
                lblFee5.Content = "$" + Total.ToString();
                _numbersCalc[4] = Total;
                var parts = _WorkDays[4].Date.Split("/");

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

        public void ID()
        {
            string[] pdfFiles = Directory.GetFiles(_Directory + "\\storage\\Invoicing\\OPTION 2 - " + _FirstName, "*.pdf")
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


            string[] pngFiles = Directory.GetFiles(_Directory + "\\storage\\Invoicing\\OPTION 2 - " + _FirstName, "*.png")
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

        public void Calculations()
        {
            var parts = _Percentage.Split("/");
            float total = 0;
            foreach (float number in _numbersCalc)
            {
                total = total + number;
            }
            Math.Round(Convert.ToDecimal(total), 2);
            lblSubTotal.Content = "$" + total.ToString();
            lblGST.Content = "x " + parts[0] + "% - GST:";

            double cut = float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat) / 100;
            double total2 = cut * total;
            total2 = Math.Round(total2, 2);


            double gst = total2 * 0.0909;
            double percgst = total2 - gst;
            percgst = Math.Round(percgst, 2);
            gst = Math.Round(gst, 2);
            lblPercGST.Content = "$" + percgst.ToString();
            lblGSTCalc.Content = "$" + gst.ToString();



            lblInvTotal.Content = "$" + total2;

            DateTime dateTime = DateTime.UtcNow.Date;
            var y = dateTime.ToString().Split(" ");
            lblDateMain.Content = y[0];


        }

        public class work
        {
            public string smallClient { get; set; }
            public string amtSmallClient { get; set; }
            public string fullClient { get; set; }
            public string amtFullClient { get; set; }
            public string Date { get; set; }

        }


    }
}
