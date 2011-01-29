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

        private const int OFFY_AMOUNT = 10;
        private const float SLOWEST_SPEED = 1.25f;
        private const float FASTEST_SPEED = 3.25f;
        private const float BOB_RATE = 0.01f;

        
        

        
        /// <summary>
        /// create a fish that moves from right to left
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="normFish">image for normal fish</param>
        /// <param name="sickFish">image for sick fish</param>
        public Fish(int x, int y, Texture2D normFish, Texture2D sickFish, Game game)
            : base(null,game)
        {
            
            position.X = x;
            position.Y = y;
            Random rnd = new Random();
            baseY = y;
            velocity = SLOWEST_SPEED + (FASTEST_SPEED - SLOWEST_SPEED * (float)rnd.NextDouble());

            System.Diagnostics.Trace.WriteLine("");

            offY = (float)rnd.NextDouble();

            if (rnd.NextDouble() > 0.5f)
                dir = 1;
            else
                dir = -1;

            

            if (rnd.NextDouble() <= polluted_percent)
            {
                isPolluted = true;
                sprite = sickFish;
            }
            else
            {
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

            position.X -= velocity;
            position.Y = baseY + (OFFY_AMOUNT * offY);

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



        


    }
}
