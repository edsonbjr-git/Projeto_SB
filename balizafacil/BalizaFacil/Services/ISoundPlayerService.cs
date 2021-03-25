using BalizaFacil.Core;
using BalizaFacil.Models;

namespace BalizaFacil.Services
{
    public interface ISoundPlayerService
    {
        void PlaySound(VoiceType type);
        void StopSound();
        void StartBeep(BeepSpeed speed);
        void ChangeBeepSpeed(BeepSpeed speed);
        void StopBeep();
        bool Beeping { get; }
        void PlaySound(VoiceType stop, double delay, ApplicationStep currentStep);
        void PlaySound(VoiceType stop, bool Priority);
        void ResetPlayers();

    }
}
