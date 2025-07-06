using GraphQL_Demonstration.Model;
using System.Text.Json.Serialization;

namespace GraphQL_Demonstration.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public double UnitPrice { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
