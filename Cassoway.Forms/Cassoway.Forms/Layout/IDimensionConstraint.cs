using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public interface IDimensionConstraint
    {
        Dimension Dimension { get; }
        View View { get; }
        float Constant { get; }
    }
    
    public interface IDimensionTargetConstraint
    {
        Dimension SourceDimension { get; }
        View SourceView { get; }
        Dimension TargetDimension { get; }
        View TargetView { get; }
        float Multiplier { get; }
        float Constant { get; }
    }
}