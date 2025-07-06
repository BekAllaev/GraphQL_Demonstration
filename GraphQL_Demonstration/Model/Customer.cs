namespace GraphQL_Demonstration.Model
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
