using System.Windows;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace HardDriveWaker_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private volatile bool isRunning;
        private string statusText;
        Thread thread = null;
        static readonly string FILE_NAME = "ca.poganatz.harddrivewaker.txt";

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (value != isRunning)
                {
                    isRunning = value;
                    NotifyRunningChanged();
                }
            }
        }
        public bool IsNotRunning { get { return !IsRunning; } }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Update the running flags for the buttons
        /// </summary>
        private void NotifyRunningChanged()
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("IsRunning"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsNotRunning"));
            }
        }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                if(value != statusText)
                {
                    statusText = value;
                    if(PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            IsRunning = false;
            StatusText = "Welcome!";
        }

        /// <summary>
        /// Start the thread to write to the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Awake_Button_Click(object sender, RoutedEventArgs e)
        {
            StatusText = "Running";
            IsRunning = true;
            thread = new Thread(ExecuteInBackground);
            thread.Start();
        }

        /// <summary>
        /// Stop writing the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            StatusText = "Not Running";
            IsRunning = false;
            thread.Join();
        }

        /// <summary>
        /// Writes the file to keep the hard disk awake until IsRunning is set to false
        /// </summary>
        void ExecuteInBackground()
        {
            while (IsRunning)
            {
                using (FileStream fs = File.OpenWrite(FILE_NAME))
                {
                    byte[] data = new byte[] { (byte)'H', (byte)'E', (byte)'L', (byte)'L', (byte)'O', (byte)' ', (byte)'W', (byte)'O',
                    (byte)'R', (byte)'L',(byte) 'D', (byte)'!'};
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Stops the execution of file writing and deletes the file if necessary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(thread != null)
            {
                if(thread.IsAlive)
                {
                    IsRunning = false;
                    thread.Join();
                }
                File.Delete(FILE_NAME);
            }
        }
    }
}
