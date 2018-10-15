namespace Deposit.WebApi.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DimensionsDto Dimensions { get; set; }
        
        public class DimensionsDto
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int Depth { get; set; }
        }
    }
}