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


namespace PodPanic.LevelObjects
{
    class LevelLogic
    {

        public int LevelLength { get; set;}
        public float ProbabilityFishPollution { get; set; }
        public float ProbabilityEnemyType { get; set; }
        public float ProbabilityEnemyFish { get; set;}
        public int TimeBetweenEvents { get; set;}
        public float PollutionLevel { get; set; }
        public String LevelName { get; set; }
        public int LevelNumber { get; set; }
        public int CurrentPosition { get; set; }

        public Texture2D Background {get; set;}

        public static TimeSpan ElapsedGameTime; 


        public LevelLogic()
        {
        }

        public LevelLogic(Texture2D loadedTexture)
        {
            Background = loadedTexture;
        }

        public void setDataFromXml(XmlNode node)
        {
            setLevelLengthXML(node);
            setProbabilityEnemyTypeXML(node);
            setProbabilityFishPollutionXML(node);
            setProbabilityEnemyTypeXML(node);
            setProbabilityEnemyFishXML(node);
            setTimeBetweenEventsXML(node);
            setPollutionLevelXML(node);
            setLevelNameXML(node);
            setLevelNumberXML(node);
        }

        private void setLevelLengthXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelLength").InnerText;
            LevelLength = Convert.ToInt32(value);
        }

        private void setProbabilityEnemyTypeXMP(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("ProbabilityEnemyType").InnerText;
            ProbabilityEnemyType = (float)Convert.ToDouble(value);
        }
        
        private void setProbabilityFishPollutionXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("ProbabilityFishPollution").InnerText;
            ProbabilityFishPollution = (float)Convert.ToDouble(value);
        } 

        private void setProbabilityEnemyTypeXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("ProbabilityEnemyType").InnerText;
            ProbabilityEnemyType = (float)Convert.ToDouble(value);
        }

        private void setProbabilityEnemyFishXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("ProbabilityEnemyFish").InnerText;
            ProbabilityEnemyFish = (float)Convert.ToDouble(value);
        }

        private void setTimeBetweenEventsXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("TimeBetweenEvents").InnerText;
            TimeBetweenEvents = Convert.ToInt32(value);
        }

        private void setPollutionLevelXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("PollutionLevel").InnerText;
            PollutionLevel = Convert.ToSingle(value);
        }

        private void setLevelNameXML(XmlNode node)
        {
            LevelName = node.Attributes.GetNamedItem("LevelName").InnerText;
        }

        private void setLevelNumberXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelNumber").InnerText;
            LevelNumber = Convert.ToInt32(value);
        }

        public int PercentCompleted()
        {
            return (int)(CurrentPosition/LevelLength) * 10;
        }
      
    }
}
