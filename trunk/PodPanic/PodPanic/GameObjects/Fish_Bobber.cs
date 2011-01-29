using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodPanic.GameObjects
{
    class Fish_Bobber
    {
        private float off;
    
        private float dir;
        private float bRate;
        private int bAmount;
        private float sizeAdd;
        private Random rnd;

        public Fish_Bobber(float offset, float direction, float rate, int amount, float sizeVar)
        {
            off = offset;
            dir = direction;
            rnd = new Random();
            bRate = rate;
            bAmount = amount;
            sizeAdd = sizeVar * (float)rnd.NextDouble() - sizeVar / 2; 
        }


        public void Update(){
            off += dir * bRate;
            if ((dir > 0) && (off > 1))
            {
                off = 1;
                dir = -1;
            }
            else if ((dir < 0) && (off < -1))
            {
                off = -1;
                dir = 1;
            }
        
        }

        public float getOff()
        {
            return bAmount * off;
        }

        public float getAddSize()
        {
            return sizeAdd;
        }


    }
}
