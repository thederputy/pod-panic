using System;
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
        int scoreNum;
        String str;
        private const int SPEED = 3;

        Boolean isVisible;
        Boolean active;


        public Score(Vector2 pos, SpriteFont font)   
        {
            position = pos;
            scoreFont = font;
            scoreNum = 0;
            
            
        }

        public void Update()
        {
            if(active)
                scoreNum++;
            str = "Score : " + scoreNum / SPEED;

        }

        public void draw(SpriteBatch spriteBatch)
        {
            if(isVisible)
                spriteBatch.DrawString(scoreFont,str,position,Color.White);

        }

        public void Add(int x)
        {
            scoreNum += x * SPEED;
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
            active = true;
        }
        public void Stop()
        {
            active = false;
        }





    }

    

}
