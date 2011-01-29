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

namespace PodPanic
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PodPanic : Microsoft.Xna.Framework.Game
    {
        GameState.KeyboardManager keyManager;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; set; }
        GameState.GameStateEnum curState;
        SpriteFont devFont;
        SpriteFont PausedFont;
        GameObjects.Player thePlayer;
        GameState.AlphaShader AlphaShader;
        GameState.AlphaBlinker AlphaBlinker;
        GameObjects.Background backTemp;
        Texture2D BadGuy1;
        Texture2D BadGuy2;
        List<GameObjects.GameObject> Objects;
        bool doOnce;
        GameObjects.Menu mainMenu;


        public PodPanic()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.ApplyChanges();
            Window.Title = "Pod-Panic";
            keyManager = new global::PodPanic.GameState.KeyboardManager(this);
            keyManager.Initialize();
            curState = global::PodPanic.GameState.GameStateEnum.Loading;
            AlphaShader = new global::PodPanic.GameState.AlphaShader();
            backTemp = new global::PodPanic.GameObjects.Background(this);
            Objects = new List<global::PodPanic.GameObjects.GameObject>();
            doOnce = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            thePlayer = new global::PodPanic.GameObjects.Player(this.Content.Load<Texture2D>("Orca/OrcaTEST"), this);
            backTemp.BackgroundTexture = this.Content.Load<Texture2D>("Background/Background_2");
            BadGuy1 = this.Content.Load<Texture2D>("Enemies/Net_Test");
            BadGuy2 = this.Content.Load<Texture2D>("Enemies/Oil_Test");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            devFont = this.Content.Load<SpriteFont>("DevFont");
            PausedFont = this.Content.Load<SpriteFont>("PausedFont");
            Texture2D test = this.Content.Load<Texture2D>("MenuItem");
            mainMenu = new global::PodPanic.GameObjects.Menu(0, 0, new List<string>(new String[] { "Start", "How To Play", "Exit" }), test, test, devFont);
            //Loading Logic - Graphics
            //Loading Logic - Levels
            //Loading Logic - GameFormat
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyManager.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            if (curState == global::PodPanic.GameState.GameStateEnum.Loading)
            {
                //Temporary Loading Simulator
                if (keyManager.KeyPressed(Keys.Space))
                    curState = global::PodPanic.GameState.GameStateEnum.Menu;

                //Check Load State - if loaded:
                // -- Move to Menu State
                //if not loaded:
                //Continue loading - bypass
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                if (keyManager.KeyPressed(Keys.Space))
                    curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                if (keyManager.KeyPressed(Keys.W))
                    mainMenu.moveUp();
                if (keyManager.KeyPressed(Keys.S))
                    mainMenu.moveDown();
                //Check player cursor position
                //Highlight menu options
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                //Do Key Detection
                if (keyManager.KeyPressed(Keys.Space))
                    curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                if (keyManager.KeyPressed(Keys.W))
                    thePlayer.moveUp();
                else if (keyManager.KeyPressed(Keys.S))
                    thePlayer.modeDown();
                backTemp.Update(gameTime);
                //Update Enemy Position
                if (!doOnce)
                {
                    Objects.Add(new global::PodPanic.GameObjects.Enemy(800, getYChannel(global::PodPanic.GameState.Channel.Top), BadGuy1, 1, this));
                    doOnce = true;
                }
                if (doOnce && Objects.Count < 2)
                {
                    Objects.Add(new global::PodPanic.GameObjects.Enemy(800, getYChannel(global::PodPanic.GameState.Channel.Middle), BadGuy2, 1, this));
                }
                foreach (GameObjects.GameObject obj in Objects)
                {
                    obj.Update(gameTime);
                }
                //Update Player Position
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                if (keyManager.KeyPressed(Keys.Space))
                    curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                //Update text of pause state
            }
            AlphaShader.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (curState == global::PodPanic.GameState.GameStateEnum.Loading)
            {
                spriteBatch.DrawString(devFont, "Loading", new Vector2(0, 0), Color.White);
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                spriteBatch.DrawString(devFont, "Menu State", new Vector2(0, 0), Color.White);
                mainMenu.draw(spriteBatch);
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                helpDraw(gameTime);
                spriteBatch.DrawString(devFont, "Running", new Vector2(0, 0), Color.White);
                //Draw background
                //Draw player
                
                //Draw enemies
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                helpDraw(gameTime);
                spriteBatch.DrawString(PausedFont, "PAUSED", new Vector2(400, 300) - new Vector2(PausedFont.MeasureString("PAUSED").X*0.5f,PausedFont.MeasureString("PAUSED").Y*0.5f) , new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });
                //Update text of pause state
                
            }
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        private void helpDraw(GameTime gameTime)
        {
            Color drawColor = Color.White;
            if (curState == global::PodPanic.GameState.GameStateEnum.GamePause) drawColor = Color.Gray;
            backTemp.drawColor = drawColor;
            backTemp.Draw(gameTime);
            spriteBatch.Draw(thePlayer.getTexture(), new Rectangle((int)thePlayer.getPosition().X + 99, getYChannel(thePlayer.currChannel) + 33, 111, 83), drawColor);
            spriteBatch.Draw(thePlayer.getTexture(), new Rectangle((int)thePlayer.getPosition().X, getYChannel(thePlayer.currChannel) + 38, 99, 75), drawColor);
            spriteBatch.Draw(thePlayer.getTexture(), new Rectangle((int)thePlayer.getPosition().X + 50, getYChannel(thePlayer.currChannel), 99, 75), drawColor);
            spriteBatch.Draw(thePlayer.getTexture(), new Rectangle((int)thePlayer.getPosition().X + 50, getYChannel(thePlayer.currChannel) + 86, 99, 75), drawColor);
            foreach (GameObjects.GameObject obj in Objects)
            {
                obj.Draw(gameTime);
            }
        }

        private int getYChannel(global::PodPanic.GameState.Channel arg0)
        {
            switch (arg0)
            {
                case global::PodPanic.GameState.Channel.Top: return 150;
                case global::PodPanic.GameState.Channel.Middle: return 300;
                case global::PodPanic.GameState.Channel.Bottom: return 450;
            }
            return 0;
        }
    }
}
