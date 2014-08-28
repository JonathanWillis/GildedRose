using System.Collections.Generic;
using GildedRose.Console;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class ProgramTests
    {

        [Test]
        public void ConjuredDegradesTwiceAsFast()
        {
            var item = new Item { Name = "Conjured", Quality = 20, SellIn = 10 };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(18));
        }

        [TestCase("+5 Dexterity Vest", 10, 9)]
        [TestCase("Aged Brie", 10, 9)]
        [TestCase("Elixir of the Mongoose", 10, 9)]
        [TestCase("Sulfuras, Hand of Ragnaros", 10, 10)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 9)]
        [TestCase("Conjured Mana Cake", 10, 9)]
        public void WhenUpdatingTheSellinOfItemsIsReduced(string itemName, int sellin, int sellinAfter)
        {
            var item = new Item { Name = itemName, Quality = 20, SellIn = sellin };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.SellIn, Is.EqualTo(sellinAfter));
        }

        [TestCase(15, 20, 21)]
        [TestCase(10,20,22)]
        [TestCase(5, 20, 23)]
        [TestCase(0, 10, 0)]
        public void QuailityOfBackstagePassIncreasesByTwoWhen10DaysOrLess(int sellIn, int quality, int expectedQuality)
        {
            var item = new Item
                {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = sellIn,
                Quality = quality
            };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(expectedQuality));
        }

        [Test]
        public void QuailityDegradesByOneWhenWithinTheSellinPeriod()
        {
            var item = new Item {Name = "+5 Dexterity Vest", Quality = 20, SellIn = 10};
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(19));
        }

        [Test]
        public void QuailityDegradesByTwoWhenPastTheSellinPeriod()
        {
            var item = new Item { Name = "+5 Dexterity Vest", Quality = 20, SellIn = 0 };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(18));
        }

        [Test]
        public void QuailityOfAnItemIsNeverNegative()
        {
            var item = new Item { Name = "+5 Dexterity Vest", Quality = 0, SellIn = 0 };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(0));
        }

        [Test]
        public void QuailityOfBrieIncreasesWithAge()
        {
            var item = new Item {Name = "Aged Brie", SellIn = 2, Quality = 0};
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(1));
        }


        [Test]
        public void QuailityCanNeverBeMoreThanFifty()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 50 };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(50));
        }

        [Test]
        public void QuailityOfSulfurasNeverDecreases()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 5, Quality = 80 };
            var subject = new Program
            {
                Items = new List<ItemWrapper> { new ItemWrapper(item) }
            };

            subject.UpdateQuality();
            Assert.That(item.Quality, Is.EqualTo(80));
            Assert.That(item.SellIn, Is.EqualTo(5));
        }
    }
}