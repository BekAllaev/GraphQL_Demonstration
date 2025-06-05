namespace GraphQL_CQRS.StatisticalObjects
{
    public class CountryStatisticalObject
    {
        public string CountryName { get; set; }

        public int CustomersCount { get; set; }

        public int OrdersCount { get; set; }

        public double Sales { get; set; }
    }
}
