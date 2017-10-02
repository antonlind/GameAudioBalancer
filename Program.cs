using System;
using AudioSwitcher.AudioApi.CoreAudio;

namespace GameAudioBalancer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var controller = new CoreAudioController();

            foreach (var device in controller.GetDevices())
            {
                Console.Write(device.Name);
            }

            Console.Write("Done");
        }
    }
}