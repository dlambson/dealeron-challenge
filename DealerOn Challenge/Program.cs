// See https://aka.ms/new-console-template for more information
using DealerOn_Challenge;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace DealerOnChallenge {

    public class Program
    {
        // this will contain terms that will determine if an item is tax exempt or not
        readonly static string[] exemptTerms = { "book", "chocolate", "bar", "chocolates", "pills" };

        static void Main(string[] args)
        {
            Console.WriteLine("Sales Taxes\n____________________________");
            Console.WriteLine("Please enter items you wish to purchase. Enter a ';' on the last item to finish entering items");

            var inputs = InputItems();

            List<Item> items = CreateItems(inputs);
            items = AssignTaxes(items);

            foreach(var item in items)
            {
                Console.WriteLine("Name: " + item.Name);
                Console.WriteLine("Quantity: " + item.Quantity);
                Console.WriteLine("Price: " + item.Price);
                Console.WriteLine("SalesTax: " + item.SalesTax);
                Console.WriteLine("IsImported: " + item.IsImported);
                Console.WriteLine("IsTaxExempt: " + item.IsTaxExempt);
                Console.WriteLine("ImportTax: " + item.ImportTax);
                Console.WriteLine("Total: " + item.Total);
                Console.WriteLine("\n\n");
            }
        }

        // create a string list of all the inputs
        public static List<string> InputItems()
        {
            var inputs = new List<string>();

            while (true)
            {
                string input = Console.ReadLine();

                if (input.EndsWith(";"))
                {
                    inputs.Add(input.TrimEnd(';'));
                    break;
                } else
                {
                    inputs.Add(input);
                }
            }
            return inputs ;
        }

        //parse the input into a list of objects
        public static List<Item> CreateItems(List<string> items)
        {
            List<Item> itemsObj = new List<Item>();

            foreach(string item in items)
            {
                Item obj = new Item();
                string pattern = @"(\d+)\s+(.*?)\s+at\s+(\d+\.\d{2})";
                Match match = Regex.Match(item, pattern);
                if (match.Success)
                {   
                    obj.Quantity = Convert.ToInt16(match.Groups[1].Value);
                    obj.Name = match.Groups[2].Value;
                    obj.Price = Convert.ToDecimal(match.Groups[3].Value);

                    if (obj.Name.Contains("Imported")) {
                        obj.IsImported = true;
                    }

                    itemsObj.Add(obj);

                } else
                {
                    continue;
                }
            }

            return itemsObj;
        }

        //assign taxes to the object list
        public static List<Item> AssignTaxes(List<Item> items)
        {
            Console.WriteLine("Hello? what is in the list: " + items.Count());
            int taxPercentage = 10;
            foreach(var item in items)
            {
                item.IsTaxExempt = IsTaxExempt(item.Name.ToLower());

                if (!item.IsTaxExempt)
                {
                    item.SalesTax = Math.Ceiling((item.Price / taxPercentage) * 20) / 20;
                }
            }

            return items;
        }

        public static bool IsTaxExempt(string name)
        {
            return exemptTerms.Any(t => name == t || name.Contains(t));
        }
    }
}