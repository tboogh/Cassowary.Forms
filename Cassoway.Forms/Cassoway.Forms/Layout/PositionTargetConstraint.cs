using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public class PositionTargetConstraint: IPositionTargetConstraint
    {
        public Position SourcePosition { get; }
        public View SourceView { get; }
        public Position TargetPosition { get; }
        public View TargetView { get; }
        public float Multiplier { get; }
        public float Constant { get; }

        internal PositionTargetConstraint(View sourceView, Position sourcePosition, View targetView, Position targetPosition, float multiplier = 1f, float constant = 0f)
        {
            SourceView = sourceView;
            SourcePosition = sourcePosition;
            TargetView = targetView;
            TargetPosition = targetPosition;
            Constant = constant;
            Multiplier = multiplier;
        }
		
        public static implicit operator Constraint(PositionTargetConstraint positionTargetConstraint)
        {
            return new Constraint()
            {
                Source = positionTargetConstraint.SourceView,
                Target = positionTargetConstraint.TargetView,
                SourceAttribute = positionTargetConstraint.SourcePosition.ToAttribute(),
                TargetAttribute = positionTargetConstraint.TargetPosition.ToAttribute(),
                Constant = positionTargetConstraint.Constant,
                Multiplier = positionTargetConstraint.Multiplier
            };
        } 
    }
}