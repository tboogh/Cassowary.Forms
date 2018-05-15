using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public interface IPositionConstraint
    {
        Position Position { get; }
        View View { get; }
        float Constant { get; }
    }
}