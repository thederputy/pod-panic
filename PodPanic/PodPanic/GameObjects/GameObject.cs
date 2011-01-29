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
    class GameObject : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Attributes
        protected Texture2D sprite;
        protected Vector2 position;
        protected float velocity;
        #endregion

        /// <summary>
        /// Creates a gameObject.
        /// </summary>
        /// <param name="loadedTexture"></param>
        public GameObject(Texture2D loadedTexture, Game game)
            : base(game)
        {
            velocity = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
        }

        /// <summary>
        /// 
        /// returns the sprite of the object
        /// </summary>
        /// <returns>returns a 2d texture</returns>
        public Texture2D getTexture()
        {
            return sprite;
        }


        /// <summary>
        /// updates
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// initializes
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }


        /// <summary>
        /// 
        /// function use to get the vector for the top left corner
        /// </summary>
        /// <returns> returns a vector that contains, </returns>
        public Vector2 getPosition()
        {
            return position;
        }


        public override void  Draw(GameTime gameTime)
        {
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, position, Color.White);
            base.Draw(gameTime);
        }

    }
}
