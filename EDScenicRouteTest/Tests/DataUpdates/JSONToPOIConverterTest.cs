﻿using EDScenicRouteCore.DataUpdates;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDScenicRouteCoreTests.DataUpdates
{
    [TestFixture]
    public class JSONToPOIConverterTest
    {

        [Test]
        public void TestConvert()
        {
            const string testJson = @"[{""id"":""1"",""type"":""planetaryNebula"",""name"":""Athaip Wisteria Nebula"",""galMapSearch"":""Athaip DW-N e6 - 3063"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 645310\/ name\/ Athaip + DW - N + e6 - 3063"",""coordinates"":[511.84375,-1020.875,23171.3125]},{""id"":""2"",""type"":""planetaryNebula"",""name"":""Binary Nebula"",""galMapSearch"":""Braireau CW-V e2 - 774"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 1271751\/ name\/ Braireau + CW - V + e2 - 774"",""coordinates"":[-2183.96875,-212.21875,31553.25]},{""id"":""3"",""type"":""nebula"",""name"":""Bloody Haze Nebula"",""galMapSearch"":""Dryio Bloo PZ - W d2 - 1161"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 2036671\/ name\/ Dryio + Bloo + PZ - W + d2 - 1161"",""coordinates"":[-6375.84375,-1600.65625,28627.28125]},{""id"":""4"",""type"":""planetaryNebula"",""name"":""Blue Lilies Nebula"",""galMapSearch"":""Dumbaa XJ-R e4 - 5596"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 2183178\/ name\/ Dumbaa + XJ - R + e4 - 5596"",""coordinates"":[-4252.625,196.09375,22780.84375]},{""id"":""5"",""type"":""nebula"",""name"":""Celestial Rainbow Nebula"",""galMapSearch"":""Eok Gree PI - S e4 - 4843"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 984582\/ name\/ Eok + Gree + PI - S + e4 - 4843"",""coordinates"":[-1376.96875,-331.96875,30511.03125]},{""id"":""6"",""type"":""planetaryNebula"",""name"":""Dimidium Iter Nebula and the 'Black Fields"",""galMapSearch"":""Eorl Bru AL-P e5-1475"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2183231\/name\/Eorl+Bru+AL-P+e5-1475"",""coordinates"":[1569.9375,1387.21875,30707.4375]},{""id"":""7"",""type"":""planetaryNebula"",""name"":""Eok Bluae Stellar Remnant"",""galMapSearch"":""Eok Bluae OM-W e1-7494"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2183482\/name\/Eok+Bluae+OM-W+e1-7494"",""coordinates"":[932.25,-1179.96875,27440.5625]},{""id"":""8"",""type"":""blackHole"",""name"":""Great Annihilator Black Hole"",""galMapSearch"":""Great Annihilator"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/65259\/name\/Great+Annihilator"",""coordinates"":[354.84375,-42.4375,22997.21875]},{""id"":""9"",""type"":""planetaryNebula"",""name"":""Green Crystal Stellar Remnant"",""galMapSearch"":""Eok Gree TO-Q e5-3167"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/645595\/name\/Eok+Gree+TO-Q+e5-3167"",""coordinates"":[-1502.9375,-329.46875,30671.1875]},{""id"":""11"",""type"":""planetaryNebula"",""name"":""Hypio Orb Nebula"",""galMapSearch"":""Hypio Pri JH-U e3-6719"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/418657\/name\/Hypio+Pri+JH-U+e3-6719"",""coordinates"":[1083.8125,-79.09375,25195.25]},{""id"":""12"",""type"":""nebula"",""name"":""Inner Rim Nebula"",""galMapSearch"":""Scheau Blao NS-U f2-1773"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2203343\/name\/Scheau+Blao+NS-U+f2-1773"",""coordinates"":[4315.875,-1108.53125,33488.03125]},{""id"":""13"",""type"":""nebula"",""name"":""Magnus Nebula"",""galMapSearch"":""Hypuae Briae YQ-Z c28-339"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/707350\/name\/Hypuae+Briae+YQ-Z+c28-339"",""coordinates"":[1063.3125,465.96875,36040.375]},{""id"":""14"",""type"":""planetaryNebula"",""name"":""Rose Nebula"",""galMapSearch"":""Eorld Byoe YA-W e2-4084"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/696984\/name\/Eorld+Byoe+YA-W+e2-4084"",""coordinates"":[-1259.84375,-177.4375,30270.28125]},{""id"":""15"",""type"":""historicalLocation"",""name"":""Sagittarius A*"",""galMapSearch"":""Sagittarius A*"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/25635\/name\/Sagittarius+A%2A"",""coordinates"":[25.21875,-20.90625,25899.96875]},{""id"":""17"",""type"":""nebula"",""name"":""Zunuae Nebula"",""galMapSearch"":""Zunuae HL-Y e6903"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/221094\/name\/Zunuae+HL-Y+e6903"",""coordinates"":[-437.34375,199.53125,23539.96875]},{""id"":""18"",""type"":""nebula"",""name"":""Umbra Centralis Nebula"",""galMapSearch"":""Scheau Prao XF-E d12-1389"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2205197\/name\/Scheau+Prao+XF-E+d12-1389"",""coordinates"":[2005.34375,-816.03125,25631.6875]},{""id"":""19"",""type"":""stellarRemnant"",""name"":""The Smasiae Red Giant Binary Pair"",""galMapSearch"":""Smasiae QT-Q d5-70"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/1493494\/name\/Smasiae+QT-Q+d5-70"",""coordinates"":[4260.0625,-1965.6875,32717.5]},{""id"":""20"",""type"":""nebula"",""name"":""Phipoea Nebula"",""galMapSearch"":""Phipoea HJ-D c27-3404"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2205239\/name\/Phipoea+HJ-D+c27-3404"",""coordinates"":[-490.03125,499.6875,28260.3125]},{""id"":""21"",""type"":""stellarRemnant"",""name"":""The Betelgeusian Brothers"",""galMapSearch"":""Phroi Pri CA-A d5672"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/946728\/name\/Phroi+Pri+CA-A+d5672"",""coordinates"":[-1151.0625,-1273.9375,25891.96875]},{""id"":""22"",""type"":""planetFeatures"",""name"":""The Crown of Skadi"",""galMapSearch"":""Eok Gree PI-S e4-4843"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/984582\/name\/Eok+Gree+PI-S+e4-4843"",""coordinates"":[-1376.96875,-331.96875,30511.03125]},{""id"":""23"",""type"":""planetFeatures"",""name"":""'Trinus' Blue"",""galMapSearch"":""Dryio Blue LJ-X d2-629"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2205274\/name\/Dryio+Blue+LJ-X+d2-629"",""coordinates"":[3920.5625,-1429.84375,28683.6875]},{""id"":""24"",""type"":""planetFeatures"",""name"":""Junon"",""galMapSearch"":""Byoomeae DN-B d13-1719"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/999129\/name\/Byoomeae+DN-B+d13-1719"",""coordinates"":[2636.21875,-1154.25,25717.0625]},{""id"":""25"",""type"":""minorPOI"",""name"":""Altum Sagittarii Prime"",""galMapSearch"":""Quemeou YE-A e0"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/1140773\/name\/Quemeou+YE-A+e0"",""coordinates"":[34.15625,2849.46875,25918.0625]},{""id"":""26"",""type"":""stellarRemnant"",""name"":""Six Rings"",""galMapSearch"":""Myriesly RY-S e3-5414"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/1028142\/name\/Myriesly+RY-S+e3-5414"",""coordinates"":[-1043.71875,124.9375,25279.4375]},{""id"":""27"",""type"":""planetFeatures"",""name"":""Poseidon"",""galMapSearch"":""Eos Chrea TD-Z c27-1678"",""galMapUrl"":""https:\/\/www.edsm.net\/en\/system\/id\/2205282\/name\/Eos+Chrea+TD-Z+c27-1678"",""coordinates"":[1109.65625,12.34375,28315.84375]},{""id"":""28"",""type"":""historicalLocation"",""name"":""Derthek's Folly(also known as 'Counter Point')"",""galMapSearch"":""Drooteou PW-I a36 - 4"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 143276\/ name\/ Drooteou + PW - I + a36 - 4"",""coordinates"":[51.5625,-40.53125,51800.53125]},{""id"":""29"",""type"":""nebula"",""name"":""Greeroi Veil"",""galMapSearch"":""Greeroi MT-O d7 - 3"",""galMapUrl"":""https:\/\/ www.edsm.net\/ en\/ system\/ id\/ 145216\/ name\/ Greeroi + MT - O + d7 - 3"",""coordinates"":[4617.96875,1193.53125,37984.65625]}";
            var converter = new JSONToPOIConverter();
            var items = converter.ConvertJSONToPOIs(testJson);
            Assert.AreEqual(27, items.Count);
            foreach(var item in items)
            {
                Assert.NotNull(item.Coordinates);
                Assert.NotNull(item.Id);
                Assert.NotNull(item.Name);
                Assert.NotNull(item.GalMapSearch);
                Assert.NotNull(item.GalMapUrl);
                Assert.NotNull(item.Type);
            }
        }
    }
}