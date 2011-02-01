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
        protected Boolean isDead;
        public bool hasHitPlayer { get; set; }
        public bool signalRemoval { get; set; }
        /// <summary>
        /// The amount of time the object has been alive for
        /// </summary>
        protected double timeAlive;
        /// <summary>
        /// The time the object hit the player
        /// </summary>
        protected double timeOfDeath;
        static private Random rnds;
        private int SPRITE_WIDTH;
        private int SPRITE_HEIGHT;
        protected Rectangle rect;
        static private Boolean firsts = true;
        private const float SIZEVARIATION = 0.2f; 

        /// <summary>
        /// Used for collision detection
        /// </summary>
        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        protected GameState.AlphaBlinker blinker;

        #endregion

        public Color drawColor { get; set; }

        /// <summary>
        /// Creates a gameObject.
        /// </summary>
        /// <param name="loadedTexture"></param>
        public GameObject(Texture2D loadedTexture, Game game)
            : base(game)
        {
            if (firsts)
            {
                rnds = new Random();
                firsts = false;
            }

            float reduction = 1f - (SIZEVARIATION * (float)rnds.NextDouble());
            SPRITE_WIDTH = (int)((float)loadedTexture.Width * reduction);
            SPRITE_HEIGHT = (int)((float)loadedTexture.Height * reduction);
            signalRemoval = false;
            blinker = new global::PodPanic.GameState.AlphaBlinker();
            drawColor = Color.White;
            velocity = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
            rect = new Rectangle();
            rect.Width = SPRITE_WIDTH;
            rect.Height = SPRITE_HEIGHT;
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
        /// Updates the game object 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timeAlive += gameTime.ElapsedRealTime.Milliseconds;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
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
            Color finDrawColor = drawColor;
            if (isDead) finDrawColor.A = (byte)blinker.AlphaVal;
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X -50, (int)position.Y- 25, SPRITE_WIDTH, SPRITE_HEIGHT), finDrawColor);
            base.Draw(gameTime);
        }

    }
}
