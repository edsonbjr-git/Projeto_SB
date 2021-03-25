using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using BalizaFacil.Droid.Services;
using BalizaFacil.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]
namespace BalizaFacil.Droid.Services
{
    public class LocationService : ILocationService
    {

        LocationSettingsRequest.Builder Builder;
        GoogleApiClient GoogleApiClient;

        public LocationService()
        {
            GoogleApiClient = new GoogleApiClient.Builder(Android.App.Application.Context)
                                        .AddApi(LocationServices.API)
                                        .Build();

            GoogleApiClient.Connect();

            LocationRequest locationRequest = LocationRequest.Create()
                                                .SetPriority(LocationRequest.PriorityHighAccuracy)
                                                .SetInterval(100000)
                                                .SetFastestInterval(100000 / 2);

            Builder = new LocationSettingsRequest.Builder()
                                                .AddLocationRequest(locationRequest)
                                                .SetAlwaysShow(true);
        }


        public async Task RequestEnableGPS()
        {
            try
            {
                var result = (LocationSettingsResult)(await LocationServices.SettingsApi.CheckLocationSettings(GoogleApiClient, Builder.Build()));

                if (result.Status.StatusCode != LocationSettingsStatusCodes.Success)
                    result.Status.StartResolutionForResult(MainActivity.Instance, 0x1);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
    }
}