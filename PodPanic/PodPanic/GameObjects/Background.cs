using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace PodPanic.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Texture2D BackgroundTexture { get; set; }
        public Color drawColor { get; set; } 
        private int curXPos;
        public Background(Game game)
            : base(game)
        {
            drawColor = Color.White;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            curXPos += 1;
            if (curXPos >= BackgroundTexture.Width)
                curXPos = 0;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ((PodPanic)(this.Game)).spriteBatch.Draw(BackgroundTexture, new Vector2(-curXPos, -75), drawColor);
            ((PodPanic)(this.Game)).spriteBatch.Draw(BackgroundTexture, new Vector2(BackgroundTexture.Width - curXPos, -75), drawColor);
            base.Draw(gameTime);
        }
    }
}