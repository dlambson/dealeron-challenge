// See https://aka.ms/new-console-template for more information
using DealerOn_Challenge;
using System;

namespace DealerOnChallenge {


    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sales Taxes\n____________________________");
            Console.WriteLine("Please enter items you wish to purchase. When finished, do Ctrl+Z and press 'Enter' to finish:");

            string[] inputs = InputItems();
            foreach (string line in inputs)
            {
                Console.WriteLine(line);
            }

        }


        public static string[] InputItems()
        {
            string input = Console.In.ReadToEnd();
            string[] inputs = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("You entered:");
            return inputs;
        }

        //parse the input into a list of objects
        public static List<Item> createItems(string[] items)
        {
            List<Item> itemsObj = new List<Item>();

            return itemsObj;
        }
    }
}




//Parse the strings in the array



