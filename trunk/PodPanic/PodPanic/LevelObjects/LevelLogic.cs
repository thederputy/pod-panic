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
        public float FishToEnemyRatio {get; set;}
        public float PollutedWaterRatio { get; set; }
        public int NumberOfEnemies {get; set;}
        public int LevelLength {get; set;}
        public int LevelNumber {get; set;}
        public Texture2D Background {get; set;}
        public String LevelName {get; set;}
        public float CurrentPosition {get; set;}

        public static TimeSpan ElapsedGameTime; 

        public LevelLogic()
        {
        }

        public void setDataFromXml(XmlNode node)
        {
            setFishToEnemyRatioXML(node);
            setPollutedWaterRatioXML(node);
            setNumberOfEnemiesXML(node);
            setLevelLengthXML(node);
            setLevelNumberXML(node);
            setLevelNameXML(node);
        }

        public LevelLogic(Texture2D loadedTexture)
        {
            Background = loadedTexture;
        }

        private void setFishToEnemyRatioXML(XmlNode node)
        {
            //XmlNode root = node.FirstChild;

            String value;
            value = node.Attributes.GetNamedItem("FishToEnemyRatio").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        private void setPollutedWaterRatioXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("PollutedWaterRatio").InnerText;
            PollutedWaterRatio = Convert.ToSingle(value);
        }

        private void setNumberOfEnemiesXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("NumberOfEnemies").InnerText;
            NumberOfEnemies = Convert.ToInt32(value);
        }

        private void setLevelLengthXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelLength").InnerText;
            LevelLength = Convert.ToInt32(value);
        }

        private void setLevelNumberXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelNumber").InnerText;
            LevelNumber = Convert.ToInt32(value);
        }

        private void setLevelNameXML(XmlNode node)
        {
            LevelName = node.Attributes.GetNamedItem("LevelName").InnerText;
        }

        public int PercentCompleted()
        {
            return (int)(LevelLength / CurrentPosition)* 10;
        }
      
    }
}
