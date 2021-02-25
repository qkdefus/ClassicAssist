﻿#region License

// Copyright (C) 2020 Reetus
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Data.Vendors;
using ClassicAssist.UI.ViewModels.Agents;
using ClassicAssist.UO;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network;
using ClassicAssist.UO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClassicAssist.Tests.Agents
{
    [TestClass]
    public class BuyAgentTests
    {
        [TestMethod]
        public void WillBuyCorrectAmount()
        {
            int vendorSerial = 0x0b0354;

            byte[] shopList =
            {
                0x74, 0x01, 0x67, 0x42, 0x0C, 0x82, 0x83, 0x1B, 0x00, 0x00, 0x00, 0x12, 0x08, 0x31, 0x30, 0x32,
                0x33, 0x38, 0x33, 0x34, 0x00, 0x00, 0x00, 0x00, 0x08, 0x08, 0x31, 0x30, 0x32, 0x34, 0x30, 0x33,
                0x31, 0x00, 0x00, 0x00, 0x00, 0x05, 0x08, 0x31, 0x30, 0x32, 0x33, 0x38, 0x32, 0x37, 0x00, 0x00,
                0x00, 0x00, 0x0B, 0x08, 0x31, 0x30, 0x34, 0x31, 0x30, 0x37, 0x32, 0x00, 0x00, 0x00, 0x00, 0x0F,
                0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x35, 0x36, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x08, 0x31, 0x30,
                0x32, 0x33, 0x38, 0x35, 0x31, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x08, 0x31, 0x30, 0x32, 0x33, 0x38,
                0x34, 0x38, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x08, 0x31, 0x30, 0x32, 0x33, 0x38, 0x34, 0x36, 0x00,
                0x00, 0x00, 0x00, 0x0F, 0x08, 0x31, 0x30, 0x32, 0x33, 0x38, 0x35, 0x32, 0x00, 0x00, 0x00, 0x00,
                0x0F, 0x08, 0x31, 0x30, 0x32, 0x33, 0x38, 0x34, 0x39, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x08, 0x31,
                0x30, 0x32, 0x33, 0x38, 0x34, 0x37, 0x00, 0x00, 0x00, 0x00, 0x05, 0x08, 0x31, 0x30, 0x32, 0x33,
                0x39, 0x36, 0x32, 0x00, 0x00, 0x00, 0x00, 0x05, 0x08, 0x31, 0x30, 0x32, 0x33, 0x39, 0x36, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x03, 0x08, 0x31, 0x30, 0x32, 0x33, 0x39, 0x37, 0x32, 0x00, 0x00, 0x00,
                0x00, 0x03, 0x08, 0x31, 0x30, 0x32, 0x33, 0x39, 0x37, 0x33, 0x00, 0x00, 0x00, 0x00, 0x03, 0x08,
                0x31, 0x30, 0x32, 0x33, 0x39, 0x37, 0x34, 0x00, 0x00, 0x00, 0x00, 0x03, 0x08, 0x31, 0x30, 0x32,
                0x33, 0x39, 0x37, 0x36, 0x00, 0x00, 0x00, 0x00, 0x03, 0x08, 0x31, 0x30, 0x32, 0x33, 0x39, 0x38,
                0x31, 0x00, 0x00, 0x00, 0x00, 0x03, 0x08, 0x31, 0x30, 0x32, 0x33, 0x39, 0x38, 0x30, 0x00, 0x00,
                0x00, 0x00, 0x0C, 0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x38, 0x32, 0x00, 0x00, 0x00, 0x00, 0x0C,
                0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x38, 0x33, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x31, 0x30,
                0x32, 0x37, 0x39, 0x38, 0x34, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x31, 0x30, 0x32, 0x37, 0x39,
                0x38, 0x35, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x38, 0x36, 0x00,
                0x00, 0x00, 0x00, 0x0C, 0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x38, 0x37, 0x00, 0x00, 0x00, 0x00,
                0x0C, 0x08, 0x31, 0x30, 0x32, 0x37, 0x39, 0x38, 0x31, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x31,
                0x30, 0x32, 0x37, 0x39, 0x38, 0x38, 0x00
            };

            byte[] contents =
            {
                0x3C, 0x02, 0x21, 0x00, 0x1B, 0x40, 0x1F, 0x00, 0x1B, 0x1F, 0x34, 0x00, 0x00, 0x14, 0x00, 0x1B,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x1A, 0x1F, 0x2D, 0x00,
                0x00, 0x14, 0x00, 0x1A, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00,
                0x19, 0x1F, 0x33, 0x00, 0x00, 0x14, 0x00, 0x19, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1F, 0x00, 0x17, 0x1F, 0x32, 0x00, 0x00, 0x14, 0x00, 0x18, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x14, 0x1F, 0x31, 0x00, 0x00, 0x14, 0x00, 0x17,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x13, 0x1F, 0x30, 0x00,
                0x00, 0x14, 0x00, 0x16, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00,
                0x12, 0x1F, 0x2F, 0x00, 0x00, 0x14, 0x00, 0x15, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1F, 0x00, 0x10, 0x1F, 0x2E, 0x00, 0x00, 0x14, 0x00, 0x14, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x0F, 0x0F, 0x8C, 0x00, 0x00, 0x28, 0x00, 0x13,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x0E, 0x0F, 0x8D, 0x00,
                0x00, 0x28, 0x00, 0x12, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00,
                0x0D, 0x0F, 0x88, 0x00, 0x00, 0x28, 0x00, 0x11, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1F, 0x00, 0x0C, 0x0F, 0x86, 0x00, 0x00, 0x14, 0x00, 0x10, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x0A, 0x0F, 0x85, 0x00, 0x00, 0x28, 0x00, 0x0F,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x09, 0x0F, 0x84, 0x00,
                0x00, 0x28, 0x00, 0x0E, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00,
                0x08, 0x0F, 0x7B, 0x00, 0x00, 0x14, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1F, 0x00, 0x07, 0x0F, 0x7A, 0x00, 0x00, 0x14, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x06, 0x0F, 0x07, 0x00, 0x00, 0x14, 0x00, 0x0B,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x05, 0x0F, 0x09, 0x00,
                0x00, 0x14, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00,
                0x04, 0x0F, 0x0C, 0x00, 0x00, 0x14, 0x00, 0x09, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1F, 0x00, 0x02, 0x0F, 0x06, 0x00, 0x00, 0x14, 0x00, 0x08, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x01, 0x0F, 0x08, 0x00, 0x00, 0x14, 0x00, 0x07,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1F, 0x00, 0x00, 0x0F, 0x0B, 0x00,
                0x00, 0x14, 0x00, 0x06, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1E, 0xFF,
                0xFF, 0x1F, 0x14, 0x00, 0x00, 0x10, 0x00, 0x05, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00,
                0x00, 0x40, 0x1E, 0xFF, 0xFE, 0x17, 0x18, 0x00, 0x00, 0x0A, 0x00, 0x04, 0x00, 0x01, 0x00, 0x42,
                0x0C, 0x82, 0x83, 0x00, 0xE1, 0x40, 0x1E, 0xFF, 0xFD, 0x0E, 0xF3, 0x00, 0x00, 0x14, 0x00, 0x03,
                0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1E, 0xFF, 0xFC, 0x0F, 0xBF, 0x00,
                0x00, 0x14, 0x00, 0x02, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00, 0x40, 0x1E, 0xFF,
                0xFB, 0x0E, 0xFA, 0x00, 0x00, 0x0A, 0x00, 0x01, 0x00, 0x01, 0x00, 0x42, 0x0C, 0x82, 0x83, 0x00, 0x00
            };

            byte[] containerDisplay = { 0x24, 0x00, 0x0B, 0x03, 0x54, 0x00, 0x30, 0x00, 0x00 };

            Mobile vendor = new Mobile( vendorSerial );
            vendor.SetLayer( Layer.ShopBuy, 0x420c8283 );

            Engine.Mobiles.Add( vendor );

            IncomingPacketHandlers.Initialize();

            VendorBuyTabViewModel vm = new VendorBuyTabViewModel();

            VendorBuyAgentEntry entry = new VendorBuyAgentEntry();

            entry.Items.Add( new VendorBuyAgentItem
            {
                Graphic = 0xf8c,
                Amount = 1,
                Enabled = true,
                Hue = -1,
                MaxPrice = -1
            } );
            entry.Items.Add( new VendorBuyAgentItem
            {
                Graphic = 0xf8d,
                Amount = 1,
                Enabled = true,
                Hue = -1,
                MaxPrice = -1
            } );
            entry.Items.Add( new VendorBuyAgentItem
            {
                Graphic = 0xf85,
                Amount = 1,
                Enabled = true,
                Hue = -1,
                MaxPrice = -1
            } );
            entry.Items.Add( new VendorBuyAgentItem
            {
                Graphic = 0xf88,
                Amount = 1,
                Enabled = true,
                Hue = -1,
                MaxPrice = -1
            } );

            entry.Enabled = true;
            vm.Items.Add( entry );

            PacketHandler containerContentsHandler = IncomingPacketHandlers.GetHandler( 0x3C );
            containerContentsHandler?.OnReceive( new PacketReader( contents, contents.Length, false ) );

            vendor.Equipment.Add( Engine.Items.GetItem( 0x420c8283 ) );

            PacketHandler shopListHandler = IncomingPacketHandlers.GetHandler( 0x74 );

            shopListHandler?.OnReceive( new PacketReader( shopList, shopList.Length, false ) );

            PacketHandler containerDisplayHandler = IncomingPacketHandlers.GetHandler( 0x24 );

            AutoResetEvent are = new AutoResetEvent( false );

            List<ShopListEntry> buyList = new List<ShopListEntry>();

            void OnPacketSentEvent( byte[] data, int length )
            {
                if ( data[0] != 0x3B )
                {
                    return;
                }

                PacketReader reader = new PacketReader( data, data.Length, false );

                int count = ( data.Length - 8 ) / 7;

                reader.Seek( 5, SeekOrigin.Current );

                for ( int i = 0; i < count; i++ )
                {
                    reader.ReadByte(); // layer
                    int serial = reader.ReadInt32();
                    int amount = reader.ReadInt16();

                    buyList.Add( new ShopListEntry { Amount = amount, Item = Engine.Items.GetItem( serial ) } );
                }
            }

            Engine.InternalPacketSentEvent += OnPacketSentEvent;

            containerDisplayHandler?.OnReceive( new PacketReader( containerDisplay, containerDisplay.Length, true ) );

            foreach ( VendorBuyAgentItem item in vm.Items.Where( e => e.Enabled ).SelectMany( e => e.Items ) )
            {
                int amount = buyList.Where( e => e.Item.ID == item.Graphic ).Sum( e => e.Amount );

                Assert.AreEqual( item.Amount, amount, 0, "Buy incorrect amount" );
            }

            Engine.Items.Clear();
            Engine.Mobiles.Clear();
        }

        [TestMethod]
        public void WillConvertOld()
        {
            VendorBuyTabViewModel vm = new VendorBuyTabViewModel();

            JObject json = JObject.Parse(
                "{\r\n    \"VendorBuy\": {\r\n        \"Enabled\": true,\r\n        \"IncludeBackpackAmount\": true,\r\n        \"IncludePurchasedAmount\": false,\r\n        \"Items\": [\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"citrine%s%\",\r\n                \"Graphic\": 3861,\r\n                \"Hue\": 0,\r\n                \"Amount\": 500,\r\n                \"MaxPrice\": 50\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"sapphire%s%\",\r\n                \"Graphic\": 3865,\r\n                \"Hue\": 0,\r\n                \"Amount\": 500,\r\n                \"MaxPrice\": 100\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"rub%ies/y%\",\r\n                \"Graphic\": 3859,\r\n                \"Hue\": 0,\r\n                \"Amount\": 500,\r\n                \"MaxPrice\": 75\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Black Pearl%s%\",\r\n                \"Graphic\": 3962,\r\n                \"Hue\": 0,\r\n                \"Amount\": 1000,\r\n                \"MaxPrice\": 5\r\n            },\r\n            {\r\n                \"Enabled\": true,\r\n                \"Name\": \"recall rune\",\r\n                \"Graphic\": 7956,\r\n                \"Hue\": 0,\r\n                \"Amount\": 17,\r\n                \"MaxPrice\": -1\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"blank scroll%s%\",\r\n                \"Graphic\": 3636,\r\n                \"Hue\": 0,\r\n                \"Amount\": 500,\r\n                \"MaxPrice\": 5\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Blood Moss\",\r\n                \"Graphic\": 3963,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": 5\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Mandrake Root%s%\",\r\n                \"Graphic\": 3974,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": 3\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Ginseng\",\r\n                \"Graphic\": 3973,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": -1\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Sulfurous Ash\",\r\n                \"Graphic\": 3980,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": -1\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Garlic\",\r\n                \"Graphic\": 3972,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": -1\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Nightshade\",\r\n                \"Graphic\": 3976,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": -1\r\n            },\r\n            {\r\n                \"Enabled\": false,\r\n                \"Name\": \"Spider's Silk\",\r\n                \"Graphic\": 3981,\r\n                \"Hue\": 0,\r\n                \"Amount\": -1,\r\n                \"MaxPrice\": -1\r\n            }\r\n        ]\r\n    }\r\n}" );

            vm.Deserialize( json, new Options() );

            Assert.AreEqual( 1, vm.Items.Count );

            VendorBuyAgentEntry entry = vm.Items[0];

            JToken config = json["VendorBuy"];

            Assert.AreEqual( entry.Enabled, config["Enabled"].ToObject<bool>() );
            Assert.AreEqual( entry.IncludeBackpackAmount, config["IncludeBackpackAmount"].ToObject<bool>() );

            VendorBuyAgentItem[] oldItems =
                JsonConvert.DeserializeObject<VendorBuyAgentItem[]>( config["Items"].ToString() );

            for ( int index = 0; index < oldItems.Length; index++ )
            {
                VendorBuyAgentItem oldItem = oldItems[index];
                VendorBuyAgentItem newItem = entry.Items[index];

                Assert.AreEqual( oldItem.Enabled, newItem.Enabled );
                Assert.AreEqual( oldItem.Name, newItem.Name );
                Assert.AreEqual( oldItem.Graphic, newItem.Graphic );
                Assert.AreEqual( oldItem.Amount, newItem.Amount );
                Assert.AreEqual( oldItem.Hue, newItem.Hue );
                Assert.AreEqual( oldItem.MaxPrice, newItem.MaxPrice );
            }
        }

        [TestMethod]
        public void VendorBuyCommandWillSet()
        {
            VendorBuyTabViewModel vm = new VendorBuyTabViewModel();

            VendorBuyAgentEntry entry = new VendorBuyAgentEntry { Name = "Test", Enabled = true };

            vm.Items.Add( entry );

            AgentCommands.SetVendorBuyAutoBuy( "Test", "off" );

            Assert.IsFalse( entry.Enabled );

            AgentCommands.SetVendorBuyAutoBuy( "Test", "on" );

            Assert.IsTrue( entry.Enabled );

            AgentCommands.SetVendorBuyAutoBuy( "Test" );

            Assert.IsFalse( entry.Enabled );
        }
    }
}