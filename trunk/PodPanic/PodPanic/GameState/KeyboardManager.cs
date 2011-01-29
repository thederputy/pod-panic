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
    public class KeyboardManager : Microsoft.Xna.Framework.GameComponent
    {
        private KeyboardState prevState;
        private KeyboardState curState;

        public KeyboardManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            prevState = Keyboard.GetState();
            curState = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            prevState = curState;
            curState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public bool KeyPressed(Keys Key)
        {
            if (prevState.IsKeyUp(Key) && curState.IsKeyDown(Key))
                return true;
            return false;
        }
    }
}