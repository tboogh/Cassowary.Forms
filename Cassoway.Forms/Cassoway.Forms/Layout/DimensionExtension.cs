using System;

namespace Cassoway.Forms.Layout
{
    public static class DimensionExtension
    {
        public static Constraint.Attribute ToAttribute(this Dimension dimension)
        {
            switch (dimension)
            {
                case Dimension.Width:
                    return Constraint.Attribute.Width;
                case Dimension.Height:
                    return Constraint.Attribute.Height;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
            }
        }
    }
}