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


        /// <summary>
        /// creates an enemy at x,y that looks like the texture and moves with a velocity from left to right.
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="loadedTexture">the texture it draws</param>
        /// <param name="dam">how much damage it does</param>
        Enemy(int x, int y, Texture2D loadedTexture, int dam)
            : base(loadedTexture)
        {
            velocity = .5f;
            position.X = x;
            position.Y = y;
            damage = dam;
            
        }

        public void move()
        {
            position.X += velocity;
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
