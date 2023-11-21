using Newtonsoft.Json;

namespace Cloud.Core.Models
{
    internal class InstallApplianceResponse
    {
        /// <summary>
        /// Gets detailed information about the appliance.
        /// </summary>
        [JsonProperty("appliance")]
        public net.openstack.Core.Domain.Appliance Appliance { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallApplianceResponse"/> class with the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance of the InstallApplianceResponse (see <see cref="Appliance"/>).</param>
        public InstallApplianceResponse(net.openstack.Core.Domain.Appliance appliance)
        {
            Appliance = appliance;
        }
    }
}
