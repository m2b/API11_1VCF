using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ITVizion.VizionDI.Definitions;

namespace APIVCF
{
    public class SimpleObject:Recordable<long>
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public class Reflector
    {
        public Dictionary<Type, string> Names { get; set; }
    }

    class ReflectionSandbox
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Serializing simple config to file");
            Reflector config = new Reflector();
            config.Names = new System.Collections.Generic.Dictionary<Type, string>();
            config.Names.Add(typeof(SimpleObject), "Simple");
            config.Names.Add(typeof(Recordable<long>), "BaseSimple");

			// Save json config to file
			string json = JsonConvert.SerializeObject(config, Formatting.Indented);

			//write string to file
			File.WriteAllText("simple.json", json);


			Console.WriteLine("Press any key to terminate");
            Console.ReadKey();
        }
    }
}
