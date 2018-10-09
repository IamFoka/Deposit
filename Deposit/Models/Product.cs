using System;

namespace Deposit.Models
{
    public class Product : Entity
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Sku { get; protected set; }
        public float Price { get; protected set; }
        public float Amount { get; protected set; }
        public Dimensions Dimensions { get; protected set; }
        public DateTime RegisterDate { get; protected set; }

        protected Product() :
            base()
        { }

        public static Product MakeProduct(string name, string description, float price, Dimensions dimensions)
        {
            if (name == string.Empty)
                throw new ArgumentException("Name can't be empty.");

            if (description == string.Empty)
                throw new ArgumentException("Description can't be empty.");

            if (price <= 0)
                throw new ArgumentException("Price must be larger than 0.");

            if (dimensions == null)
                throw new ArgumentNullException("Dimensions must have a value.");

            var product = new Product();

            product.RegisterDate = DateTime.Now;
            product.Amount = 0;
            product.Dimensions = dimensions;
            product.Rename(name, description);
            product.UpdatePrice(price);
            product.GenerateSku();

            return product;
        }

        private void GenerateSku()
        {
            String sku = String.Empty;
            int count = 0;

            foreach (var c in Description)
            {
                if (count <= 2 && c != ' ')
                {
                    sku += c;
                    count++;
                }

                if (c == ' ')
                {
                    sku += '-';
                    count = 0;
                }
            }

            sku += '-' + Dimensions.Width.ToString() + '-' + Dimensions.Height.ToString() + '-' + Dimensions.Depth.ToString();
            Sku = sku.ToUpper();
        }

        public void UpdatePrice(float price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price must be larger than 0.");
            }

            Price = price;
        }

        public void UpdateAmount(float amount)
        {
            if (amount == 0)
                throw new ArgumentException("Amount can't be equal to 0.");

            if (Amount + amount < 0)
                throw new ArgumentException("Total amount can't be lower than 0.");

            Amount += amount;
        }

        public void Rename(string name, string description)
        {
            if (name == String.Empty)
                throw new ArgumentException("Name can't be empty.");

            if (description == String.Empty)
                throw new ArgumentException("Description can't be empty.");

            Name = name;
            Description = description;
            GenerateSku();
        }

        public void Redimension(int width, int height, int depth)
        {
            Dimensions.Rescale(width, height, depth);
            GenerateSku();
        }
    }
}