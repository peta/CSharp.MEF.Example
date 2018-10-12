using System.ComponentModel.Composition;

namespace ExtensibleCalculator
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    internal class SubtractOperation : IOperation
    {
        #region Implementation of IOperation

        /// <inheritdoc />
        public int Operate(int left, int right)
        {
            return left - right;
        }

        #endregion
    }
}