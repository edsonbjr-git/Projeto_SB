using System;
using System.Reflection;
using System.Threading;
using AudioToolbox;
using BalizaFacil.Core;
using BalizaFacil.iOS.Services;
using BalizaFacil.Models;
using BalizaFacil.Services;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

[assembly: Dependency(typeof(SoundPlayerService))]
namespace BalizaFacil.iOS.Services
{
    public class SoundPlayerService : ISoundPlayerService
    {
        private ISimpleAudioPlayer Player { get; set; } = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
         
        
        private SystemSound ToneGenerator { get; set; } = new SystemSound(1071);
        private Thread beepThread { get; set; }
        private int millisecondsToSleep
        {
            get
            {
                switch (speed)
                {
                    case BeepSpeed.Slow:
                        return 900;
                    case BeepSpeed.Normal:
                        return 600;
                    case BeepSpeed.Fast:
                        return 300;
                    case BeepSpeed.VeryFast:
                        return 150;
                    case BeepSpeed.InsideGreen:
                        return 100;
                    default:
                        return 900;
                }
            }
        }

        public bool Beeping { get => beeping; }

        private volatile BeepSpeed speed;
        private volatile bool beeping;

        private void Beep()
        {
            while (beeping)
            {
                if (!Player.IsPlaying)

                    ToneGenerator.PlayAlertSound();
                Thread.Sleep(millisecondsToSleep);
            }
        }

        public void PlaySound(VoiceType type)
        {
            if (Player.IsPlaying)
                Player.Stop();

            string filename = Enum.GetName(typeof(VoiceType), type);
            Player.Load(GetStreamFromFile(filename));
            Player.Play();
        }

        System.IO.Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"BalizaFacil.Resources.Audio.{filename}.m4a");

            return stream;
        }

        public void StartBeep(BeepSpeed speed)
        {
            this.speed = speed;
            beeping = true;

            beepThread = new Thread(Beep);

            beepThread.Start();
        }

        public void ChangeBeepSpeed(BeepSpeed speed)
        {
            this.speed = speed;
        }

        public void StopBeep()
        {
            beeping = false;

            if (beepThread != null)
                beepThread.Abort();
        }

        public void StopSound()
        {
            throw new NotImplementedException();
        }

        public void PlaySound(VoiceType stop, double delay, ApplicationStep currentStep)
        {
            throw new NotImplementedException();
        }

        public void PlaySound(VoiceType stop, bool Priority)
        {
            throw new NotImplementedException();
        }

        public void ResetPlayers()
        {
            throw new NotImplementedException();
        }
    }
}