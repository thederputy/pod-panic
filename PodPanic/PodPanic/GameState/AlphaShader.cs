using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace PodPanic.GameState
{
    class AlphaShader
    {
        public const int MAX_ALPHA = 255;
        public const int MIN_ALPHA = 0;
        public const int NUM_TICKS = 2;
        public int AlphaVal { get; set; }
        private bool isUp;
        int milliCovered;

        public AlphaShader()
        {
            AlphaVal = 255;
            milliCovered = 0;
        }
        public void Update(GameTime curTime)
        {
            milliCovered += (int)curTime.ElapsedRealTime.Ticks;
            if (milliCovered >= NUM_TICKS)
            {
                milliCovered = 0;
                if (isUp)
                    AlphaVal += 1;
                else
                    AlphaVal -= 1;
                if (AlphaVal <= MIN_ALPHA)
                {
                    isUp = true;
                    AlphaVal = MIN_ALPHA;
                }
                else if (AlphaVal >= MAX_ALPHA)
                {
                    isUp = false;
                    AlphaVal = MAX_ALPHA;
                }
                AlphaVal = Math.Max(Math.Min(AlphaVal, MAX_ALPHA), MIN_ALPHA);
            }
        }
    }
    class AlphaBlinker
    {
        public const int MAX_ALPHA = 255;
        public const int MIN_ALPHA = 0;
        public const int NUM_MILLISECONDS = 500;
        public int AlphaVal { get; set; }
        int milliCovered;
        public AlphaBlinker()
        {
            AlphaVal = MAX_ALPHA;
            milliCovered = 0;
        }
        public void Update(GameTime curTime)
        {
            milliCovered += curTime.ElapsedRealTime.Milliseconds;
            if (milliCovered >= NUM_MILLISECONDS)
            {
                if (AlphaVal == MIN_ALPHA)
                    AlphaVal = MAX_ALPHA;
                else
                    AlphaVal = MIN_ALPHA;
                milliCovered = 0;
            }
        }
    }
}
