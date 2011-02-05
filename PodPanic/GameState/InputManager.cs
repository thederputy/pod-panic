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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {
        private KeyboardState prevKeyState;
        private KeyboardState curKeyState;
        private GamePadState prevPadState;
        private GamePadState curPadState;

        public InputManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            curKeyState = prevKeyState = Keyboard.GetState();
            curPadState = prevPadState = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            prevKeyState = curKeyState;
            curKeyState = Keyboard.GetState();
            prevPadState = curPadState;
            curPadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }

        public bool KeyPressed(Keys Key)
        {
            if (prevKeyState.IsKeyUp(Key) && curKeyState.IsKeyDown(Key))
                return true;
            return false;
        }
        public bool ButtonPressed(Buttons Button)
        {
            if (prevPadState.IsButtonUp(Button) && curPadState.IsButtonDown(Button))
                return true;
            return false;
        }
        public bool isKeyDown(Keys Key)
        {
            if (curKeyState.IsKeyDown(Key))
                return true;
            return false;
        }
        public bool isButtonDown(Buttons Button)
        {
            if (curPadState.IsButtonDown(Button))
                return true;
            return false;
        }
        public bool isCommandPressed(KeyMapEnum Command)
        {
            if (Command == KeyMapEnum.ActionKey)
            {
                bool key = KeyPressed(KeyMapping.Arrows.ActionKey) || KeyPressed(KeyMapping.WASD.ActionKey);
                bool button = ButtonPressed(ButtonMapping.ControlStick.ActionKey) || ButtonPressed(ButtonMapping.DPad.ActionKey);
                return key || button;
            }
            else if (Command == KeyMapEnum.ExitKey)
            {
                return KeyPressed(KeyMapping.CurrentKeyMap.ExitKey) || ButtonPressed(ButtonMapping.CurrentButtonMap.ExitKey);
            }
            else if (Command == KeyMapEnum.MoveDown)
            {
                bool key = KeyPressed(KeyMapping.Arrows.MoveDown) || KeyPressed(KeyMapping.WASD.MoveDown);
                bool button = ButtonPressed(ButtonMapping.ControlStick.MoveDown) || ButtonPressed(ButtonMapping.DPad.MoveDown);
                return key || button;
            }
            else if (Command == KeyMapEnum.MoveUp)
            {
                bool key = KeyPressed(KeyMapping.Arrows.MoveUp) || KeyPressed(KeyMapping.WASD.MoveUp);
                bool button = ButtonPressed(ButtonMapping.ControlStick.MoveUp) || ButtonPressed(ButtonMapping.DPad.MoveUp);
                return key || button;
            }
            else if (Command == KeyMapEnum.MoveRight)
            {
                bool key = KeyPressed(KeyMapping.Arrows.MoveRight) || KeyPressed(KeyMapping.WASD.MoveRight);
                bool button = ButtonPressed(ButtonMapping.ControlStick.MoveRight) || ButtonPressed(ButtonMapping.DPad.MoveRight);
                return key || button;
            }
            else
                return false;
        }
        public bool isCommandDown(KeyMapEnum Command)
        {
            if (Command == KeyMapEnum.ActionKey)
            {
                bool key = isKeyDown(KeyMapping.Arrows.ActionKey) || isKeyDown(KeyMapping.WASD.ActionKey);
                bool button = isButtonDown(ButtonMapping.ControlStick.ActionKey) || isButtonDown(ButtonMapping.DPad.ActionKey);
                return key || button;
            }
            else if (Command == KeyMapEnum.ExitKey)
            {
                return isKeyDown(KeyMapping.CurrentKeyMap.ExitKey) || isButtonDown(ButtonMapping.CurrentButtonMap.ExitKey);
            }
            else if (Command == KeyMapEnum.MoveDown)
            {
                bool key = isKeyDown(KeyMapping.Arrows.MoveDown) || isKeyDown(KeyMapping.WASD.MoveDown);
                bool button = isButtonDown(ButtonMapping.ControlStick.MoveDown) || isButtonDown(ButtonMapping.DPad.MoveDown);
                return key || button;
            }
            else if (Command == KeyMapEnum.MoveUp)
            {
                bool key = isKeyDown(KeyMapping.Arrows.MoveUp) || isKeyDown(KeyMapping.WASD.MoveUp);
                bool button = isButtonDown(ButtonMapping.ControlStick.MoveUp) || isButtonDown(ButtonMapping.DPad.MoveUp);
                return key || button;
            }
            else if (Command == KeyMapEnum.MoveRight)
            {
                bool key = isKeyDown(KeyMapping.Arrows.MoveRight) || isKeyDown(KeyMapping.WASD.MoveRight);
                bool button = isButtonDown(ButtonMapping.ControlStick.MoveRight) || isButtonDown(ButtonMapping.DPad.MoveRight);
                return key || button;
            }
            else
                return false;
        }
    }
}