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

            //items.Add(new SensorValueItem { Name = "AAA", Value = "111" });
            //items.Add(new SensorValueItem { Name = "BBB", Value = "222" });
            //items.Add(new SensorValueItem { Name = "CCC", Value = "333" });
            //items.Add(new SettingItem { Name = "DDD", Value = "444" });

            return await Task.FromResult(items);
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
