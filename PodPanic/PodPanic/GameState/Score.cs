﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PodPanic.GameObjects
{
    class Score
    {
        Vector2 position;
        SpriteFont scoreFont;
        int levelScore;
        int totalScore;
        String str;
        private const int SPEED = 3;

        public Boolean isVisible;
        public Boolean isActive;
        public Boolean hitDuringLevel;
        public Boolean updatedEndOfLevel;


        public Score(Vector2 pos, SpriteFont font)
        {
            position = pos;
            scoreFont = font;
            levelScore = 0;
            totalScore = 0;
            hitDuringLevel = false;
            updatedEndOfLevel = false;
        }

        public void Update()
        {
            str = "Score: " + totalScore + levelScore;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.DrawString(scoreFont, str, position, Color.White);
        }

        /// <summary>
        /// Reset the score for the new level
        /// </summary>
        public void resetScore()
        {
            levelScore = 0;
            totalScore = 0;
            isActive = false;
            isVisible = false;
            hitDuringLevel = false;
            updatedEndOfLevel = false;
        }

        /// <summary>
        /// Modifies a score by a given change value
        /// </summary>
        /// <param name="changeValue">the amount to change the score by.
        /// +100 for fish, -20 for poisoned fish, -30 for oil slick, -50 for net.</param>
        public void modify(int changeValue)
        {
            levelScore += changeValue;
        }

        public void beginLevel()
        {
            levelScore = 0;
        }

        /// <summary>
        /// Ends the score keeping for the level.
        /// </summary>
        /// <param name="thePlayer">the player of the game, to get the lives and stuff from.</param>
        public void endLevel(Player thePlayer)
        {
            totalScore += levelScore;
        }

        public void Hide()
        {
            isVisible = false;
        }

        public void Show()
        {
            isVisible = true;
        }

        public void Start()
        {
            isActive = true;
        }
        public void Stop()
        {
            isActive = false;
        }
    }
}