#region Using Statements
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
#endregion

namespace PodPanic.GameObjects
{
    class GameObject
    {
        #region Attributes
        public Texture2D sprite;
        public Vector2 position;
        public float velocity;
        #endregion

        /// <summary>
        /// Creates a gameObject.
        /// </summary>
        /// <param name="loadedTexture"></param>
        public GameObject(Texture2D loadedTexture)
        {
            velocity = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
        }


    }
}
