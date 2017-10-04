using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GameAudioBalancer
{
    public class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler();

        private static void Main(string[] args)
        {
            var balancer = new AudioBalancer();

            SetConsoleCtrlHandler(() =>
            {
                balancer.Dispose();

                Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");
                Thread.Sleep(5000); //simulate some cleanup delay
                Console.WriteLine("Cleanup complete");
                Application.Exit();
                Environment.Exit(-1);

                return true;
            }, true);

            Application.Run();
        }
    }
}