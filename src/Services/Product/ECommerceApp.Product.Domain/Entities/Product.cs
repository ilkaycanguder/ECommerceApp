using ECommerceApp.Product.Domain.Enums;
using ECommerceApp.Shared.Abstractions;

namespace ECommerceApp.Product.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public ProductStatus Status { get; private set; } = ProductStatus.Active;

        private Product() { }

        public static Product Create(
            string name,
            string description,
            decimal price,
            int stock,
            string category)
        {
            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                Category = category,
                Status = ProductStatus.Active
            };
        }

        public void Update(
            string name,
            string description,
            decimal price,
            int stock,
            string category)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Category = category;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            Status = ProductStatus.Inactive;
            SetUpdatedAt();
        }

        public void Activate()
        {
            Status = ProductStatus.Active;
            SetUpdatedAt();
        }

        public void SetOutOfStock()
        {
            Status = ProductStatus.OutOfStock;
            SetUpdatedAt();
        }
    }
}
