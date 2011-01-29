using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PodPanic.GameObjects
{
    class Fish : GameObject  {


        static private float polluted_percent;
        private Boolean isPolluted;
        private float offY;
        private float dir;
        private int baseY;
        private int timeCounter;
        private int animationPointer;
        private List<Fish_Bobber> bobbers;
        private Rectangle source;


        private const int OFFY_AMOUNT = 15;
        private const float SLOWEST_SPEED = 2.25f;
        private const float FASTEST_SPEED = 4.0f;
        private const float BOB_RATE = 0.02f;
        private const int ANIMATE_SPEED = 700;// in milliseconds
        private const float FISH_LENGTH = 60.0f;
        private const int SPRITE_HEIGHT = 70;
        private const int SPRITE_WIDTH = 190;//380
        private const int FRAMES = 2;
        private const int FISHNUM = 12;
        private const int FISHDISTANCEX = 50;
        private const int FISHDISTANCEY = 20;
        private const float FISHSIZEVARIATION = 30;



        private Random rnd;

        
        

        
        /// <summary>
        /// create a fish that moves from right to left
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="normFish">image for normal fish</param>
        /// <param name="sickFish">image for sick fish</param>
        /// 

        public Fish(int x, int y,Texture2D normFish, Texture2D sickFish, Game game)
            : base(null,game)
        {



            source = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);

            position.X = x;
            position.Y = y;
            rnd = new Random();
            baseY = y;
            velocity = SLOWEST_SPEED + (FASTEST_SPEED - SLOWEST_SPEED * (float)rnd.NextDouble());
            animationPointer = 0;
            System.Diagnostics.Trace.WriteLine("");

            
            bobbers = new List<Fish_Bobber>();

            for (int i = 0; i < FISHNUM; i++)
            {
                offY = (float)rnd.NextDouble();

                if (rnd.NextDouble() > 0.5f)
                    dir = 1;
                else
                    dir = -1;

                bobbers.Add(new Fish_Bobber(offY, dir, BOB_RATE, OFFY_AMOUNT, FISHSIZEVARIATION));
            }


            //bobbers.Add(new Fish_Bobber());
            

            if (rnd.NextDouble() <= polluted_percent)
            {
                isPolluted = true;
                //spriteAnimations = sickFish;
                sprite = sickFish;
            }
            else
            {
                //spriteAnimations = normFish;    
                sprite = normFish;
            }

        }

       

        /// <summary>
        /// sets the percent chance of a fish being sick
        /// </summary>
        /// <param name="percent"> a float between 0 and 1 that represent a percentage</param>
        static public void setPollutionPercent(float percent){
            polluted_percent = percent;       
        }

        


        /// <summary>
        /// update moves the fish forward
        /// </summary>
        /// <param name="gameTime">game time object</param>
        public override void Update(GameTime gameTime)
        {
            /*
            offY += dir * BOB_RATE;
            if ((dir > 0) && (offY > 1))
            {
                offY = 1;
                dir = -1;
            }
            else if ((dir < 0) && (offY < -1))
            {
                offY = -1;
                dir = 1;
            }
             */
            for (int i = 0; i < bobbers.Count; i++)
            {
               bobbers[i].Update();
            }

            position.X -= velocity;
            //position.Y = baseY + (OFFY_AMOUNT * offY);
            position.Y = baseY;


           // System.Diagnostics.Trace.WriteLine(" hey : " + gameTime.ElapsedRealTime.Milliseconds);

            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

            if(timeCounter/ANIMATE_SPEED > 0){
                animationPointer++;
                animationPointer = animationPointer %FRAMES;
                source = new Rectangle(SPRITE_WIDTH * animationPointer, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
                
                //sprite = spriteAnimations[animationPointer];
            }

            timeCounter = timeCounter % ANIMATE_SPEED;

            //System.Diagnostics.Trace.WriteLine(" hey2 : " + timeCounter);
            //System.Diagnostics.Trace.WriteLine(" hey3 : " + (float)(timeCounter)/1000);
            //System.Diagnostics.Trace.WriteLine("???:" + velocity);

            base.Update(gameTime);
        }


        /// <summary>
        /// returns whether or not a fish is sick 
        /// 
        /// </summary>
        /// <returns> returns true if fish is sick</returns>
        public Boolean isSick()
        {
            return isPolluted;
        }


        public override void  Draw(GameTime gameTime)
        {
            if(!isDead){

                

                for (int i = 0; i < bobbers.Count; i++)
                {
                    //float variation = FISHSIZEVARIATION * (float)rnd.NextDouble() - FISHSIZEVARIATION/2; // FISHSIZEVARIATION * variation
                    //((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + (int)(200 * rnd.NextDouble()), (int)position.Y + (int)(150 * rnd.NextDouble()), (int)FISH_LENGTH, (int)(FISH_LENGTH * ((float)sprite.Height / ((float)sprite.Width / FRAMES)))), source, drawColor);       
                    ((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X + (FISHDISTANCEX * (i % 3)), (int)position.Y + (int)bobbers[i].getOff() + (FISHDISTANCEY * (i / 3)), (int)(FISH_LENGTH + bobbers[i].getAddSize()), (int)((FISH_LENGTH + bobbers[i].getAddSize()) * ((float)sprite.Height / ((float)sprite.Width / FRAMES)))), source, drawColor);
                    System.Diagnostics.Trace.WriteLine("this :" + i + " : " + bobbers[i].getAddSize());
                }

                //((PodPanic)(this.Game)).spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, (int)FISH_LENGTH, (int)(FISH_LENGTH * ((float)sprite.Height / ((float)sprite.Width/FRAMES)))), source, drawColor);
            

            }
        }
        


    }
}
