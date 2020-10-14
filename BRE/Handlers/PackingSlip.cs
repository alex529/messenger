using System;
using System.Collections.Generic;

namespace BRE.Handlers
{
    internal class PackingSlip
    {
        public PackingSlip()
        {
        }
        public List<string> Departments { get; set; } = new List<string>();
        public List<string> Items { get; set; } = new List<string>();
        public List<Guid> Products { get; set; } = new List<Guid>();

        public void Generate()
        {
            Console.WriteLine("Generating PackingSlip");
            if (Departments.Count > 0)
                Console.WriteLine("Departments:");
            foreach (var d in Departments)
            {
                Console.WriteLine($"\t{d}");
            }
            if (Items.Count > 0)
                Console.WriteLine("Items:");
            foreach (var i in Items)
            {
                Console.WriteLine($"\t{i}");
            }
            if (Products.Count > 0)
                Console.WriteLine("Products:");
            foreach (var p in Products)
            {
                Console.WriteLine($"\t{p}");
            }
        }
    }
}
