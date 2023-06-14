namespace PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties
{
    public class TestSpecifications
    {
        public RoboCourierSpecification robo_courier_specification { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TestSpecifications other = (TestSpecifications)obj;

            return robo_courier_specification.Equals(other);
        }

        public override int GetHashCode()
        {
            return robo_courier_specification != null ? robo_courier_specification.GetHashCode() : 0;
        }
    }
}
