namespace PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties
{
    public class RoboCourierSpecification
    {
        public string mode { get; set; }

        public RoboCourierSpecification(string mode)
        {
            this.mode = mode;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            RoboCourierSpecification other = (RoboCourierSpecification)obj;

            return mode == other.mode;
        }

        public override int GetHashCode()
        {
            return mode.GetHashCode();
        }
    }
}
