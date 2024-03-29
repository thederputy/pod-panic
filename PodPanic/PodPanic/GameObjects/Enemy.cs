﻿using System;
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
        private float dir;
        private int baseY;

        private const int OFFY_AMOUNT = 10;
        private const float SLOWEST_SPEED = 2.2f;
        private const float FASTEST_SPEED = 4.4f;
        private const float BOB_RATE = 0.01f;

        public GameState.EnemyType type;

        /// <summary>
        /// creates an enemy at x,y that looks like the texture and moves with a velocity from left to right.
        /// </summary>
        /// <param name="x">starting x</param>
        /// <param name="y">starting y</param>
        /// <param name="loadedTexture">the texture it draws</param>
        /// <param name="dam">how much damage it does</param>
        /// <param name="game">the game</param>
        /// <param name="type">the enemy type, net or oil barrel</param>
        public Enemy(int x, int y, Texture2D loadedTexture, int dam, Game game, GameState.EnemyType enemyType)
            : base(loadedTexture, game)
        {
            Random rnd = new Random();
            type = enemyType;

            velocity = SLOWEST_SPEED + (FASTEST_SPEED - SLOWEST_SPEED * (float)rnd.NextDouble());

            //System.Diagnostics.Trace.WriteLine("");

            offY = (float)rnd.NextDouble();

            if (rnd.NextDouble() > 0.5f)
                dir = 1;
            else
                dir = -1;

            position.X = x;
            position.Y = y;
            baseY = y;
            
            damage = dam;

            hitBoxOffsetX = 5;
            hitBoxOffsetY = 5;
        }
        
        /// <summary>
        /// update moves the position forward
        /// </summary>
        /// <param name="gameTime">the game time object</param>
        public override void Update(GameTime gameTime)
        {
            //offY += gameTime.ElapsedGameTime.Milliseconds;
            offY += dir * BOB_RATE;
            if ((dir > 0)&&(offY > 1)){
                offY = 1;
                dir = -1;
            }
            else if ((dir < 0) && (offY < -1))
            {
                offY = -1;
                dir = 1;
            }
            if (hasHitPlayer)
            {
                if (!isDead)
                {
                    timeOfDeath = timeAlive;
                }
                isDead = true;
            }
            if (!isDead)
            {
                position.X -= velocity;
                position.Y = baseY + (OFFY_AMOUNT * offY);
            }
            //else // the enemy is dead
            //{
            //    if (timeOfDeath > 750)
            //    {
            //        this.signalRemoval = true;
            //    }
            //}
            blinker.Update(gameTime);
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
