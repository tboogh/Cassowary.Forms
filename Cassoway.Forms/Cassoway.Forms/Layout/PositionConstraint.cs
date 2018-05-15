using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public class PositionConstraint: IPositionConstraint
    {
        protected PositionConstraint(View view, Position position, float constant = 0f)
        {
            View = view;
            Position = position;
            Constant = constant;
        }

        public static PositionConstraint Left(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.Left, constant);
        }
	    
        public static PositionConstraint Top(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.Top, constant);
        }
	    
        public static PositionConstraint Right(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.Right, constant);
        }
	    
        public static PositionConstraint Bottom(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.Bottom, constant);
        }
	    
        public static PositionConstraint CenterX(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.CenterX, constant);
        }
	    
        public static PositionConstraint CenterY(View view, float constant = 0f)
        {
            return new PositionConstraint(view, Position.CenterY, constant);
        }

        public PositionTargetConstraint ToLeft(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.Left, multiplier, Constant);
        }
	    
        public PositionTargetConstraint ToTop(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.Top, multiplier, Constant);
        }
	    
        public PositionTargetConstraint ToRight(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.Right, multiplier, Constant);
        }
	    
        public PositionTargetConstraint ToBottom(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.Bottom, multiplier, Constant);
        }
	    
        public PositionTargetConstraint ToCenterX(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.CenterX, multiplier, Constant);
        }
	    
        public PositionTargetConstraint ToCenterY(View view, float multiplier = 1f)
        {
            return new PositionTargetConstraint(View, Position, view, Position.CenterY, multiplier, Constant);
        }

        public Position Position { get; }
        public View View { get; }
        public float Constant { get; }
    }
}