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

        public float FishToEnemyRatio;
        public float PollutedWaterRatio;
        public int NumberOfEnemies;
        public int LevelLength;
        public int LevelNumber;
        public Texture2D Background;
        public static TimeSpan ElapsedGameTime;

        public LevelLogic()
        {
            setFishToEnemyRatio(0);
            setLevelLength(0);
            setNumberOfEnemies(0);
            setPollutedWaterRatio(0);
            setLevelNumber(0);
        }

        public LevelLogic(Texture2D loadedTexture)
        {
            Background = loadedTexture;
        }

        public void setFishToEnemyRatio(float ratio)
        {
            FishToEnemyRatio = ratio;
        }

        public void setFishToEnemyRatioXML(XmlNode node)
        {
            //XmlNode root = node.FirstChild;

            String value;
            value = node.Attributes.GetNamedItem("FishToEnemyRatio").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        public float getFishToEnemyRatio()
        {
            return FishToEnemyRatio;
        }

        public void setPollutedWaterRatio(float ratio)
        {
            PollutedWaterRatio = ratio;
        }

        public void setPollutedWaterRatioXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("PollutedWaterRatio").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        public float getPollutedWaterRatio()
        {
            return PollutedWaterRatio;
        }

        public void setNumberOfEnemies(int number)
        {
            NumberOfEnemies = number;
        }

        public void setNumberOfEnemiesXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("NumberOfEnemies").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        public int getNumberOfEnemies()
        {
            return NumberOfEnemies;
        }

        public void setLevelLength(int length)
        {
            LevelLength = length;
        }

        public void setLevelLengthXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelLength").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        public int getLevelLength()
        {
            return LevelLength;
        }

        public void setLevelNumber(int level)
        {
            LevelNumber = level;
        }

        public void setLevelNumberXML(XmlNode node)
        {
            String value;
            value = node.Attributes.GetNamedItem("LevelNumber").InnerText;
            FishToEnemyRatio = Convert.ToSingle(value);
        }

        public int getLevelNumber()
        {
            return LevelNumber;
        }

    }
}
