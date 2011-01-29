#region Using statements
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
        private float currHP;
        private float currRot;

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
            int threshold = 75;
            updateAlive();
            if (position.Y > ((PodPanic)(this.Game)).getYChannel(currChannel))
            {
                position.Y -= 2;
                if (currRot > -Math.PI / 6 && position.Y - ((PodPanic)(this.Game)).getYChannel(currChannel) > threshold)
                {
                    currRot -= 0.01f;
                }
                else if (position.Y - ((PodPanic)(this.Game)).getYChannel(currChannel) < threshold)
                {
                    currRot += 0.01f;
                }
            }
            else if (position.Y < ((PodPanic)(this.Game)).getYChannel(currChannel))
            {
                position.Y += 2;
                if (currRot < Math.PI / 6 && ((PodPanic)(this.Game)).getYChannel(currChannel) - position.Y > threshold)
                {
                    currRot += 0.01f;
                }
                else if (((PodPanic)(this.Game)).getYChannel(currChannel) - position.Y < threshold)
                {
                    currRot -= 0.01f;
                }
            }
            else
            {

                if (currRot > 0.0f)
                    currRot -= 0.01f;
                else if (currRot < 0.0f)
                    currRot += 0.01f;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 99, (int)position.Y + 33, 111, 83), null, drawColor, currRot, new Vector2(400,300), SpriteEffects.None, 0);
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y + 38, 99, 75), null, drawColor, currRot, new Vector2(400, 300), SpriteEffects.None, 0);
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 50, (int)position.Y, 99, 75), null, drawColor, currRot, new Vector2(400, 300), SpriteEffects.None, 0);
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + 50, (int)position.Y + 86, 99, 75), null, drawColor, currRot, new Vector2(400, 300), SpriteEffects.None, 0);
        }
    }
}
