using System;

namespace Deposit.Domain.Entities
{
    public class Dimensions
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        public Dimensions()
        { }

        public static Dimensions MakeDimensions(int width, int height, int depth)
        {
            var dimensions = new Dimensions();

            dimensions.Rescale(width, height, depth);

            return dimensions;
        }

        public void Rescale(int width, int height, int depth)
        {
            if (height <= 0)
            {
                throw new ArgumentException("Height must be larger than 0.");
            }

            if (width <= 0)
            {
                throw new ArgumentException("Width must be larger than 0.");
            }

            if (depth <= 0)
            {
                throw new ArgumentException("Depth must be larger than 0.");
            }

            Height = height;
            Width = width;
            Depth = depth;
        }

    }
}