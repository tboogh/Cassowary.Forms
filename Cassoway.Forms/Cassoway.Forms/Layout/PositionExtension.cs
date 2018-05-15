using System;

namespace Cassoway.Forms.Layout
{
    public static class PositionExtension
    {
        public static Constraint.Attribute ToAttribute(this Position position)
        {
            switch (position)
            {
                case Position.Left:
                    return Constraint.Attribute.Left;
                case Position.Top:
                    return Constraint.Attribute.Top;
                case Position.Right:
                    return Constraint.Attribute.Right;
                case Position.Bottom:
                    return Constraint.Attribute.Bottom;
                case Position.CenterX:
                    return Constraint.Attribute.CenterX;
                case Position.CenterY:
                    return Constraint.Attribute.CenterY;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }
    }
}