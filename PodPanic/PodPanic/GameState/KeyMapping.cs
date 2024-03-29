﻿#region Using statements
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
        public static KeyMapping CurrentKeyMap { get; set; }
        public static KeyMapping Arrows { get; set; }
        public static KeyMapping WASD { get; set; }
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveRight { get; set; }
        public Keys ActionKey { get; set; }
        public Keys ExitKey { get; set; }

        public static KeyMapping GetDefaultKeyMap()
        {
            initializeArrows();
            initializeWASD();
            return Arrows;
        }

        public static void setKeyMappingAsArrows()
        {
            CurrentKeyMap = new KeyMapping() { MoveUp = Keys.Up, MoveDown = Keys.Down, MoveRight = Keys.Right, ActionKey = Keys.Enter, ExitKey = Keys.Escape };
        }

        public static void setKeyMappingAsWASD()
        {
            CurrentKeyMap = new KeyMapping() { MoveUp = Keys.W, MoveDown = Keys.S, MoveRight = Keys.D, ActionKey = Keys.Space, ExitKey = Keys.Escape };
        }

        private static void initializeArrows()
        {
            Arrows = new KeyMapping() { MoveUp = Keys.Up, MoveDown = Keys.Down, MoveRight = Keys.Right, ActionKey = Keys.Enter, ExitKey = Keys.Escape };
        }

        private static void initializeWASD()
        {
            WASD = new KeyMapping() { MoveUp = Keys.W, MoveDown = Keys.S, MoveRight = Keys.D, ActionKey = Keys.Space, ExitKey = Keys.Escape };
        }
    }

    class ButtonMapping
    {
        public static ButtonMapping CurrentButtonMap { get; set; }
        public static ButtonMapping ControlStick { get; set; }
        public static ButtonMapping DPad { get; set; }
        public Buttons MoveUp { get; set; }
        public Buttons MoveDown { get; set; }
        public Buttons MoveRight { get; set; }
        public Buttons ActionKey { get; set; }
        public Buttons ExitKey { get; set; }

        public static ButtonMapping GetDefaultButtonMap()
        {
            initializeControlStick();
            initializeDPad();
            return ControlStick;
        }

        public static void setButtonMappingAsControlStick()
        {
            CurrentButtonMap = new ButtonMapping() { MoveUp = Buttons.LeftThumbstickUp, MoveDown = Buttons.LeftThumbstickDown, MoveRight = Buttons.LeftThumbstickRight, ActionKey = Buttons.A, ExitKey = Buttons.Start };
        }

        public static void setButtonMappingAsDPad()
        {
            CurrentButtonMap = new ButtonMapping() { MoveUp = Buttons.DPadUp, MoveDown = Buttons.DPadDown, MoveRight = Buttons.DPadRight, ActionKey = Buttons.A, ExitKey = Buttons.Start };
        }

        private static void initializeControlStick()
        {
            ControlStick = new ButtonMapping() { MoveUp = Buttons.LeftThumbstickUp, MoveDown = Buttons.LeftThumbstickDown, MoveRight = Buttons.LeftThumbstickRight, ActionKey = Buttons.A, ExitKey = Buttons.Start };
        }

        private static void initializeDPad()
        {
            DPad = new ButtonMapping() { MoveUp = Buttons.DPadUp, MoveDown = Buttons.DPadDown, MoveRight = Buttons.DPadRight, ActionKey = Buttons.A, ExitKey = Buttons.Start };
        }
    }

    public enum KeyMapEnum
    {
        MoveUp, MoveDown, MoveRight, ActionKey, ExitKey
    }
}
