using GuildedRose;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuildedRoseTests
{
    [TestClass]
    public class GuildedRoseShopTests
    {
        private const String Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const String AgedBrie = "Aged Brie";
        private const String BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";

        private GuildedRoseShop guildedRoseShop;
        
        public GuildedRoseShopTests()
        {
            guildedRoseShop = new GuildedRoseShop() { Items = new List<Item>() };
        }

        [TestMethod]
        public void NormalItemQualityDecreasesByOnePerDay()
        {
            var updatedItem = CreateItemAndUpdate("Plain Joe's Sword", 10, 15);

            Assert.AreEqual(9, updatedItem.Quality);
        }

        [TestMethod]
        public void NormalItemSellByDateDecreasesByOnePerDay()
        {
            var updatedItem = CreateItemAndUpdate("Plain Joe's Sword", 10, 15);

            Assert.AreEqual(14, updatedItem.SellIn);
        }

        [TestMethod]
        public void NormalItemPastSellInDateQualityDecreasesByTwoPerDay()
        {
            var updatedItem = CreateItemAndUpdate("Plain Joe's Dusty Sword", 10, 0);

            Assert.AreEqual(8, updatedItem.Quality);
        }

        [TestMethod]
        public void NormalItemWithZeroQualityRemainsAtZeroPerDay()
        {
            var updatedItem = CreateItemAndUpdate("Plain Joe's Cardboard Sword", 0, 15);

            Assert.AreEqual(0, updatedItem.Quality);
        }

        [TestMethod]
        public void AgedBrieBeforeSellInDateQualityIncreasesByOnePerDay()
        {
            var updatedItem = CreateItemAndUpdate(AgedBrie, 10, 15);

            Assert.AreEqual(11, updatedItem.Quality);
        }
         
        [TestMethod]
        public void AgedBrieAfterSellInDateQualityIncreasesByTwoPerDay()
        {
            var updatedItem = CreateItemAndUpdate(AgedBrie, 10, 0);

            Assert.AreEqual(12, updatedItem.Quality);
        }

        [TestMethod]
        public void AgedBrieBeforeSellInDateWithQualityFiftyDoesNotIncreaseInQuality()
        {
            var updatedItem = CreateItemAndUpdate(AgedBrie, 50, 10);

            Assert.AreEqual(50, updatedItem.Quality);
        }

        [TestMethod]
        public void AgedBrieAfterSellInDateQualityDoesNotIncreaseAboveFifty()
        {
            var updatedItem = CreateItemAndUpdate(AgedBrie, 50, -2);

            Assert.AreEqual(50, updatedItem.Quality);
        }

        [TestMethod]
        public void SulfurasBeforeSellInDateQualityDoesNotChange()
        {
            var updatedItem = CreateItemAndUpdate(Sulfuras, 80, 10);

            Assert.AreEqual(80, updatedItem.Quality);
        }

        [TestMethod]
        public void SulfurasAfterSellInDateQualityDoesNotChange()
        {
            var updatedItem = CreateItemAndUpdate(Sulfuras, 80, -10);

            Assert.AreEqual(80, updatedItem.Quality);
        }

        [TestMethod]
        public void BackstagePassesMoreThanTenDaysBeforeSellInDateQualityIncreasesByOnePerDay()
        {
            var updatedItem = CreateItemAndUpdate(BackstagePasses, 20, 15);

            Assert.AreEqual(21, updatedItem.Quality);
        }

        [TestMethod]
        public void BackstagePassesBetweenTenAndSixDaysInclusivelyBeforeSellInDateQualityIncreasesByTwoPerDay()
        {
            AddItemToShop(BackstagePasses, 20, 10);

            AssertItemQualityIncreasesByGivenIntervalInInclusiveRange(2, 10, 6);
        }

        [TestMethod]
        public void BackstagePassesBetweenFiveAndOneDaysInclusivelyBeforeSellInDateQualityIncreasesByThreePerDay()
        {
            AddItemToShop(BackstagePasses, 20, 5);

            AssertItemQualityIncreasesByGivenIntervalInInclusiveRange(3, 5, 1);
        }

        [TestMethod]
        public void BackstagePassesAfterSellInDateQualityIsZero()
        {
            var updatedItem = CreateItemAndUpdate(BackstagePasses, 20, 0);

            Assert.AreEqual(0, updatedItem.Quality);
        }

        [TestMethod]
        public void TwoNormalItemsBeforeSellInDateQualityDecreasesByOnePerDay()
        {
            var itemName1 = "Joe's sword of noodliness";
            var itemName2 = "Mario's spicy meatball";

            AddItemToShop(itemName1, 4, 10);
            AddItemToShop(itemName2, 8, 11);

            guildedRoseShop.UpdateQuality();

            var updatedItem1 = GetItemFromShop(itemName1);
            var updatedItem2 = GetItemFromShop(itemName2);

            Assert.AreEqual(3, updatedItem1.Quality);
            Assert.AreEqual(7, updatedItem2.Quality);
        }

        private Item GetItemFromShop(String name)
        {
            return guildedRoseShop.Items.First(i => i.Name == name);
        }

        private void AssertItemQualityIncreasesByGivenIntervalInInclusiveRange(Int32 qualityInterval, Int32 startSellInDate, Int32 endSellInDate)
        {
            var expected = guildedRoseShop.Items[0].Quality;
            var daysToRun = startSellInDate - endSellInDate + 1;

            for (var i = 0; i < daysToRun; i++)
            {
                guildedRoseShop.UpdateQuality();
                expected += qualityInterval;
                var actual = guildedRoseShop.Items[0].Quality;
                Assert.AreEqual(expected, actual);
            }
        }

        private Item CreateItemAndUpdate(String name, Int32 quality, Int32 sellIn)
        {
            AddItemToShop(name, quality, sellIn);

            guildedRoseShop.UpdateQuality();

            return GetItemFromShop(name);
        }

        private void AddItemToShop(String name, Int32 quality, Int32 sellIn)
        {
            var normalItem = new Item { Name = name, Quality = quality, SellIn = sellIn };
            guildedRoseShop.Items.Add(normalItem);
        }
    }
}
