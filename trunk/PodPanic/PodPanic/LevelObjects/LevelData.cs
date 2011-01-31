using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PodPanic.LevelObjects
{
    public static class LevelData
    {
        public static XmlDocument level1;
        public static XmlDocument level2;
        public static XmlDocument level3;
        public static XmlDocument level4;

        private static void initLevel1()
        {
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
            level1.LoadXml(xmlData);
        }

        private static void initLevel2()
        {
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
            level1.LoadXml(xmlData);
        }

        private static void initLevel3()
        {
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
            level1.LoadXml(xmlData);
        }

        private static void initLevel4()
        {
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
            level1.LoadXml(xmlData);
        }
    }
}