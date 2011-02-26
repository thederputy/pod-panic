using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PodPanic.LevelObjects
{
    public static class LevelData
    {
        public static XmlDocument level1 = initLevel1();
        public static XmlDocument level2 = initLevel2();
        public static XmlDocument level3 = initLevel3();
        public static XmlDocument level4 = initLevel4();
        public static string[,] levelBKGs = initLevelBKGs();

        private static XmlDocument initLevel1()
        {
            XmlDocument lvl = new XmlDocument();
            string xmlData = "<?xml version='1.0' encoding='utf-8'?>";
            xmlData += "<XnaContent>";
            xmlData += "  <Level";
            xmlData += " LevelNumber='1'";
            xmlData += " LevelName='Burrard Inlet'";
            xmlData += " LevelLength='2000'";
            xmlData += " PollutionLevel='0'";
            xmlData += " ProbabilityFishPollution='0.5'";
            xmlData += " ProbabilityEnemyType='0'";
            xmlData += " ProbabilityEnemyFish='0.1'";
            xmlData += " TimeBetweenEvents='50' />";
            xmlData += "</XnaContent>";
            lvl.LoadXml(xmlData);
            return lvl;
        }

        private static XmlDocument initLevel2()
        {
            XmlDocument lvl = new XmlDocument();
            string xmlData = "<?xml version='1.0' encoding='utf-8'?>";
            xmlData += "<XnaContent>";
            xmlData += "  <Level";
            xmlData += " LevelNumber='2'";
            xmlData += " LevelName='Salish Swim'";
            xmlData += " LevelLength='7000'";
            xmlData += " PollutionLevel='0.2'";
            xmlData += " ProbabilityFishPollution='0.2'";
            xmlData += " ProbabilityEnemyType='0.3'";
            xmlData += " ProbabilityEnemyFish='0.4'";
            xmlData += " TimeBetweenEvents='40' />";
            xmlData += "</XnaContent>";
            lvl.LoadXml(xmlData);
            return lvl;
        }

        private static XmlDocument initLevel3()
        {
            XmlDocument lvl = new XmlDocument();
            string xmlData = "<?xml version='1.0' encoding='utf-8'?>";
            xmlData += "<XnaContent>";
            xmlData += "  <Level";
            xmlData += " LevelNumber='3'";
            xmlData += " LevelName='Georgia Gateway'";
            xmlData += " LevelLength='15000'";
            xmlData += " PollutionLevel='0.3'";
            xmlData += " ProbabilityFishPollution='0.3'";
            xmlData += " ProbabilityEnemyType='0.7'";
            xmlData += " ProbabilityEnemyFish='0.6'";
            xmlData += " TimeBetweenEvents='35' />";
            xmlData += "</XnaContent>";
            lvl.LoadXml(xmlData);
            return lvl;
        }

        private static XmlDocument initLevel4()
        {
            XmlDocument lvl = new XmlDocument();
            string xmlData = "<?xml version='1.0' encoding='utf-8'?>";
            xmlData += "<XnaContent>";
            xmlData += "  <Level";
            xmlData += " LevelNumber='4'";
            xmlData += " LevelName='Juan de Fuca Strait'";
            xmlData += " LevelLength='25000'";
            xmlData += " PollutionLevel='0.6'";
            xmlData += " ProbabilityFishPollution='0.7'";
            xmlData += " ProbabilityEnemyType='0.8'";
            xmlData += " ProbabilityEnemyFish='0.85'";
            xmlData += " TimeBetweenEvents='35' />";
            xmlData += "</XnaContent>";
            lvl.LoadXml(xmlData);
            return lvl;
        }
        private static string[,] initLevelBKGs()
        {
            return new string[,]
            {
                {"Water_Final", "MidGround", "SkyandDepth"},
                {"Fore1", "Mid1", "Back1"},
                {"Fore2", "Mid2", "Back2"},
                {"Fore3", "Mid3", "Back3"}
            };
        }
    }
}