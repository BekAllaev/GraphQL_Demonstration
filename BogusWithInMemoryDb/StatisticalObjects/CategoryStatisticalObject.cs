namespace BogusWithInMemoryDb.StatisticalObjects
{
    public class CategoryStatisticalObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductCount { get; set; }

        public double CategoryProductsOverallPrice { get; set; }
    }
}
