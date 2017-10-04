using System;
using System.Runtime.InteropServices;
using Gma.System.MouseKeyHook;

namespace GameAudioBalancer
{
    public class AudioBalancer
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public IKeyboardMouseEvents InputHook;

        public AudioBalancer()
        {
            VolumeMixer.SetApplicationVolume(5596, 50f);
        }

        public void Dispose()
        {
            InputHook.Dispose();
        }
    }
}