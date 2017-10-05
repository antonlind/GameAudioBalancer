using System;
using Gma.System.MouseKeyHook;

namespace GameAudioBalancer
{
    public class AudioBalancer
    {
        public IKeyboardMouseEvents InputHook;

        public AudioBalancer()
        {
            foreach (var entry in VolumeMixer.GetPresentableMixerEntries())
            {
                Console.WriteLine(entry.Id);
            }
        }

        public void Dispose()
        {
            InputHook.Dispose();
        }
    }
}