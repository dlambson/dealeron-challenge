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
        readonly static string[] exemptTerms = { "book", "chocolate", "chocolates", "pills" };

        static void Main(string[] args)
        {
            Console.WriteLine("Sales Taxes\n____________________________");
            Console.WriteLine("Please enter items you wish to purchase. Enter a ';' on the last item to finish entering items");

            var inputs = InputItems();

            List<Item> items = CreateItems(inputs);
            items = AssignTaxes(items);

            OutputReceipt(items);
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
            int taxPercentage = 10;
            decimal importTax = 0.05m;
            foreach(var item in items)
            {
                item.IsTaxExempt = IsTaxExempt(item.Name.ToLower());

                if (!item.IsTaxExempt)
                {
                    item.SalesTax = Math.Ceiling((item.Price / taxPercentage) * 20) / 20;
                }

                if (item.IsImported)
                {
                    item.ImportTax = Math.Ceiling((item.Price * importTax) * 20) / 20;
                }

                item.Total = item.Price + item.SalesTax + item.ImportTax;
            }

            return items;
        }

        //returns if an item is exempt based on terms found in the input
        public static bool IsTaxExempt(string name)
        {
            return exemptTerms.Any(t => name == t || name.Contains(t));
        }

        //return list of unique items (removes duplicate items if any)
        public static List<Item> GetUniqueItems(List<Item> items)
        {
            return items.GroupBy(item => new { item.Name, item.Price }).Select(group => group.First()).ToList();
        }

        //takes the list of item objects and totals up the receipts
        public static void OutputReceipt(List<Item> items)
        {
            Console.WriteLine("\n------------Output--------------\n");
            decimal total = 0.00m;
            decimal totalSalesTax = 0.00m;
            string output = "";
            var uniqueItems = GetUniqueItems(items);
            
            foreach(var item in uniqueItems)
            {
                //check for duplicate items finding by name and price
                int duplicates = items.Count(i => i.Name == item.Name && i.Price == item.Price);
                output += $"{item.Name}: ";
                //if there are more than one of the same item in the list, then multiply the duplicate totals and add them to the total
                if (duplicates > 1)
                {
                    decimal itemMultTotal = item.Total * duplicates;
                    output += $"{itemMultTotal} ({duplicates} @ {item.Total})\n";
                    totalSalesTax += duplicates * (item.SalesTax + item.ImportTax);
                    total += duplicates * (item.Total);
                } else
                {
                    output += $"{item.Total}\n";
                    totalSalesTax += item.SalesTax + item.ImportTax;
                    total += item.Total;
                }
            }

            output += $"Sales Taxes: {totalSalesTax}\n";
            output += $"Total: {total}";

            Console.WriteLine(output);
        }
    }
}