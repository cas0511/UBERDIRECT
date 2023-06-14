using PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties;
using PH.UberConnect.Api.Responses.Delivery;
using System.Text.Json;

namespace PH.UberConnect.Api.EndpointsBody
{
    public class DeliveryBody : IEndpointBody
    {
        public string? dropoff_address { get; set; }
        public string? pickup_address { get; set; }
        public double dropoff_latitude { get; set; }
        public double dropoff_longitude { get; set; }
        public string? dropoff_phone_number { get; set; }
        public double pickup_latitude { get; set; }
        public double pickup_longitude { get; set; }
        public string? pickup_phone_number { get; set; }
        public string? pickup_name { get; set; }
        public string? dropoff_name { get; set; }
        public List<ManifestItem>? manifest_items { get; set; }
        public string? external_store_id { get; set; }
        public string? manifest_reference { get; set; }
        public string? dropoff_notes { get; set; }
        public VerificationRequirement pickup_verification { get; set; }
        public VerificationRequirement dropoff_verification { get; set; }
        public TestSpecifications? test_specifications { get; set; }
        public int manifest_total_value { get; set; }
        public string? pickup_ready_dt { get; set; }
        public string? pickup_notes { get; set; }

        public DeliveryBody() { }

        public DeliveryBody(string? dropoff_address,
                             string? pickup_address,
                             double dropoff_latitude,
                             double dropoff_longitude,
                             string? dropoff_phone_number,
                             double pickup_latitude,
                             double pickup_longitude,
                             string? pickup_phone_number,
                             string? pickup_name,
                             string? dropoff_name,
                             List<ManifestItem>? manifest_items,
                             string? external_store_id)
        {
            this.dropoff_address = dropoff_address;
            this.pickup_address = pickup_address;
            this.dropoff_latitude = dropoff_latitude;
            this.dropoff_longitude = dropoff_longitude;
            this.dropoff_phone_number = dropoff_phone_number;
            this.pickup_latitude = pickup_latitude;
            this.pickup_longitude = pickup_longitude;
            this.pickup_phone_number = pickup_phone_number;
            this.pickup_name = pickup_name;
            this.dropoff_name = dropoff_name;
            this.manifest_items = manifest_items;
            this.external_store_id = external_store_id;
        }

        override
        public string ToString() => ToJson();

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        override
        public bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            DeliveryBody other = (DeliveryBody)obj;

            return dropoff_address == other.dropoff_address &&
                   pickup_address == other.pickup_address &&
                   dropoff_latitude == other.dropoff_latitude &&
                   dropoff_longitude == other.dropoff_longitude &&
                   dropoff_phone_number == other.dropoff_phone_number &&
                   pickup_latitude == other.pickup_latitude &&
                   pickup_longitude == other.pickup_longitude &&
                   pickup_phone_number == other.pickup_phone_number &&
                   pickup_name == other.pickup_name &&
                   dropoff_name == other.dropoff_name &&
                   AreManifestItemsEqual(manifest_items, other.manifest_items) &&
                   external_store_id == other.external_store_id &&
                   manifest_reference == other.manifest_reference &&
                   dropoff_notes == other.dropoff_notes &&
                   pickup_verification.Equals(other.pickup_verification) &&
                   dropoff_verification.Equals(other.dropoff_verification) &&
                   test_specifications == other.test_specifications &&
                   manifest_total_value == other.manifest_total_value &&
                   pickup_ready_dt == other.pickup_ready_dt;
        }

        /// <summary>
        /// Determines whether two ManifestItems are equal by comparing their name, quantity, and size.
        /// </summary>
        /// <param name="item1">The first ManifestItem to compare.</param>
        /// <param name="item2">The second ManifestItem to compare.</param>
        /// <returns>true if the two ManifestItems are equal; otherwise, false.</returns>
        private bool AreManifestItemsEqual(List<ManifestItem>? items1, List<ManifestItem>? items2)
        {
            if (items1 == null && items2 == null)
                return true;

            if (items1 == null || items2 == null || items1.Count != items2.Count)
                return false;

            for (int i = 0; i < items1.Count; i++)
            {
                if (!items1[i].Equals(items2[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (dropoff_address?.GetHashCode() ?? 0);
                hash = hash * 23 + (pickup_address?.GetHashCode() ?? 0);
                hash = hash * 23 + dropoff_latitude.GetHashCode();
                hash = hash * 23 + dropoff_longitude.GetHashCode();
                hash = hash * 23 + (dropoff_phone_number?.GetHashCode() ?? 0);
                hash = hash * 23 + pickup_latitude.GetHashCode();
                hash = hash * 23 + pickup_longitude.GetHashCode();
                hash = hash * 23 + (pickup_phone_number?.GetHashCode() ?? 0);
                hash = hash * 23 + (pickup_name?.GetHashCode() ?? 0);
                hash = hash * 23 + (dropoff_name?.GetHashCode() ?? 0);
                hash = hash * 23 + (manifest_items?.GetHashCode() ?? 0);
                hash = hash * 23 + (external_store_id?.GetHashCode() ?? 0);
                hash = hash * 23 + (manifest_reference?.GetHashCode() ?? 0);
                hash = hash * 23 + (dropoff_notes?.GetHashCode() ?? 0);
                hash = hash * 23 + pickup_verification.GetHashCode();
                hash = hash * 23 + dropoff_verification.GetHashCode();
                hash = hash * 23 + (test_specifications?.GetHashCode() ?? 0);
                hash = hash * 23 + manifest_total_value.GetHashCode();
                hash = hash * 23 + (pickup_ready_dt?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
