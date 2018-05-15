using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cassoway.Forms.Layout
{   
    public class DimensionTargetConstraint : IDimensionTargetConstraint
    {
        public DimensionTargetConstraint(View sourceView, Dimension sourceDimension, View targetView, Dimension targetDimension, float multiplier, float constant)
        {
            SourceView = sourceView;
            TargetView = targetView;
            SourceDimension = sourceDimension;
            TargetDimension = targetDimension;
            Multiplier = multiplier;
            Constant = constant;
        }

        public Dimension SourceDimension { get; }
        public View SourceView { get; }
        public Dimension TargetDimension { get; }
        public View TargetView { get; }
        public float Multiplier { get; }
        public float Constant { get; }
        
        public static implicit operator Constraint(DimensionTargetConstraint positionTargetConstraint)
        {
            return new Constraint()
            {
                Source = positionTargetConstraint.SourceView,
                Target = positionTargetConstraint.TargetView,
                SourceAttribute = positionTargetConstraint.SourceDimension.ToAttribute(),
                TargetAttribute = positionTargetConstraint.TargetDimension.ToAttribute(),
                Constant = positionTargetConstraint.Constant,
                Multiplier = positionTargetConstraint.Multiplier
            };
        } 
    }
}