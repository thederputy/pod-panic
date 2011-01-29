#region Using statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace PodPanic.GameState
{
    public static class LoadingManager
    {
        public static string pathToLevels = ".../../../Content/Levels/";

        public static XmlNode getXmlLevelNodeFromFile(String fileName)
        {
            XmlDocument levelXml = new XmlDocument();
            levelXml.Load(fileName);

            // Create the reader to read the xml document
            XmlTextReader levelReader = new XmlTextReader(fileName);
            levelReader.MoveToContent();

            XmlNode levelNode;

            while (true)
            {
                levelNode = levelXml.ReadNode(levelReader);
                if (levelNode.Name.Equals("Level"))
                {
                    break;  // we read a level node correctly?
                }
            }
            return levelNode;
        }

        public static void loadGraphicsAsset(String assetName)
        {
        }
    }
}
