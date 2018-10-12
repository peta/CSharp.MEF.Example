using System;
using System.ComponentModel.Composition;
using MainApplication.Interop;

namespace MainApplication
{
    [Export(typeof(ICalculator))]
    class InternalCalculator : ICalculator
    {
        #region Implementation of ICalculator

        /// <inheritdoc />
        public string Calculate(string input)
        {
            return $"Result for: {input}";
        }

        #endregion
    }
}