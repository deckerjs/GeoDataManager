using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GeoStoreAPI.Services
{
    public class APIHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var healthCheckResult = GetCurrentHealth();

            if (healthCheckResult)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Healthy"));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("Health Problems"));
        }

        private bool GetCurrentHealth(){
            return true;
        }
    }    
}