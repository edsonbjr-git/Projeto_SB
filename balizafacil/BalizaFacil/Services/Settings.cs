using System;
namespace BalizaFacil.Services
{
    public static class Settings
    {
    
        public static string FirebaseAuthJson
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(FirebaseAuthJson), string.Empty);
            set => Xamarin.Essentials.Preferences.Set(nameof(FirebaseAuthJson), value);
        }
    }
}
