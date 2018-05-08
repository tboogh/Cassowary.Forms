
namespace Cassowary
{
    public interface IEditContext
    {
        ClSimplexSolver EndEdit();

        IEditContext SuggestValue(ClVariable clVariable, double value);

        IEditContext Resolve();
    }
}
