using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodPanic.GameObjects
{
    
    class Menu
    {
        private List<String> menuList;
        private int pointer;


        public Menu(List<String> mList)
        {
            menuList = mList;
            pointer = 0;
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
