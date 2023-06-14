namespace PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties
{
    public class VerificationRequirement
    {
        public SignatureRequirement? signature_requirement { get; set; }
        public bool picture { get; set; }

        public VerificationRequirement(SignatureRequirement signature_requirement, bool picture)
        {
            this.signature_requirement = signature_requirement;
            this.picture = picture;
        }

        public VerificationRequirement(bool picture)
        {
            this.picture = picture;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            VerificationRequirement other = (VerificationRequirement)obj;

            return signature_requirement.Equals(other.signature_requirement);
        }

        public override int GetHashCode()
        {
            return signature_requirement.GetHashCode();
        }
    }
}
