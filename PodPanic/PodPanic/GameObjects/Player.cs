﻿#region Using statements
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
using PodPanic.GameState;
#endregion

namespace PodPanic.GameObjects
{
    class Player : GameObject
    {
        public const float ROT_SPEED = 0.02f;
        public const int THRESHOLD = 75;
        public const int YSPEED = 5;
        public const float MAX_ROT = (float)Math.PI / 6;
        public Vector2 SIZE_OF_LEAD_WHALE = new Vector2(111, 83);
        public Vector2 SIZE_OF_WHALE = new Vector2(99, 75);
        public Vector2 OFFSET_LEADWHALE = new Vector2(99,33);
        public Vector2 OFFSET_REARWHALE = new Vector2(0,38);
        public Vector2 OFFSET_TOPWHALE = new Vector2(50, 0);
        public Vector2 OFFSET_BOTWHALE = new Vector2(50, 86);
        private float currHP;
        private float currRot;

        private const int ANIMATE_SPEED = 100;// in milliseconds
        private const int SPRITE_HEIGHT = 70;
        private const int SPRITE_WIDTH = 190;//380


        /// <summary>
        /// Stores the current HP of the unit.
        /// Read-only method. The actual HP change will happen in the <code>recuceHP</code>
        /// and <code>increaseHP</code> methods.
        /// </summary>
        public float CurrHP
        {
            get { return currHP; }
        }

        /// <summary>
        /// Stores the total health of the player.
        /// </summary>
        public static float TotalHP = 100;

        private bool alive;

        /// <summary>
        /// Stores whether the unit is alive or not, so that we can decide whether to draw it.
        /// This is a read-only field as it is set by the CurrHP.
        /// </summary>
        public bool Alive
        {
            get
            {
                updateAlive();
                return alive; 
            }
        }

        /// <summary>
        /// Represents the chanel that the player is in.
        /// Either Top, Middle, or Bottom.
        /// </summary>
        public Channel currChannel;




        /// <summary>
        /// Constructor for the Player.
        /// </summary>
        /// <param name="loadedTexture"></param>
        public Player(Texture2D loadedTexture, Game game)
            :base(loadedTexture, game)
        {
            position = new Vector2(50, ((PodPanic)(this.Game)).getYChannel(currChannel));
            currRot = 0.0f;
        }





        /// <summary>
        /// Reduces the player's health. If it is 0, they will be marked as not alive.
        /// </summary>
        /// <param name="damageAmount">the amount to decrease the HP by</param>
        public void reduceHP(float damageAmount)
        {
            currHP -= damageAmount;
            if (currHP < 0)
            {
                currHP = 0;
            }
        }

        /// <summary>
        /// Increases the player's health.
        /// </summary>
        /// <param name="bonusAmount">the amount to increase the HP by</param>
        public void increaseHP(float bonusAmount)
        {
            currHP += 10;
            if (currHP > TotalHP)
            {
                currHP = TotalHP;
            }
        }

        /// <summary>
        /// Updates whether the player is alive or not.
        /// </summary>
        private void updateAlive()
        {
            if (currHP > 0)
            {
                alive = true;
            }
            else
            {
                alive = false;
            }
        }

        /// <summary>
        /// Moves the player up to the next channel.
        /// </summary>
        public void moveUp()
        {
            switch (currChannel)
            {
                case Channel.Bottom:
                    currChannel = Channel.Middle;
                    break;
                case Channel.Middle:
                    currChannel = Channel.Top;
                    break;
                case Channel.Top:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Moves the player down to the next channel
        /// </summary>
        public void modeDown()
        {
            switch (currChannel)
            {
                case Channel.Top:
                    currChannel = Channel.Middle;
                    break;
                case Channel.Middle:
                    currChannel = Channel.Bottom;
                    break;
                case Channel.Bottom:
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            updateAlive();
            if (position.Y > ((PodPanic)(this.Game)).getYChannel(currChannel))
            {
                position.Y -= YSPEED;
                if (currRot > -MAX_ROT && position.Y - ((PodPanic)(this.Game)).getYChannel(currChannel) > THRESHOLD)
                {
                    currRot -= ROT_SPEED;
                }
                else if (position.Y - ((PodPanic)(this.Game)).getYChannel(currChannel) < THRESHOLD)
                {
                    currRot += ROT_SPEED;
                }
            }
            else if (position.Y < ((PodPanic)(this.Game)).getYChannel(currChannel))
            {
                position.Y += YSPEED;
                if (currRot < MAX_ROT && ((PodPanic)(this.Game)).getYChannel(currChannel) - position.Y > THRESHOLD)
                {
                    currRot += ROT_SPEED;
                }
                else if (((PodPanic)(this.Game)).getYChannel(currChannel) - position.Y < THRESHOLD)
                {
                    currRot -= ROT_SPEED;
                }
            }
            else
            {
                if (currRot > 0.0f)
                    currRot -= ROT_SPEED;
                else if (currRot < 0.0f)
                    currRot += ROT_SPEED;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw the Lead Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 99, (int)position.Y + 33, (int)SIZE_OF_LEAD_WHALE.X, (int)SIZE_OF_LEAD_WHALE.Y), null, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            //Draw the Rear Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y + 38, (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), null, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            //Draw the Top Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 50, (int)position.Y, (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), null, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            //Draw the Bottom Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 50, (int)position.Y + 86, (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), null, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
        }
    }
}