using BalizaFacil.Models;
using System.ComponentModel;

namespace BalizaFacil.Services
{
    public interface IStorageService : INotifyPropertyChanged
    {
        string Address { get; set; }
        Car Car { get; set; }
        Cars Cars { get; set; }
        string Username { get; set; }
        string Birthday { get; set; }
        bool TrainingMode { get; set; }
        bool FinalStepFree { get; set; }
        string TotalStops { get; set; }
        double reactionTime { get; set; }
    }
}

