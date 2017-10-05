using System;
using System.Collections.Generic;

// ReSharper disable StringIndexOfIsCultureSpecific.1

public partial class VolumeMixer
{
    public struct MixerEntry
    {
        public string Id;
        public int PId;

        public MixerEntry(string id = "not set", int pId = -1)
        {
            Id = id;
            PId = pId;
        }
    }

    public static MixerEntry[] GetDefaultMixerEntries()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        var deviceEnumerator = (IMMDeviceEnumerator) new MMDeviceEnumerator();
        IMMDevice speakers;
        deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

        var iIdIAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
        object o;
        speakers.Activate(ref iIdIAudioSessionManager2, 0, IntPtr.Zero, out o);
        var mgr = (IAudioSessionManager2) o;

        IAudioSessionEnumerator sessionEnumerator;
        mgr.GetSessionEnumerator(out sessionEnumerator);
        int count;
        sessionEnumerator.GetCount(out count);

        var entries = new MixerEntry[count];

        for (var i = 0; i < count; i++)
        {
            IAudioSessionControl2 ctl;
            sessionEnumerator.GetSession(i, out ctl);

            string idRet;
            ctl.GetSessionIdentifier(out idRet);
            int pIdRet;
            ctl.GetProcessId(out pIdRet);

            entries[i] = new MixerEntry(idRet, pIdRet);
        }

        return entries;
    }

    public static MixerEntry[] GetPresentableMixerEntries()
    {
        var list = new List<MixerEntry>();

        foreach (var entry in GetDefaultMixerEntries())
        {
            if (!entry.Id.Contains(".exe"))
                continue;

            try
            {
                var i = entry.Id.IndexOf(".exe") + 4;
                var s = entry.Id.Remove(i);
                var array = s.Split('\\');

                var final = array[array.Length - 2] + "\\" + array[array.Length - 1];

                list.Add(new MixerEntry(final, entry.PId));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Failed to make audio mixer string presentable");
                throw;
            }
        }

        return list.ToArray();
    }
}