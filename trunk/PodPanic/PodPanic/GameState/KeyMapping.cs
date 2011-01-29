#region Using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace PodPanic.GameState
{
    class KeyMapping
    {
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveRight { get; set; }
        public Keys ActionKey { get; set; }
        public Keys ExitKey { get; set; }

        public static KeyMapping GetDefaultKeyMap()
        {
            return new KeyMapping() { MoveUp = Keys.W, MoveDown = Keys.S, MoveRight = Keys.D, ActionKey = Keys.Space, ExitKey = Keys.Escape };
        }
    }
    class ButtonMapping
    {
        public Buttons MoveUp { get; set; }
        public Buttons MoveDown { get; set; }
        public Buttons MoveRight { get; set; }
        public Buttons ActionKey { get; set; }
        public Buttons ExitKey { get; set; }

        public static ButtonMapping GetDefaultButtonMap()
        {
            return new ButtonMapping() { MoveUp = Buttons.LeftThumbstickUp, MoveDown = Buttons.LeftThumbstickDown, MoveRight = Buttons.LeftThumbstickRight, ActionKey = Buttons.A, ExitKey = Buttons.Back };
        }
    }
}
