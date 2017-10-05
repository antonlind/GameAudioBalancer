using System;
using Gma.System.MouseKeyHook;
using Reactive.Bindings;

namespace GameAudioBalancer
{
    public class AudioBalancer
    {
        private readonly IKeyboardMouseEvents _inputHook;
        private readonly ReactiveProperty<string> _asd = new ReactiveProperty<string>();

        public AudioBalancer()
        {
            _inputHook = Hook.GlobalEvents();

//            foreach (var entry in VolumeMixer.GetPresentableMixerEntries())
//            {
//                Console.WriteLine(entry.Id);
//            }

            _inputHook.KeyDown += (sender, args) => { _asd.Value = args.KeyCode.ToString(); };
            _asd.Subscribe(Console.WriteLine);
        }

        public void Dispose()
        {
            _inputHook.Dispose();
        }
    }
}