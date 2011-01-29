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
        public int AlphaVal { get; set; }
        private bool isUp;

        public AlphaShader()
        {
            AlphaVal = 255;
        }
        public void Update(GameTime curTime)
        {
            if (curTime.ElapsedRealTime.Milliseconds >= 1)
            {
                if (isUp)
                    AlphaVal += 1;
                else
                    AlphaVal -= 1;
                if (AlphaVal <= 0)
                {
                    isUp = true;
                    AlphaVal = 0;
                }
                else if (AlphaVal >= 255)
                {
                    isUp = false;
                    AlphaVal = 255;
                }
                AlphaVal = Math.Max(Math.Min(AlphaVal, 255), 0);
            }
        }
    }
    class AlphaBlinker
    {
        public int AlphaVal { get; set; }
        int milliCovered;
        public AlphaBlinker()
        {
            AlphaVal = 255;
            milliCovered = 0;
        }
        public void Update(GameTime curTime)
        {
            milliCovered += curTime.ElapsedRealTime.Milliseconds;
            if (milliCovered >= 500)
            {
                if (AlphaVal == 0)
                    AlphaVal = 255;
                else
                    AlphaVal = 0;
                milliCovered = 0;
            }
        }
    }
}
