using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ejejjopnik
{
    public static class SerDeser
    {
        public static void Serialize<T>(T data, string filePath)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }

        public static T Deserialize<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
    public class Note
    {
        public string TitleNote { get; set; }
        public string DescriptionNote { get; set; }
        public DateTime DateNote { get; set; }
    }


    public partial class MainWindow : Window
    {
        private string Path = "ejejopnik.json";
        private List<Note> AllNotes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            datepick.SelectedDate = DateTime.Today;
            Read();
            ShowNote();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            Note note = new Note
            {
                TitleNote = titleN.Text,
                DescriptionNote = descr.Text,
                DateNote = datepick.SelectedDate ?? DateTime.Today
            };
            AllNotes.Add(note);
            SerDeser.Serialize(AllNotes, Path);
            ShowNote();
            ClearLable();
        }
   
        private void ShowNote()
        {
            if (datepick.SelectedDate != null)
            {
                DateTime selectedDate = datepick.SelectedDate ?? DateTime.Today;
                List<Note> show = AllNotes.FindAll(note => note.DateNote.Date == selectedDate.Date);
                listboxNote.ItemsSource = show;
            }
        }

        private void ClearLable()
        {
            titleN.Text = " ";
            descr.Text = " ";
        }


        private void Read()
        {
            if (File.Exists(Path))
            {
                AllNotes = SerDeser.Deserialize<List<Note>>(Path);
            }
        }
        
        private void updateN_Click(object sender, RoutedEventArgs e)
        {
            Note select = (Note)listboxNote.SelectedItem;
            select.TitleNote = titleN.Text;
            select.DescriptionNote = descr.Text;

            SerDeser.Serialize(AllNotes, Path);
            ShowNote();
            ClearLable();
        }
       
        
        private void deleteN_Click(object sender, RoutedEventArgs e)
        {
            Note select = (Note)listboxNote.SelectedItem;
            AllNotes.Remove(select);
            SerDeser.Serialize(AllNotes, Path);
            ShowNote();
            ClearLable();
        }

        private void listboxNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxNote.SelectedItem != null) 
            {
                Note select = (Note)listboxNote.SelectedItem;
                titleN.Text = select.TitleNote;
                descr.Text = select.DescriptionNote;
            }
            
        }

        private void datepick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowNote();
        }
    }
}