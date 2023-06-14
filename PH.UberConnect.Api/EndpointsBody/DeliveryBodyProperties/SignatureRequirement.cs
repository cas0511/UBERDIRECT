namespace PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties
{
    public class SignatureRequirement
    {
        public bool enabled { get; set; }
        public bool collect_signer_name { get; set; }
        public bool collect_signer_relationship { get; set; }

        public SignatureRequirement(bool enabled, bool collect_signer_name, bool collect_signer_relationship)
        {
            this.enabled = enabled;
            this.collect_signer_name = collect_signer_name;
            this.collect_signer_relationship = collect_signer_relationship;
        }

        override
        public bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            SignatureRequirement other = (SignatureRequirement)obj;

            return enabled == other.enabled &&
                   collect_signer_name == other.collect_signer_name &&
                   collect_signer_relationship == other.collect_signer_relationship;
        }

        override
        public int GetHashCode()
        {
            return HashCode.Combine(enabled, collect_signer_name, collect_signer_relationship);
        }
    }
}
