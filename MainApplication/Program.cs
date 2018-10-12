using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using MainApplication.Interop;

namespace MainApplication
{
    class Program
    {
        private CompositionContainer _container;

        [ImportMany(typeof(ICalculator))]
        public IEnumerable<Lazy<ICalculator>> _calculators;

        private Program()
        {
            // Build catalog aggregate which combines all specific catalog implementations we add to it
            // whose job is to discover extension parts and make them available to our main app
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
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
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        public void RunCalculator()
        {
            if (_calculators.Count() == 0)
            {
                Console.Error.WriteLine("No ICalculator implementation discovered. Terminating...");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // Let user choose calculator implementation
            Console.WriteLine("Choose one of the calculator implementations:");
            for (int i = 0;  i < _calculators.Count(); i++)
            {
                var calc = _calculators.ElementAt(i);
                Console.WriteLine($"  {i+1}) {calc.Value.GetType().FullName}");                
            }
            Console.WriteLine("Please enter ordinal number: ");
            if (int.TryParse(Console.ReadLine(), out int choosenIdx) == false || choosenIdx > _calculators.Count())
            {
                Console.Error.WriteLine("Invalid selection. Terminating...");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // Run calculation in a REPL
            var calculatorImpl = _calculators.ElementAt(choosenIdx - 1).Value;
            string userInput;
            Console.Write("Enter term: ");
            while (true)
            {
                userInput = Console.ReadLine();
                if (userInput == "exit") break;
                Console.WriteLine(calculatorImpl.Calculate(userInput));
            }
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.RunCalculator();
        }
    }
}
