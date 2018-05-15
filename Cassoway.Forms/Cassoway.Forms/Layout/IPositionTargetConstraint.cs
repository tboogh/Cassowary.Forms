using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public interface IPositionTargetConstraint
    {
        Position SourcePosition { get; }
        View SourceView { get; }
        Position TargetPosition { get; }
        View TargetView { get; }
        float Multiplier { get; }
        float Constant { get; }
    }
}