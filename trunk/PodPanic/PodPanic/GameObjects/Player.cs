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
        public const int MAX_HP = 10;
        public const int MAX_LIVES = 6;
        public const float MAX_ROT = (float)Math.PI / 6;
        public Vector2 SIZE_OF_LEAD_WHALE = new Vector2(133, 48);// 100, 36
        public Vector2 SIZE_OF_WHALE = new Vector2(80, 35); // 80, 29

        public Vector2 OFFSET_LEADWHALE = new Vector2(280, 45); // 250, 45
        public Vector2 OFFSET_REARWHALE = new Vector2(125, 38); // 125, 38
        public Vector2 OFFSET_TOPWHALE = new Vector2(175, 0); // 175, 0
        public Vector2 OFFSET_BOTWHALE = new Vector2(175, 86); // 175, 86
        private int currHP;
        private int livesOwned;

        public int LivesOwned
        {
            get { return livesOwned; }
            set { livesOwned = value; }
        }
        private float currRot;
        public static int Speed { get; set; }

        private Boolean flip1;
        private Boolean flip2;
        private Boolean flip3;
        private Boolean flip4;

        private const float BOB_RATE = 0.02f;
        private const int OFFY_AMOUNT = 15;

        private const int ANIMATE_SPEED = 80;
        private const int SPRITE_HEIGHT = 84;
        private const int SPRITE_WIDTH = 235;
        private const int FRAMES = 4;
        private int timeCounter;
        
        private int animationPointer1;
        private int animationPointer2;
        private int animationPointer3;
        private int animationPointer4;

        private int left1;
        private int left2;
        private int left3;

        private Rectangle source;

        Fish_Bobber bobber_lead;
        Fish_Bobber bobber_top;
        Fish_Bobber bobber_bottom;
        Fish_Bobber bobber_rear;

        private Texture2D dmg_sprite;
        private bool isInDamagedState;
        private int numBlinks;
        private bool doOnceBlinkCount;

        /// <summary>
        /// Stores the current HP of the unit.
        /// Read-only method. The actual HP change will happen in the <code>recuceHP</code>
        /// and <code>increaseHP</code> methods.
        /// </summary>
        public float CurrHP
        {
            get { return currHP; }
        }

        public void reset()
        {
            Speed = 1;
            currHP = MAX_HP;
            livesOwned = MAX_LIVES;
            left1 = 0;
            left2 = 0;
            left3 = 0;

        }

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
        public Player(Texture2D loadedTexture, Texture2D dmgTexture, Game game)
            :base(loadedTexture, game)
        {
            isInDamagedState = false;
            dmg_sprite = dmgTexture;
            source = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            position = new Vector2(50, ((PodPanic)(this.Game)).getYChannel(currChannel));
            currRot = 0.0f;
            animationPointer1 = 0;
            animationPointer2 = 1;
            animationPointer3 = 2;
            animationPointer4 = 3;

            currHP = MAX_HP;

            Random rnd = new Random();

            left1 = 0;
            left2 = 0;
            left3 = 0;

            bobber_lead = new Fish_Bobber((float)rnd.NextDouble(), 1f, BOB_RATE, OFFY_AMOUNT);
            bobber_top = new Fish_Bobber((float)rnd.NextDouble(), -1f, BOB_RATE, OFFY_AMOUNT);
            bobber_bottom = new Fish_Bobber((float)rnd.NextDouble(), -1f, BOB_RATE, OFFY_AMOUNT);
            bobber_rear = new Fish_Bobber((float)rnd.NextDouble(), 1f, BOB_RATE, OFFY_AMOUNT);
            livesOwned = MAX_LIVES;
            currHP = MAX_HP;
            blinker = new AlphaBlinker();
            rect.Width = 150;
        }

        public int getHealthPercent()
        {
            return (int)( (float)livesOwned / (float)MAX_LIVES * 100.0f);
        }

        /// <summary>
        /// Reduces the player's health. If it is 0, they will be marked as not alive.
        /// </summary>
        /// <param name="damageAmount">the amount to decrease the HP by</param>
        public void reduceHP(int damageAmount)
        {
            currHP -= damageAmount;
            if (currHP < 0)
            {
                currHP = 0;
                updateAlive();
            }
            isInDamagedState = true;
        }

        /// <summary>
        /// Increases the player's health.
        /// </summary>
        /// <param name="bonusAmount">the amount to increase the HP by</param>
        public void increaseHP(float bonusAmount)
        {
            currHP += (int)bonusAmount;
            if (currHP > MAX_HP)
            {
                currHP = MAX_HP;
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
                livesOwned -= 1;
                currHP = MAX_HP;
                if (livesOwned <= 0)
                {
                    ((PodPanic)this.Game).endGameFail();
                }
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
            base.Update(gameTime);
            blinker.Update(gameTime);
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

                if (flip1)
                   animationPointer1--;
                else
                    animationPointer1++;
                animationPointer1 = animationPointer1 % FRAMES;
                if (animationPointer1 == 0 || animationPointer1 == 3)
                 flip1 =!flip1;

                if (flip2)
                    animationPointer2--;
                else
                    animationPointer2++;
                animationPointer2 = animationPointer2 % FRAMES;
                if (animationPointer2 == 0 || animationPointer2 == 3)
                    flip2 = !flip2;

                if (flip3)
                    animationPointer3--;
                else
                    animationPointer3++;
                animationPointer3 = animationPointer3 % FRAMES;
                if (animationPointer3 == 0 || animationPointer3 == 3)
                    flip3 = !flip3;


                if (animationPointer4 == 0 || animationPointer4 == 3)
                    flip4 = !flip4;
                if (flip4)
                    animationPointer4--;
                else
                    animationPointer4++;
                animationPointer4 = animationPointer4 % FRAMES;
                
            }


            timeCounter = timeCounter % ANIMATE_SPEED;

            bobber_lead.Update();
            bobber_bottom.Update();
            bobber_rear.Update();
            bobber_top.Update();

            //System.Diagnostics.Trace.WriteLine(currRot);
            //(animationPointer + 1) - (((animationPointer + 1) / 3) * ((animationPointer ) % 4))
            //System.Diagnostics.Trace.WriteLine((animationPointer + 1) - (((animationPointer + 1) / 4) * ((animationPointer) % 4)));
            //System.Diagnostics.Trace.WriteLine(animationPointer);
            
            if (currRot > 0.54)
            {
                currRot = 0.54f;
            }
            if (currRot < -0.54)
            {
                currRot = -0.54f;
            }
            rect.X = (int)position.X + 50;
            rect.Y = (int)position.Y + 50;
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D drawnTex;
            if (isInDamagedState && blinker.AlphaVal == 255)
            {
                drawnTex = dmg_sprite;
                if(!doOnceBlinkCount)
                    numBlinks += 1;
            }
            else
            {
                doOnceBlinkCount = false;
                drawnTex = sprite;
                if (numBlinks >= 3)
                {
                    isInDamagedState = false;
                }
            }
            //livesOwned--;

            source = new Rectangle(SPRITE_WIDTH * animationPointer1, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Lead Whale
            ((PodPanic)(this.Game)).spriteBatch.Draw(drawnTex, new Rectangle((int)(position.X + OFFSET_LEADWHALE.X), (int)(position.Y + OFFSET_LEADWHALE.Y + bobber_lead.getOff()), (int)SIZE_OF_LEAD_WHALE.X, (int)SIZE_OF_LEAD_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            
            
            source = new Rectangle(SPRITE_WIDTH * animationPointer2 , 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Rear Whale
            if (livesOwned < MAX_LIVES * 0.50 && left2 < 500)
            {
                left2++;
            }

            ((PodPanic)(this.Game)).spriteBatch.Draw(drawnTex, new Rectangle((int)(position.X + OFFSET_REARWHALE.X - left2), (int)(position.Y + OFFSET_REARWHALE.Y + bobber_rear.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            
            source = new Rectangle(SPRITE_WIDTH * animationPointer3, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Top Whale
            if (livesOwned < MAX_LIVES*0.75 && left1 < 500)
            {
                left1++;
            }
            ((PodPanic)(this.Game)).spriteBatch.Draw(drawnTex, new Rectangle((int)(position.X + OFFSET_TOPWHALE.X - left1), (int)(position.Y + OFFSET_TOPWHALE.Y + bobber_top.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            
            
            source = new Rectangle(SPRITE_WIDTH * animationPointer4, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Draw the Bottom Whale
            if (livesOwned < MAX_LIVES*0.25 && left3 < 500)
            {
                left3++;
            }
            ((PodPanic)(this.Game)).spriteBatch.Draw(drawnTex, new Rectangle((int)(position.X + OFFSET_BOTWHALE.X - left3), (int)(position.Y + OFFSET_BOTWHALE.Y + bobber_bottom.getOff()), (int)SIZE_OF_WHALE.X, (int)SIZE_OF_WHALE.Y), source, drawColor, currRot, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            
            //System.Diagnostics.Trace.WriteLine(" this : " + livesOwned / MAX_LIVES * 100.0f);
            //System.Diagnostics.Trace.WriteLine(" lives : " + livesOwned);
        }

        //  - (((animationPointer+1)/3)*((animationPointer+1)%3))


        public int whatVictory()
        {
            if (livesOwned >= ((int)MAX_LIVES * 0.66))
                return 3;
            else if (livesOwned >= ((int)MAX_LIVES * 0.33))
                return 2;
            else
                return 1;
        }
    }
}
