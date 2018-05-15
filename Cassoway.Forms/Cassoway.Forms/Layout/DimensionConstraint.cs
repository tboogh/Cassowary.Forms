using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public class DimensionConstraint: IDimensionConstraint
    {
        protected DimensionConstraint(View view, Dimension dimension, float constant = 0f)
        {
            View = view;
            Dimension = dimension;
            Constant = constant;
        }

        public static DimensionConstraint Width(View view, float constant = 0f)
        {
            return new DimensionConstraint(view, Dimension.Width, constant);
        }
	    
        public static DimensionConstraint Height(View view, float constant = 0f)
        {
            return new DimensionConstraint(view, Dimension.Height, constant);
        }

        public DimensionTargetConstraint ToWidth(View view, float multiplier = 1f)
        {
            return new DimensionTargetConstraint(View, Dimension, view, Dimension.Width, multiplier, Constant);
        }
	    
        public DimensionTargetConstraint ToHeight(View view, float multiplier = 1f)
        {
            return new DimensionTargetConstraint(View, Dimension, view, Dimension.Height, multiplier, Constant);
        }

        public Dimension Dimension { get; }
        public View View { get; }
        public float Constant { get; }
        
        public static implicit operator Constraint(DimensionConstraint dimensionConstraint)
        {
            return new Constraint()
            {
                Source = dimensionConstraint.View,
                SourceAttribute = dimensionConstraint.Dimension.ToAttribute(),
                Constant = dimensionConstraint.Constant,
            };
        } 
    }
}