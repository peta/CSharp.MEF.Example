using System.ComponentModel.Composition.Primitives;

namespace ExtensibleCalculator
{
    public interface IOperation
    {
        int Operate(int left, int right);
    }
}