using System.Text.Json.Serialization;

namespace PH.UberConnect.Api.Responses.Delivery
{
    public class ManifestItem
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public string size { get; set; }

        [JsonIgnore]
        public Dimensions dimensions { get; set; }
        [JsonIgnore]
        public bool must_be_upright { get; set; }
        [JsonIgnore]
        public int weight { get; set; }
        [JsonIgnore]
        public int price { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ManifestItem)obj;

            return name == other.name &&
                   quantity == other.quantity &&
                   size == other.size;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + (name != null ? name.GetHashCode() : 0);
            hash = hash * 23 + quantity.GetHashCode();
            hash = hash * 23 + (size != null ? size.GetHashCode() : 0);
            return hash;
        }
    }
}
