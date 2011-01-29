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
        

        
        /// <summary>
        /// create a fish that moves from right to left
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="normFish">image for normal fish</param>
        /// <param name="sickFish">image for sick fish</param>
        public Fish(int x, int y, Texture2D normFish, Texture2D sickFish)
            : base(null)
        {
            velocity = .25f;
            position.X = x;
            position.Y = y;
            Random rnd = new Random();

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
        /// moves fish along the x at a velocity
        /// </summary>
        //public void move()
        //{
         //   position.X += velocity;
            
            //System.Diagnostics.Trace.WriteLine(position.X);
            //System.Diagnostics.Trace.WriteLine(isPolluted);
            //System.Diagnostics.Trace.WriteLine(polluted_percent);
        //}


        /// <summary>
        /// update moves the fish forward
        /// </summary>
        /// <param name="gameTime">game time object</param>
        public void Update(GameTime gameTime)
        {
            


            position.X += velocity;
            

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
