using sensortest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace sensortest.Services
{
    public class SensorValuesRepository : ISensorValuesRepository
    {
        public async Task<IEnumerable<SensorValueItem>> GetSensorValuesAsync()
        {
            var items = new List<SensorValueItem>();

            items.Add(await GetLocationAsync());
            items.AddRange(GetBatteryInfoAsync());

            return await Task.FromResult(items);
        }

        private IEnumerable<SensorValueItem> GetBatteryInfoAsync()
        {
            var state = Battery.State.ToString();
            var level = Battery.ChargeLevel.ToString();
            var source = Battery.PowerSource.ToString();

            return new SensorValueItem[]
            {
                new SensorValueItem{ Name = "Battery State:", Value = state },
                new SensorValueItem{ Name = "Battery Level:", Value = level },
                new SensorValueItem{ Name = "Battery Source:", Value = source }
            };

        }

        private async Task<SensorValueItem> GetLocationAsync()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var settingItem = new SensorValueItem();
            settingItem.Name = "Location";
            settingItem.Value = "Not Set";
            try
            {
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    string mockLocation = string.Empty;
                    if (location.IsFromMockProvider)
                    {
                        mockLocation = "(Mocked)";
                    }
                    settingItem.Value = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude} {mockLocation}";
                    return settingItem;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                settingItem.Value = $"Handle not supported on device exception {fnsEx.Message}";
                return settingItem;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                settingItem.Value = $"Handle not enabled on device exception {fneEx.Message}";
                return settingItem;
            }
            catch (PermissionException pEx)
            {
                settingItem.Value = $"Handle permission exception {pEx.Message}";
                return settingItem;
            }
            catch (Exception ex)
            {
                settingItem.Value = $"Unable to get location {ex.Message}";
                return settingItem;
            }

            return settingItem;
        }



    }
}
