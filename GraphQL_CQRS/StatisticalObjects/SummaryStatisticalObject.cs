namespace GraphQL_CQRS.StatisticalObjects
{
    public class SummaryStatisticalObject
    {
        public int AmountOfOrders { get; set; }

        public double OverallSales { get; set; }

        public double MaxCheck { get; set; }

        public double MinCheck { get; set; }

        public double AverageCheck { get; set; }
    }
}
