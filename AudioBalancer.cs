using System;
using System.Linq;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Reactive.Bindings;

namespace GameAudioBalancer
{
    public class AudioBalancer
    {
        private readonly IKeyboardMouseEvents _inputHook;

        public AudioBalancer()
        {
            _inputHook = Hook.GlobalEvents();

            Console.WriteLine("Welcome!\n");
            Console.WriteLine("The following applications were found via your default playback device: ");

            var entries = VolumeMixer.GetPresentableMixerEntries();

            for (var i = 0; i < entries.Length; i++)
                Console.WriteLine(i + " - " + entries[i].Id);

            Console.WriteLine(
                "\nPlease enter the number of the application you would like to balance against the rest.");

            int? choice = null;

            while (choice == null)
            {
                var userInput = Console.ReadLine();

                try
                {
                    choice = entries.ToList().IndexOf(entries[int.Parse(userInput)]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, your input did not match a device. Please try again.");
                }
            }

            Console.Clear();

            var selected = entries[choice.Value];
            var selectedShortName = selected.Id.Split('\\')[1];
            entries = VolumeMixer.GetDefaultMixerEntries();

            var currentBalance = new ReactiveProperty<int>();

            _inputHook.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Up && currentBalance.Value < 50)
                    currentBalance.Value++;

                if (args.KeyCode == Keys.Down && currentBalance.Value > -50)
                    currentBalance.Value--;
            };

            currentBalance.Subscribe(i =>
            {
                VolumeMixer.SetApplicationVolume(selected.PId, 50 + i);

                foreach (var entry in entries)
                {
                    if (entry.PId == selected.PId)
                        continue;

                    VolumeMixer.SetApplicationVolume(entry.PId, 50 - i);
                }

                Console.Clear();
                Console.WriteLine("Balancing!");
                Console.WriteLine("\n");
                Console.WriteLine("Press arrow-up to shift focus towards " + selectedShortName);
                Console.WriteLine("Press arrow-down to shift focus towards the non selected apps");
                Console.WriteLine("\n");
                Console.WriteLine(selectedShortName + ": " + (50 + i) + "\t" + "Others: " + (50 - i));
                Console.WriteLine("\n");
                Console.WriteLine("Press CTRL-C or just close the window to exit");
            });
        }

        public void Dispose()
        {
            _inputHook.Dispose();
        }
    }
}