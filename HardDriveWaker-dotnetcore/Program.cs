using System;
using System.IO;
using System.Threading;
namespace HardDriveWaker_dotnetcore
{

    class Program
    {
        static volatile bool quit = false;
        static readonly string FILE_NAME = "ca.poganatz.harddrivewaker.txt";
        static void ExecuteInBackground()
        { 
            while(!quit)
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

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Alexander's Hard Drive Waker!");
            Thread thread = new Thread(ExecuteInBackground);
            thread.Start();
            Console.WriteLine("Press enter to exit!");
            Console.ReadLine();
            quit = true;
            thread.Join();
            File.Delete(FILE_NAME);
        }
    }
}
