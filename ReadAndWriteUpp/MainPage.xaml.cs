using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SharedLibrary.Models;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Xml;
using Windows.Storage.Streams;
using SharedLibrary.Helpers;
using System.Collections.ObjectModel;
using CsvHelper;
using System.Xml.Linq;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReadAndWriteUpp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
        private ViewModel viewModel { get; set; }
        private ObservableCollection<string> CsvRows = new ObservableCollection<string>();
     
       
        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new ViewModel();

        }


        // Write to Json File
        private async Task CreatefileAsyncJson()
        {

            await storageFolder.CreateFileAsync("mystory.json", CreationCollisionOption.ReplaceExisting);
        }
        private async Task WriteTofileAsyncJson()
        {

            StorageFile file = await storageFolder.GetFileAsync("mystory.json");
            var person = new Person(tbFirstName.Text, tbLastName.Text, tbAge.Text, tbCountry.Text, tbCity.Text, tbJob.Text);

            var list = new List<Person>() { person };
            var json = JsonConvert.SerializeObject(list);

            await FileIO.WriteTextAsync(file, json);

        }
        private void btnJson_Click(object sender, RoutedEventArgs e)
        {

            CreatefileAsyncJson().GetAwaiter();
            WriteTofileAsyncJson().GetAwaiter();
        }


        // Write to Csv File
        private async void btnCsv_Click(object sender, RoutedEventArgs e)
        {
            await storageFolder.CreateFileAsync("NewUser.csv", CreationCollisionOption.ReplaceExisting);
            StorageFile file = await storageFolder.GetFileAsync("NewUser.csv");

            var text1 = tbFirstName.Text;
            var text2 = tbLastName.Text;
            var text3 = tbAge.Text;
            var text4 = tbCountry.Text;
            var text5 = tbCity.Text;
            var text6 = tbJob.Text;

            var r = new Person(text1, text2, text3, text4, text5, text6);

            var content = $"{r.FirstName} , {r.LastName} , {r.Age} , {r.Country} , {r.City} , {r.Jobb}";

            var lines = new List<string>() { content };

            await FileIO.WriteLinesAsync(file, lines);

        }

        // Write to Txt file
        private async void btnTxt_Click(object sender, RoutedEventArgs e)
        {
            await storageFolder.CreateFileAsync("newUser.txt", CreationCollisionOption.ReplaceExisting);
            StorageFile file = await storageFolder.GetFileAsync("newUser.txt");
            var person = new Person(tbFirstName.Text, tbLastName.Text, tbAge.Text, tbCountry.Text, tbCity.Text, tbJob.Text);

            var content = $"{person.FirstName} {person.LastName} {person.Age}  {person.Country}  {person.City}  {person.Jobb}";

            var lines = new List<string>() { content };

            await FileIO.WriteLinesAsync(file, lines);



        }

        // Write to Xml file
        private async void btnXml_Click(object sender, RoutedEventArgs e)
        {
            await storageFolder.CreateFileAsync("newUser.xml", CreationCollisionOption.ReplaceExisting);
            StorageFile file = await storageFolder.GetFileAsync("newUser.xml");

            using (IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                System.IO.Stream s = writeStream.AsStreamForWrite();
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = ("  "),
                    CloseOutput = true
                };
                settings.Async = true;

                var person = new Person(tbFirstName.Text, tbLastName.Text, tbAge.Text, tbCountry.Text, tbCity.Text, tbJob.Text);

                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(s, settings))
                {
                    writer.WriteStartElement($"NewUser");
                    writer.WriteElementString($"FullName", $"{person.FullName}");
                    writer.WriteElementString($"Age", $"{person.Age}");
                    writer.WriteElementString($"Country", $"{person.Country}");
                    writer.WriteElementString($"City", $"{person.City}");
                    writer.WriteElementString($"Jobb", $"{person.Jobb}");

                    writer.Flush();
                    await writer.FlushAsync();
                }
            }
        }




        // Read From Files

        // Read from Json
        public async Task ReadFromJSonFile(string fileName)
        {

            var persons = JsonConvert.DeserializeObject<ObservableCollection<Person>>(await ReadWriteHelp.ReadfromFileAsync(fileName));

            foreach (var person in persons)
            {
                viewModel.Persons.Add(person);
            }
        }
        private  void btnJsonfile_Click_1(object sender, RoutedEventArgs e)
        {

            ReadFromJSonFile("mystory.json").GetAwaiter();

        }


        // Read from csv
        private async void btnCsvfile_Click(object sender, RoutedEventArgs e)
        {

           

            var items = new List<Person>();

         
            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            StorageFile file = await storageFolder.GetFileAsync("newUser.csv");

            var lines = await FileIO.ReadLinesAsync(file);

            foreach(var line in lines)
            {
                var data = line.Split(',');

                items.Add(new Person()
                {
                    FirstName = data[0],
                    LastName = data[1],
                    Age = data[2],
                    Country = data[3],
                    City = data[4],
                    Jobb = data[5]

                });
            }

            lvPerson.ItemsSource = items;

            

        }

        // Read from Xml
        private async void btnXmlfile_Click(object sender, RoutedEventArgs e)
        {

            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            StorageFile file = await storageFolder.GetFileAsync("newUser.xml");

            using (var stream = await file.OpenStreamForReadAsync())
            {
                var doc = new XmlDocument();
                doc.Load(stream);


                XmlNodeList nodes = doc.GetElementsByTagName("NewUser");
                foreach (XmlNode node in nodes)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        lvXml.Items.Add(child.InnerText);
                    }

                }
            }


            


        }

 
    }
    
}


                
                            


    

