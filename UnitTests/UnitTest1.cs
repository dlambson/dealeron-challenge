using DealerOn_Challenge;
using DealerOnChallenge;

namespace UnitTests
{
    public class Tests
    {
        [Test]
        public void CreateItems_Test()
        {
            // Arrange
            List<string> inputItems = new List<string>
        {
            "1 Book at 12.49",
            "1 Imported Music CD at 14.99",
            "Invalid Item" // This should be ignored
        };

            List<Item> expected = new List<Item>
        {
            new Item { Quantity = 1, Name = "Book", Price = 12.49m, IsImported = false },
            new Item { Quantity = 1, Name = "Imported Music CD", Price = 14.99m, IsImported = true }
        };

            // Act
            List<Item> result = Program.CreateItems(inputItems);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expected.Count));

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.That(result[i].Quantity, Is.EqualTo(expected[i].Quantity));
                Assert.That(result[i].Name, Is.EqualTo(expected[i].Name));
                Assert.That(result[i].Price, Is.EqualTo(expected[i].Price));
                Assert.That(result[i].IsImported, Is.EqualTo(expected[i].IsImported));
            }
        }

        [Test]
        // Tets when 1 item is imported and another where an item is not
        public void Test_AssignTaxes_1()
        {
            // Arrange
            List<Item> items = new List<Item>
        {
            new Item { Name = "Book", Price = 12.49m, IsImported = false },
            new Item { Name = "Imported Music CD", Price = 14.99m, IsImported = true }
        };

            // Act
            List<Item> result = Program.AssignTaxes(items);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            Assert.That(result[0].SalesTax, Is.EqualTo(0.00m)); // Book is tax exempt
            Assert.That(result[0].ImportTax, Is.EqualTo(0.00m)); // Not imported
            Assert.That(result[0].Total, Is.EqualTo(12.49m));

            Assert.That(result[1].SalesTax, Is.EqualTo(1.50m)); // Imported Music CD is not tax exempt
            Assert.That(result[1].ImportTax, Is.EqualTo(0.75m)); // Imported
            Assert.That(result[1].Total, Is.EqualTo(17.24m));
        }

        [Test]
        // Tests when an item is tax exempt
        public void Test_AssignTaxes_2()
        {
            // Arrange
            List<Item> items = new List<Item>
        {
            new Item { Name = "Book", Price = 12.49m, IsImported = false }
        };

            // Act
            List<Item> result = Program.AssignTaxes(items);

            // Assert
            Assert.That(result[0].SalesTax, Is.EqualTo(0.00m)); // Book is tax exempt
        }

        [Test]
        public void Test_OutputReceipt_1()
        {
            // Arrange
            List<Item> items = new List<Item>
        {
            new Item { Name = "Book", Price = 12.49m, IsImported = false, Quantity = 1, Total = 12.49m, SalesTax = 0.00m, ImportTax = 0.00m },
            new Item { Name = "Imported Music CD", Price = 14.99m, IsImported = true, Quantity = 1, Total = 17.24m, SalesTax = 1.50m, ImportTax = 0.75m }
        };

            string expectedOutput = "------------Output--------------\n\r\nBook: 12.49\nImported Music CD: 17.24\nSales Taxes: 2.25\nTotal: 29.73"; // Provide the expected output here.

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.OutputReceipt(items);

                string actualOutput = sw.ToString().Trim();

                // Assert
                Assert.That(actualOutput, Is.EqualTo(expectedOutput));
            }
        }

        [Test]
        public void Test_OutputReceipt_2()
        {
            // Arrange
            List<Item> items = new List<Item>
        {
            new Item { Name = "Book", Price = 12.49m, IsImported = false, Quantity = 1, Total = 12.49m, SalesTax = 0.00m, ImportTax = 0.00m },
            new Item { Name = "Book", Price = 12.49m, IsImported = false, Quantity = 1, Total = 12.49m, SalesTax = 0.00m, ImportTax = 0.00m },
            new Item { Name = "Imported Music CD", Price = 14.99m, IsImported = true, Quantity = 1, Total = 17.24m, SalesTax = 1.50m, ImportTax = 0.75m }
        };

            string expectedOutput = "------------Output--------------\n\r\nBook: 24.98 (2 @ 12.49)\nImported Music CD: 17.24\nSales Taxes: 2.25\nTotal: 42.22"; // Provide the expected output here.

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.OutputReceipt(items);

                string actualOutput = sw.ToString().Trim();

                // Assert
                Assert.That(actualOutput, Is.EqualTo(expectedOutput));
            }
        }
    }
}