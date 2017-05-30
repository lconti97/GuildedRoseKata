using GuildedRose;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GuildedRoseTests
{
    [TestClass]
    public class GuildedRoseShopTests
    {
        private GuildedRoseShop guildedRoseShop;
        
        public GuildedRoseShopTests()
        {
            guildedRoseShop = new GuildedRoseShop() { Items = new List<Item>() };
        }

        [TestMethod]
        public void NormalItemQualityDecreasesByOneAfterOneDay()
        {
            AddItemToShop("Plain Joe's Sword", 10, 15);

            guildedRoseShop.UpdateQuality();

            var updatedItem = guildedRoseShop.Items[0];
            Assert.AreEqual(9, updatedItem.Quality);
        }

        [TestMethod]
        public void NormalItemSellByDateDecreasesByOneAfterOneDay()
        {
            AddItemToShop("Plain Joe's Sword", 10, 15);

            guildedRoseShop.UpdateQuality();

            var updatedItem = guildedRoseShop.Items[0];
            Assert.AreEqual(14, updatedItem.SellIn);
        }

        [TestMethod]
        public void NormalItemPastSellInDateQualityDecreasesByTwoAfterOneDay()
        {
            AddItemToShop("Plain Joe's Dusty Sword", 10, 0);

            guildedRoseShop.UpdateQuality();

            var updatedItem = guildedRoseShop.Items[0];
            Assert.AreEqual(8, updatedItem.Quality);
        }

        [TestMethod]
        public void NormalItemWithZeroQualityRemainsAtZeroAfterOneDay()
        {
            AddItemToShop("Plain Joe's Cardboard Sword", 0, 15);

            guildedRoseShop.UpdateQuality();

            var updatedItem = guildedRoseShop.Items[0];
            Assert.AreEqual(0, updatedItem.Quality);
        }

        private void AddItemToShop(String name, Int32 quality, Int32 sellIn)
        {
            var normalItem = new Item { Name = name, Quality = quality, SellIn = sellIn };
            guildedRoseShop.Items.Add(normalItem);
        }
    }
}
