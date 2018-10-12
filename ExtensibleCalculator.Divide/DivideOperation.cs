using System.ComponentModel.Composition;

namespace ExtensibleCalculator.Divide
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '/')]
    internal class DivideOperation : IOperation
    {
        #region Implementation of IOperation

        /// <inheritdoc />
        public int Operate(int left, int right)
        {
            return left / right;
        }

        #endregion
    }
}