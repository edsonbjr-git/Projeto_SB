using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BalizaFacil.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Xamarin.Essentials;

namespace BalizaFacil.Library
{
    public class LogAppCenter
    {
        private static CancellationTokenSource cts;

        public LogAppCenter()
        {
        }

        public static async Task TrackEventAsync(string sessao, IDictionary<string,string> dados )
        {
            try
            {
                dados = dados ?? new Dictionary<string, string>();

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
#if DEBUG
                    dados.Add("Latitude", location.Latitude.ToString());
                    dados.Add("Longitude", location.Latitude.ToString());
                    dados.Add("Altitude", location.Latitude.ToString());


                    LogBaliza logBaliza = new LogBaliza();
                    logBaliza.Id = Guid.NewGuid().ToString();
                    logBaliza.Data = DateTime.Now;
                    logBaliza.Latitude = location.Latitude;
                    logBaliza.Lontitude = location.Longitude;
                    logBaliza.Altitude = location.Longitude;
                    logBaliza.ModeloCelular = Xamarin.Essentials.DeviceInfo.Manufacturer + " - " + Xamarin.Essentials.DeviceInfo.Model;
                    logBaliza.VersaoAndroid =  Xamarin.Essentials.DeviceInfo.VersionString;
                    logBaliza.Inicio = DateTime.Now.TimeOfDay;
                    logBaliza.Fim = DateTime.Now.AddHours(2).TimeOfDay;

                   var result= await App.DataStoreContainer.LogBalizaStore.AddItemAsync(logBaliza);



                    Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
#endif
                }

                dados.Add("Data", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                Analytics.TrackEvent(sessao, dados);

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }


            Analytics.TrackEvent(sessao, dados);
        }
    }
}
