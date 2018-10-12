using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MainApplication.Interop;

namespace ExtensibleCalculator
{
    [Export(typeof(ICalculator))]
    public class ExtensibleCalculator : ICalculator
    {
        private CompositionContainer _container;

        [ImportMany]
        private IEnumerable<Lazy<IOperation, IOperationData>> _operations;

        /// <inheritdoc />
        public ExtensibleCalculator()
        {
            // Build catalog aggregate which combines all specific catalog implementations we add to it
            // whose job is to discover extension parts and make them available to our main app
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ExtensibleCalculator).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons")));

            // Instantiate the actual MEF container
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException exc)
            {
                Console.Error.WriteLine(exc.ToString());
            }
        }

        #region Implementation of ICalculator

        /// <inheritdoc />
        public string Calculate(string input)
        {
            var matches = Regex.Match(input, @"\s*(\d+)\s*(\D)\s*(\d+)");
            if (matches.Groups.Count != 4)
            {
                return "Invalid user input. Please enter valid formula";
            }

            var left = int.Parse(matches.Groups[1].Value);
            var op = matches.Groups[2].Value[0];
            var right = int.Parse(matches.Groups[3].Value);

            foreach (Lazy<IOperation, IOperationData> operation in _operations)
            {
                if (operation.Metadata.Symbol.Equals(op))
                {
                    return operation.Value.Operate(left, right).ToString();
                }
            }

            return "Unknown operation. Supported operations are: " + String.Join(", ", _operations.Select(o => o.Metadata.Symbol));
        }

        #endregion
    }
}
