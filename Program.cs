using System;
using System.Reflection;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace awakedac_windows
{
    internal static class Program
    {
        private static NAudio.Wave.WasapiOut waveOut;
        private static NAudio.Wave.SampleProviders.SignalGenerator nullOut;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(NaudioAssembly);
            AwakeDAC();
            Application.ApplicationExit += (s, e) => ReleaseDAC();
            Application.Run(new Form1());

        }
        private static Assembly NaudioAssembly(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            switch (assemblyName) {
                case "NAudio.Core": return Assembly.Load(Properties.Resources.naudio_core);
                case "NAudio.Wasapi": return Assembly.Load(Properties.Resources.naudio_wasapi);
                default: return null;
            }
        }
        private static void AwakeDAC()
        {
            using (var deviceEnumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator())
            {
                var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                nullOut = new NAudio.Wave.SampleProviders.SignalGenerator() { Gain = 0 };
                waveOut = new NAudio.Wave.WasapiOut(device, AudioClientShareMode.Shared, true, 1000);
                waveOut.Init(nullOut);
                waveOut.Play();
            }
        }
        private static void ReleaseDAC()
        {
            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;
            nullOut = null;
        }
    }
}
