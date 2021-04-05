using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using BalizaFacil.Core;
using BalizaFacil.Droid.Services;
using BalizaFacil.Models;
using BalizaFacil.Services;
using Microsoft.AppCenter.Crashes;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

[assembly: Dependency(typeof(SoundPlayerService))]
namespace BalizaFacil.Droid.Services
{
    public class SoundPlayerService : ISoundPlayerService
    {


        private ISimpleAudioPlayer[] players = new ISimpleAudioPlayer[(int)VoiceType.None];
        private ToneGenerator ToneGenerator { get; set; } = new ToneGenerator(Android.Media.Stream.System, 100);
        private Thread BeepThread { get; set; }

        public bool Beeping { get => beeping; }
        protected MediaPlayer player;
        public DateTime defaultTime { get; set; }
        private volatile BeepSpeed speed;
        private volatile bool beeping;
        DateTime timeLastBeep = DateTime.Now;
        ApplicationStep currentStep = ApplicationStep.None;
        DateTime timeLastPlay = new DateTime();
        readonly AudioManager manager = AudioManager.FromContext(Android.App.Application.Context);
        private int MillisecondsToSleep
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
        public SoundPlayerService()
        {
            //player = new MediaPlayer();
            for (VoiceType voice = VoiceType.ApproachingEnd; voice < VoiceType.None; voice++)
            {
                try
                {
                    players[(int)voice] = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                    players[(int)voice].Load(GetStreamFromFile(Enum.GetName(typeof(VoiceType), voice)));
                }
                catch(Exception ef)
                {

                }
            }


            timeLastPlay = DateTime.Now;
            defaultTime = timeLastPlay;
            //[sergio] comentei para fazer testes sem volume exagerado
            //manager.SetStreamVolume(Stream.Music, manager.GetStreamMaxVolume(Stream.Music), VolumeNotificationFlags.Vibrate);
            //manager.SetStreamVolume(Stream.Alarm, manager.GetStreamMaxVolume(Stream.Alarm), VolumeNotificationFlags.Vibrate);
            //manager.SetStreamVolume(Stream.Ring, manager.GetStreamMaxVolume(Stream.Alarm), VolumeNotificationFlags.Vibrate);

        }

        private /*async*/ void Beep()
        {
            while (beeping)
            {
                if (speed != BeepSpeed.InsideGreen)
                    ToneGenerator.StartTone(Tone.CdmaAlertCallGuard, 30);
                else
                    ToneGenerator.StartTone(Tone.PropAck, 30);

                //[guilherme] tentei fazer com async, no começo percebi melhora, mas depois ficou na mesma.
                //await Task.Delay(MillisecondsToSleep);
                Thread.Sleep(MillisecondsToSleep);
                //timeLastBeep = DateTime.Now;
                //}
            }
        }

        public bool isPlaying(VoiceType type = VoiceType.None)
        {
            for (VoiceType c = VoiceType.ApproachingEnd; c < VoiceType.None; c++)
            {
                if (players[type == VoiceType.None ? (int)c : (int)type].IsPlaying)
                    return true;
                if (type != VoiceType.None)
                    return false;
            }
            return false;
        }
        public void PlayerStop()
        {
            for (VoiceType c = VoiceType.ApproachingEnd; c < VoiceType.None; c++)
            {
                if (players[(int)c].IsPlaying)
                {
                    players[(int)c].Stop();
                }
            }
        }
        public void PlaySound(VoiceType type)
        {
            try
            {
                if (isPlaying() && (type == VoiceType.ConclusionStep))
                {
                    PlayerStop();
                    players[(int)type].Play();
                    return;
                }
                if (isPlaying() && ((players[(int)VoiceType.ConclusionStep].IsPlaying) || (players[(int)VoiceType.Stop].IsPlaying)))
                    return;

                if (isPlaying())
                    PlayerStop();

                players[(int)type].Play();

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                //  BalizaFacil.App.Instance.UnhandledException(title, ex);

                Crashes.TrackError(ex);
            }
        }
        public void PlaySound(VoiceType type, double delayMilliseconds, ApplicationStep step)
        {
            try
            {
                if (currentStep != step)
                    timeLastPlay = defaultTime;

                TimeSpan lastPlay = DateTime.Now.Subtract(timeLastPlay);// timeLastPlay.Subtract(dt);
                //Debug.Print("Dentro 1º" + lastPlay.TotalMilliseconds + " - " + (lastPlay.TotalMilliseconds > delay).ToString()+
                //    "\n" + timeLastPlay.ToLongTimeString() + " -> " + DateTime.Now.ToLongTimeString());

                if (type == VoiceType.NotMeasuring && isPlaying())
                    return;

                if (lastPlay.TotalMilliseconds > delayMilliseconds)
                {
                    if (isPlaying() && ((players[(int)VoiceType.ConclusionStep].IsPlaying) || (players[(int)VoiceType.Stop].IsPlaying)))
                    {
                        //Debug.Print("1o IF");
                        timeLastPlay = DateTime.Now;
                        return;
                    }
                    if (isPlaying() && type == VoiceType.Stop && !(players[(int)VoiceType.Stop].IsPlaying))
                    {
                        //Debug.Print("2o IF");
                        PlayerStop();
                    }
                    else if ((players[(int)VoiceType.Stop].IsPlaying))
                    {
                        //Debug.Print("ElseIF");
                        timeLastPlay = DateTime.Now;
                        return;
                    }
                    else if (isPlaying())
                    {
                        PlayerStop();
                    }
                    //Debug.Print("Tocou - " + Enum.GetName(typeof(VoiceType), type));

                    players[(int)type].Play();
                    timeLastPlay = DateTime.Now;
                    currentStep = step;
                }


            }
            catch (Java.Lang.IllegalStateException ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
        public async void PlaySound(VoiceType type, bool Priority)
        {
            try
            {
                if (type == VoiceType.Stop)
                {
                    PlayerStop();
                    MainActivity.Instance.Playsong();
                }
                else
                    if (isPlaying())
                    return;
                else
                    players[(int)type].Play();
            }
            catch (Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);

            }
        }

        public void StopSound()
        {
            try
            {
                PlayerStop();

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        System.IO.Stream GetStreamFromFile(string filename)
        {
            try
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream($"BalizaFacil.Resources.Audio.{filename}.m4a");

                return stream;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return null;
        }


        public void StartBeep(BeepSpeed speed)
        {
            try
            {
                this.speed = speed;
                beeping = true;

                BeepThread = new Thread(Beep);
                BeepThread.Name = nameof(BeepThread);
                BeepThread.IsBackground = true;
                BeepThread.Priority = ThreadPriority.Lowest;

                BeepThread.Start();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ChangeBeepSpeed(BeepSpeed speed)
        {
            this.speed = speed;
        }

        public void StopBeep()
        {
            try
            {
                beeping = false;
                // [guilherme] teste - Antes ele abortava a thread fazendo com que lançasse uma exceção, desse modo ele reinicia a variavel
                if (BeepThread != null)
                {
                    BeepThread.Abort();
                    //BeepThread = new Thread(Beep);
                    //BeepThread.Name = nameof(BeepThread);
                    //BeepThread.IsBackground = true;
                    //BeepThread.Priority = ThreadPriority.Lowest;
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ResetPlayers()
        {
            players = new ISimpleAudioPlayer[(int)VoiceType.None];
            for (VoiceType c = VoiceType.ApproachingEnd; c < VoiceType.None; c++)
            {
                players[(int)c] = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                players[(int)c].Load(GetStreamFromFile(Enum.GetName(typeof(VoiceType), c)));
            }

        }

    }
}