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
                return KeyPressed(KeyMapping.CurrentKeyMap.ActionKey) || ButtonPressed(ButtonMapping.CurrentButtonMap.ActionKey);
            else if (Command == KeyMapEnum.ExitKey)
                return KeyPressed(KeyMapping.CurrentKeyMap.ExitKey) || ButtonPressed(ButtonMapping.CurrentButtonMap.ExitKey);
            else if (Command == KeyMapEnum.MoveDown)
                return KeyPressed(KeyMapping.CurrentKeyMap.MoveDown) || ButtonPressed(ButtonMapping.CurrentButtonMap.MoveDown);
            else if (Command == KeyMapEnum.MoveUp)
                return KeyPressed(KeyMapping.CurrentKeyMap.MoveUp) || ButtonPressed(ButtonMapping.CurrentButtonMap.MoveUp);
            else if (Command == KeyMapEnum.MoveRight)
                return KeyPressed(KeyMapping.CurrentKeyMap.MoveRight) || ButtonPressed(ButtonMapping.CurrentButtonMap.MoveRight);
            else
                return false;
        }
        public bool isCommandDown(KeyMapEnum Command)
        {
            if (Command == KeyMapEnum.ActionKey)
                return isKeyDown(KeyMapping.CurrentKeyMap.ActionKey) || isButtonDown(ButtonMapping.CurrentButtonMap.ActionKey);
            else if (Command == KeyMapEnum.ExitKey)
                return isKeyDown(KeyMapping.CurrentKeyMap.ExitKey) || isButtonDown(ButtonMapping.CurrentButtonMap.ExitKey);
            else if (Command == KeyMapEnum.MoveDown)
                return isKeyDown(KeyMapping.CurrentKeyMap.MoveDown) || isButtonDown(ButtonMapping.CurrentButtonMap.MoveDown);
            else if (Command == KeyMapEnum.MoveUp)
                return isKeyDown(KeyMapping.CurrentKeyMap.MoveUp) || isButtonDown(ButtonMapping.CurrentButtonMap.MoveUp);
            else if (Command == KeyMapEnum.MoveRight)
                return isKeyDown(KeyMapping.CurrentKeyMap.MoveRight) || isButtonDown(ButtonMapping.CurrentButtonMap.MoveRight);
            else
                return false;
        }
    }
}