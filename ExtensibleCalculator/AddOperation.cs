using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;

namespace ExtensibleCalculator
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '+')]
    internal class AddOperation : IOperation
    {
        #region Implementation of IOperation

        /// <inheritdoc />
        public int Operate(int left, int right)
        {
            return left + right;
        }

        #endregion
    }
}