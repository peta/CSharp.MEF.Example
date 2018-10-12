using System.ComponentModel.Composition;

namespace ExtensibleCalculator.Multiply
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '*')]
    internal class MultiplyOperation : IOperation
    {
        #region Implementation of IOperation

        /// <inheritdoc />
        public int Operate(int left, int right)
        {
            return left * right;
        }

        #endregion
    }
}