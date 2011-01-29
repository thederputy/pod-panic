using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PodPanic.GameObjects
{
    
    class Menu
    {
        private List<String> menuList;
        
        private int pointer;
        private Vector2 firstItem;
        private int distance;

        private Texture2D logo;
        private Texture2D item;
        private SpriteFont menuFont;

        private const int ADDITIONAL_SPACE_Y = 30;


        public Menu(int x, int y,List<String> mList,Texture2D menuItem, Texture2D logoImage,SpriteFont font)
        {
            menuList = mList;
            pointer = 0;

            firstItem.X = x;
            firstItem.Y = y;

            distance = menuItem.Height + ADDITIONAL_SPACE_Y;

            logo = logoImage;
            item = menuItem;

            menuFont = font;
        }


        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(logo,firstItem,Color.White);
            Color color = Color.White;
            for(int i = 0; i < menuList.Count; i++){

                if(i == pointer)
                    color = Color.Gold;
                spriteBatch.Draw(item, new Vector2(firstItem.X + logo.Width/2 - item.Width/2, firstItem.Y + logo.Height + item.Height + i * distance), color);
                color = Color.White;
                Vector2 centerOffset = menuFont.MeasureString(menuList[i]);
                centerOffset = new Vector2(centerOffset.X * 0.5f, centerOffset.Y * 0.5f);
                spriteBatch.DrawString(menuFont, menuList[i], new Vector2(firstItem.X + logo.Width / 2 - centerOffset.X, firstItem.Y + logo.Height + item.Height + i * distance + centerOffset.Y), color);
            }
            // 

            // x = x + logowidth/2 - itemwidth/2 (+5)
            // y = y + logoheight + itemheight, + i * distance (+5)
        }
        public void moveUp()
        {
            if(pointer > 0){
                pointer--;
            }
        }
        public void moveDown()
        {
            if(pointer < menuList.Count -1){
                pointer++;
            }
        }
        public String getItem(){
            return menuList[pointer];
        }
    }
}
