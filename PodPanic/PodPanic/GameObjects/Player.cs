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
        public const float ROT_SPEED = 0.02f;
        public const int THRESHOLD = 75;
        public const int YSPEED = 5;
        public const float MAX_ROT = (float)Math.PI / 6;
        public Vector2 SIZE_OF_LEAD_WHALE = new Vector2(100, 60);// 111, 83
        public Vector2 SIZE_OF_WHALE = new Vector2(80, 50); // 99, 75

        public Vector2 OFFSET_LEADWHALE = new Vector2(250, 45); // 99,33
        public Vector2 OFFSET_REARWHALE = new Vector2(125, 38); // 0,38
        public Vector2 OFFSET_TOPWHALE = new Vector2(175, 0); // 50,0
        public Vector2 OFFSET_BOTWHALE = new Vector2(175, 86); // 50,86 
        private float currHP;
        private float currRot;
        public static int Speed { get; set; }


        private const float BOB_RATE = 0.02f;
        private const int OFFY_AMOUNT = 15;

        private const int ANIMATE_SPEED = 75;
        private const int SPRITE_HEIGHT = 28;
        private const int SPRITE_WIDTH = 78;
        private const int FRAMES = 4;
        private int timeCounter;
        private int animationPointer;
        private Rectangle source;


        Fish_Bobber bobber_lead;
        Fish_Bobber bobber_top;
        Fish_Bobber bobber_bottom;
        Fish_Bobber bobber_rear;


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
            source = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            position = new Vector2(50, ((PodPanic)(this.Game)).getYChannel(currChannel));
            currRot = 0.0f;
            animationPointer = 0;

            Random rnd = new Random();


            bobber_lead = new Fish_Bobber((float)rnd.NextDouble(), 1f, BOB_RATE, OFFY_AMOUNT);
            bobber_top = new Fish_Bobber((float)rnd.NextDouble(), -1f, BOB_RATE, OFFY_AMOUNT);
            bobber_bottom = new Fish_Bobber((float)rnd.NextDouble(), -1f, BOB_RATE, OFFY_AMOUNT);
            bobber_rear = new Fish_Bobber((float)rnd.NextDouble(), 1f, BOB_RATE, OFFY_AMOUNT);

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
        public void moveRight()
        {
            position.X += 4;
            if (position.X >= 350)
                position.X -= 2;
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
                if (currRot > ROT_SPEED*2)
                    currRot -= ROT_SPEED;
                else if (currRot < -ROT_SPEED * 2)
                    currRot += ROT_SPEED;
                
                    
                
            }
            if (position.X >= 50)
                position.X -= 2;

            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (timeCounter / ANIMATE_SPEED > 0)
            {
                animationPointer++;
                animationPointer = animationPointer % FRAMES;
                source = new Rectangle(SPRITE_WIDTH * animationPointer, 0, SPRITE_WIDTH, SPRITE_HEIGHT);


            }

            timeCounter = timeCounter % ANIMATE_SPEED;

            bobber_lead.Update();
            bobber_bottom.Update();
            bobber_rear.Update();
            bobber_top.Update();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            source = new Rectangle(SPRITE_WIDTH * animationPointer, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Lead Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)(position.X + OFFSET_LEADWHALE.X), (int)(position.Y + OFFSET_LEADWHALE.Y + bobber_lead.getOff()), (int)SIZE_OF_LEAD_WHALE.X, (int)SIZE_OF_LEAD_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            source = new Rectangle(SPRITE_WIDTH * ((animationPointer+1) % FRAMES), 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Rear Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)(position.X + OFFSET_REARWHALE.X), (int)(position.Y + OFFSET_REARWHALE.Y + bobber_rear.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            source = new Rectangle(SPRITE_WIDTH * ((animationPointer + 2) % FRAMES), 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Top Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)(position.X + OFFSET_TOPWHALE.X), (int)(position.Y + OFFSET_TOPWHALE.Y + bobber_top.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            source = new Rectangle(SPRITE_WIDTH * ((animationPointer + 3) % FRAMES), 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Bottom Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)(position.X + OFFSET_BOTWHALE.X), (int)(position.Y + OFFSET_BOTWHALE.Y + bobber_bottom.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
        }
    }
}
