using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfApp3
{

    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }


    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);


      

        StringDataSource _dataSource;
        public StringDataSource dataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                listBox.ItemsSource = value.data;
            }
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        

        
        public Window1()
        {
            InitializeComponent();

            saveFileDialog.DefaultExt = ".xml";
            openFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML Files|*.xml|All Files|*.*";
            openFileDialog.Filter = "XML Files|*.xml|All Files|*.*";

            dataSource = new StringDataSource();
            dataSource.data.Add(new Student("Pavel"));
            dataSource.data.Add(new Student("Ivan")); 
        }
       

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //dataSource.data.Add(new Student(Guid.NewGuid().ToString()));
            NewStudent newStudentDialog = new NewStudent();
            //newStudentDialog.ShowDialog();
            //dataSource.data.Add(newStudentDialog.Student);

            if (newStudentDialog.ShowDialog() == true)
            {
                dataSource.data.Add(newStudentDialog.Student);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            dataSource.data.Remove(listBox.SelectedItem as Student);
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (saveFileDialog.ShowDialog() == true)
            {

                var serializer = new XmlSerializer(typeof(StringDataSource));

                using (XmlWriter fs = XmlWriter.Create(saveFileDialog.FileName))
                {

                    serializer.Serialize(fs, dataSource);

                    Debug.WriteLine("Объект сериализован");
                }
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                var serializer = new XmlSerializer(typeof(StringDataSource));
                using (XmlReader fs = XmlReader.Create(openFileDialog.FileName))
                {
                    
                    dataSource = serializer.Deserialize(fs) as StringDataSource;
                }
            }
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            dataSource = new StringDataSource();
        }
    }
}
