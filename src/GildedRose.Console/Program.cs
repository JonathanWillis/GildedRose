using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<ItemWrapper> Items;
        static void Main()
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program
                {
                              Items = new List<ItemWrapper>
                                          {
                                              new ItemWrapper(new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20}),
                                              new ItemWrapper(new Item {Name = "Aged Brie", SellIn = 2, Quality = 0}),
                                              new ItemWrapper(new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7}),
                                              new ItemWrapper(new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80}),
                                              new ItemWrapper(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 }),
                                              new ItemWrapper(new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6})
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            foreach (var itemWrapper in Items)
            {
                itemWrapper.ReduceSellin();
                itemWrapper.UpdateQuality();

                var item = itemWrapper.Item;
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

    public interface IReduceQuailityStrategy
    {
        void UpdateQuality(Item item);
    }

    public class IncreaseQuailityWithAgeStrategy : IReduceQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality += 1;
        }
    }

    public class BackstageReduceQuailityStrategy : IReduceQuailityStrategy
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

    class ConjuredReduceQuailityStrategy : IReduceQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality > 0)
            {
                item.Quality -= 2;
            }
        }
    }

    public class NeverDecreasingQuailityStrategy : IReduceQuailityStrategy
    {
        public void UpdateQuality(Item item)
        {
            //Do Nothing
        }
    }

    public class StandardReduceQuailityStrategy : IReduceQuailityStrategy
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
        private readonly IReduceQuailityStrategy _reduceQuailityStrategy;

        public ItemWrapper(Item item)
        {
            Item = item;

            if (item.Name == "Sulfuras, Hand of Ragnaros")
                _reduceQuailityStrategy = new NeverDecreasingQuailityStrategy();
            else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                _reduceQuailityStrategy = new BackstageReduceQuailityStrategy();
            else if (item.Name == "Aged Brie")
                _reduceQuailityStrategy = new IncreaseQuailityWithAgeStrategy();
            else if (item.Name.Contains("Conjured"))
                _reduceQuailityStrategy = new ConjuredReduceQuailityStrategy();
            else
                _reduceQuailityStrategy = new StandardReduceQuailityStrategy();
        }

        public void ReduceSellin()
        {
            if (Item.Name != "Sulfuras, Hand of Ragnaros")
                Item.SellIn -= 1;
        }

        public Item Item { get; private set; }

        public void UpdateQuality()
        {
            _reduceQuailityStrategy.UpdateQuality(Item);
        }
    }


    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
