using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml;
using PodPanic.LevelObjects;

namespace PodPanic
{
    class Test
    {
        public static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Level  LevelNumber = '1'  FishToEnemyRatio = '1' PollutedWaterRatio = '1' NumberOfEnemies = '1' LevelLength = '1'/>");

            LevelLogic tester = new LevelLogic();

            tester.setFishToEnemyRatioXML(doc.FirstChild);
            tester.setLevelNumberXML(doc.FirstChild);
            tester.setFishToEnemyRatioXML(doc.FirstChild);
            tester.setPollutedWaterRatioXML(doc.FirstChild);
            tester.setNumberOfEnemiesXML(doc.FirstChild);
            tester.setLevelLengthXML(doc.FirstChild);

            Console.WriteLine(tester.getFishToEnemyRatio());


        }
    }
}