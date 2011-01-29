using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PodPanic.GameObjects
{
    class Enemy : GameObject
    {
        private int damage;
        private float offY;
        private int offY_Amount;
        private float dir;
        private int baseY;


        /// <summary>
        /// creates an enemy at x,y that looks like the texture and moves with a velocity from left to right.
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="loadedTexture">the texture it draws</param>
        /// <param name="dam">how much damage it does</param>
        /// 
        public Enemy(int x, int y, Texture2D loadedTexture, int dam, Game game)
            : base(loadedTexture,game)
        {
            velocity = .5f;
            position.X = x;
            position.Y = y;
            baseY = y;
            offY = 0;
            damage = dam;
            dir = 1;
            offY_Amount = 10;
        }

        


        
        /// <summary>
        /// update moves the position forward
        /// </summary>
        /// <param name="gameTime">the game time object</param>
        public void Update(GameTime gameTime)
        {
            //offY += gameTime.ElapsedGameTime.Milliseconds;
            offY += dir * 0.01f;
            if ((dir > 0)&&(offY > 1)){
                offY = 1;
                dir = -1;
            }
            else if ((dir < 0) && (offY < -1))
            {
                offY = -1;
                dir = 1;
            }




            position.X += velocity;
            position.Y = baseY + (offY_Amount * offY);

            //System.Diagnostics.Trace.WriteLine(position.X);
            //System.Diagnostics.Trace.WriteLine(position.Y);
            //System.Diagnostics.Trace.WriteLine(gameTime.ElapsedGameTime.Milliseconds);
            //System.Diagnostics.Trace.WriteLine("******");

            base.Update(gameTime);
        }

        /// <summary>
        /// returns an int that represents the damage caused by the enemy
        /// </summary>
        /// <returns>returns an int damage</returns>
        public int getDamage()
        {
            return damage;
        }

    }
}
