using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOn_Challenge
{
    public class Item
    {
        // Name of the item
        public string? Name { get; set; }

        // Price with a 2 digit decimal
        public decimal Price { get; set; }

        // SalesTax with a 2 digit decimal
        public decimal SalesTax { get; set; }

        // IsImported, boolean
        public bool IsImported { get; set; }

        // ImportTax with a 2 digit decimal
        public decimal ImportTax { get; set; }

        // Total with a 2 digit decimal
        public decimal Total { get; set; }
    }
}
