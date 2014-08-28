using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<Item> Items;

        public static void Main()
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program
             {
                Items = new List<Item>
                {
                    new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                    new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                    new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                    new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                    new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
                    new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                }
            };

            app.UpdateQuality();
            System.Console.ReadKey();
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                var itemWrapper = new ItemWrapper(item);
                itemWrapper.ReduceSellin();
                itemWrapper.UpdateQuality();

                if (item.SellIn < 0)
                {
                    if (item.Name != "Aged Brie")
                    {
                        if (item.Name != "Backstage passes to a TAFKAL80ETC concert")
                        {
                            if (item.Quality > 0)
                            {
                                if (item.Name != "Sulfuras, Hand of Ragnaros")
                                {
                                    item.Quality = item.Quality - 1;
                                }
                            }
                        }
                        else
                        {
                            item.Quality = item.Quality - item.Quality;
                        }
                    }
                    else
                    {
                        if (item.Quality < 50)
                        {
                            item.Quality = item.Quality + 1;
                        }
                    }
                }
            }
        }
    }

    public interface IQuailityStrategy
    {
        void UpdateQuality(Item item);
    }

    public class PastSellinQualityDecreaseStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality += 1;
        }
    }

    public class IncreaseQuailityWithAgeStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality += 1;
        }
    }

    public class BackstageItemQuailityStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality < 50)
            {
                if (item.SellIn <= 0)
                    item.Quality = 0;
                else if (item.SellIn <= 5)
                    item.Quality += 3;
                else if (item.SellIn <= 10)
                    item.Quality += 2;
                else
                    item.Quality += 1;
            }
        }
    }

    public class ConjuredItemQuailityStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality > 0)
            {
                item.Quality -= 2;
            }
        }
    }

    public class NeverDecreasingQuailityStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            //Do Nothing
        }
    }

    public class StandardQuailityStrategy : IQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality > 0)
            {
                if (item.SellIn == 0)
                    item.Quality -= 2;
                else
                    item.Quality -= 1;
            }
        }
    }

    public class ItemWrapper
    {
        private readonly IList<IQuailityStrategy> _quailityStrategy = new List<IQuailityStrategy>();

        public Item Item { get; private set; }

        public ItemWrapper(Item item)
        {
            Item = item;

            if (item.Name == "Sulfuras, Hand of Ragnaros")
                _quailityStrategy.Add(new NeverDecreasingQuailityStrategy());
            else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                _quailityStrategy.Add(new BackstageItemQuailityStrategy());
            else if (item.Name == "Aged Brie")
                _quailityStrategy.Add(new IncreaseQuailityWithAgeStrategy());
            else if (item.Name.Contains("Conjured"))
                _quailityStrategy.Add(new ConjuredItemQuailityStrategy());
            else
                _quailityStrategy.Add(new StandardQuailityStrategy());
        }

        public void ReduceSellin()
        {
            if (Item.Name != "Sulfuras, Hand of Ragnaros")
                Item.SellIn -= 1;
        }

        public void UpdateQuality()
        {
            foreach (var quailityStrategy in _quailityStrategy)
                quailityStrategy.UpdateQuality(Item);
        }
    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
