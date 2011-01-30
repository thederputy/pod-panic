using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PodPanic.Audio;

namespace PodPanic
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PodPanic : Microsoft.Xna.Framework.Game
    {
        public Vector2 SCREEN_SIZE = new Vector2(800, 600);
        public const int TOPCHANNEL_Y = 150;
        public const int MIDCHANNEL_Y = 300;
        public const int BOTCHANNEL_Y = 450;
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
        Texture2D Fish;
        List<GameObjects.GameObject> Objects;
        bool DevMode;
        GameObjects.Menu mainMenu;
        Texture2D fishTest;
        Random rand;
        GameObjects.Score score;
        LevelObjects.LevelLogic[] Levels;
        int CurrentLevel;
        GameState.LevelProgress lvlProgress;
        int secondsSinceStart;
        int secondsSinceLastEvent;
        int distanceCovered;

        #region Sound Effects
        SoundEffect ambientWavesEngine;
        SoundEffect barrelHitEngine;
        SoundEffect chompEngine;
        SoundEffect entrySplashEngine;
        SoundEffect finSplashEngine;
        SoundEffect gameStartEngine;

        SoundEffectInstance ambientWavesInstance;
        SoundEffectInstance barrelHitInstance;
        SoundEffectInstance chompInstance;
        SoundEffectInstance entrySplashInstance;
        SoundEffectInstance finSplashInstance;
        SoundEffectInstance gameStartInstance;
        #endregion

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
            this.graphics.PreferredBackBufferHeight = (int)SCREEN_SIZE.Y;
            this.graphics.PreferredBackBufferWidth = (int)SCREEN_SIZE.X;
            //this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();
            Window.Title = "Pod-Panic";
            keyManager = new global::PodPanic.GameState.KeyboardManager(this);
            keyManager.Initialize();
            curState = global::PodPanic.GameState.GameStateEnum.Loading;
            AlphaShader = new global::PodPanic.GameState.AlphaShader();
            AlphaBlinker = new global::PodPanic.GameState.AlphaBlinker();
            backTemp = new global::PodPanic.GameObjects.Background(this);
            Objects = new List<global::PodPanic.GameObjects.GameObject>();
            //doOnce = false;
            lvlProgress = global::PodPanic.GameState.LevelProgress.StartingLevel;
            CurrentLevel = 0;
            GameObjects.Player.Speed = 1;
            secondsSinceStart = 0;
            secondsSinceLastEvent = 0;
            rand = new Random();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            thePlayer = new global::PodPanic.GameObjects.Player(this.Content.Load<Texture2D>("Orca/OrcaFinal2"), this);
            backTemp.BackgroundTexture = this.Content.Load<Texture2D>("Background/Background_2");
            BadGuy1 = this.Content.Load<Texture2D>("Enemies/Net_Test");
            BadGuy2 = this.Content.Load<Texture2D>("Enemies/Oil_Test");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            devFont = this.Content.Load<SpriteFont>("DevFont");
            PausedFont = this.Content.Load<SpriteFont>("PausedFont");
            Texture2D test = this.Content.Load<Texture2D>("MenuItem");
            Texture2D logo = this.Content.Load<Texture2D>("LOGO");
            mainMenu = new global::PodPanic.GameObjects.Menu((int)(SCREEN_SIZE.X / 2 - logo.Width/2), (int)(SCREEN_SIZE.Y / 4 - logo.Height / 2), new List<string>(new String[] { "Start", "How To Play", "Exit" }), test, logo, devFont);
            Levels = new global::PodPanic.LevelObjects.LevelLogic[4];
            score = new GameObjects.Score(new Vector2(675,0),devFont);

            //List<Texture2D> fList = new List<Texture2D>();
            fishTest = this.Content.Load<Texture2D>("Food/Salmon_Sprite");


            //Loading Logic - Graphics
            //Loading Logic - Levels
            LevelObjects.LevelLogic level1 = new LevelObjects.LevelLogic();
            LevelObjects.LevelLogic level2 = new LevelObjects.LevelLogic();
            LevelObjects.LevelLogic level3 = new LevelObjects.LevelLogic();
            LevelObjects.LevelLogic level4 = new LevelObjects.LevelLogic();
            Levels[0] = level1;
            Levels[1] = level2;
            Levels[2] = level3;
            Levels[3] = level4;

            // to get to the levels
            XmlNode levelNode1 = GameState.LoadingManager.getXmlLevelNodeFromFile(GameState.LoadingManager.pathToLevels + "level1.xml");
            level1.setDataFromXml(levelNode1);
            XmlNode levelNode2 = GameState.LoadingManager.getXmlLevelNodeFromFile(GameState.LoadingManager.pathToLevels + "level2.xml");
            level2.setDataFromXml(levelNode2);
            XmlNode levelNode3 = GameState.LoadingManager.getXmlLevelNodeFromFile(GameState.LoadingManager.pathToLevels + "level3.xml");
            level3.setDataFromXml(levelNode3);
            XmlNode levelNode4 = GameState.LoadingManager.getXmlLevelNodeFromFile(GameState.LoadingManager.pathToLevels + "level4.xml");
            level4.setDataFromXml(levelNode4);
            
            //Loading Logic - GameFormat

            #region Sound effect loading
            ambientWavesEngine = Content.Load<SoundEffect>("Sounds/ambientWaves");
            ambientWavesInstance = ambientWavesEngine.CreateInstance();

            barrelHitEngine = Content.Load<SoundEffect>("Sounds/barrelHit");
            barrelHitInstance = barrelHitEngine.CreateInstance();

            chompEngine = Content.Load<SoundEffect>("Sounds/chomp");
            chompInstance = chompEngine.CreateInstance();

            entrySplashEngine = Content.Load<SoundEffect>("Sounds/entrySplash");
            entrySplashInstance = entrySplashEngine.CreateInstance();

            finSplashEngine = Content.Load<SoundEffect>("Sounds/finSplash");
            finSplashInstance = finSplashEngine.CreateInstance();

            gameStartEngine = Content.Load<SoundEffect>("Sounds/gameStart");
            gameStartInstance = gameStartEngine.CreateInstance();
            #endregion
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
                SoundManager.playSound(gameStartInstance, 0.2f);
                if (keyManager.KeyPressed(Keys.Space))
                {
                    char firstChar = mainMenu.getItem().ToCharArray()[0];
                    if (firstChar == 'S')
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                        score.Show();
                        score.Start();
                    }
                    else if (firstChar == 'H')
                    {
                        //How to logic
                    }
                    else if (firstChar == 'E')
                    {
                        this.Exit();
                    }
                }
                if (keyManager.KeyPressed(Keys.W))
                    mainMenu.moveUp();
                if (keyManager.KeyPressed(Keys.S))
                    mainMenu.moveDown();


                backTemp.Update(gameTime);
                //Check player cursor position
                //Highlight menu options
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                #region Sound Playing
                SoundManager.stopSound(gameStartInstance);
                SoundManager.startLoopedSound(ambientWavesInstance, 0.05f);
                #endregion
                //Do Key Detection
                
                if (lvlProgress == global::PodPanic.GameState.LevelProgress.StartingLevel)
                {
                    secondsSinceStart += (int)gameTime.ElapsedRealTime.Milliseconds;
                    //System.Diagnostics.Trace.WriteLine(gameTime.ElapsedRealTime.Milliseconds);
                    SoundManager.playSound(entrySplashInstance, 0.5f);
                    if (keyManager.KeyPressed(Keys.Space))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                        score.Stop();
                        SoundManager.pauseSound(ambientWavesInstance);
                    }
                    if (secondsSinceStart >= 1250) //**************************************here
                    {
                        secondsSinceStart = 0;
                        lvlProgress = global::PodPanic.GameState.LevelProgress.RunningLevel;
                    }
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.RunningLevel)
                {
                    //Event Spawning Logic
                    secondsSinceLastEvent += 1;
                    if (secondsSinceLastEvent >= Levels[CurrentLevel].TimeBetweenEvents + rand.Next(0, 100))
                    {
                        secondsSinceLastEvent = 0;
                        float chance = rand.Next(0, 20) / 20;
                        if (chance < Levels[CurrentLevel].ProbabilityEnemyFish)
                        {
                            //Enemy Spawn
                        }
                        else
                        {
                            //GameObjects.Fish  = new global::PodPanic.GameObjects.Fish(SCREEN_SIZE.X, getYChannel(global::PodPanic.GameState.Channel.Middle), Fish, Fish, this);
                            //Objects.Add(
                        }
                    }
                    distanceCovered += GameObjects.Player.Speed;
                    if (distanceCovered >= Levels[CurrentLevel].LevelLength)
                    {
                        lvlProgress = global::PodPanic.GameState.LevelProgress.FinishedLevel;
                        distanceCovered = 0;
                    }
                    if (keyManager.KeyPressed(Keys.Space))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                        score.Stop();
                        SoundManager.pauseSound(ambientWavesInstance);
                    }
                    if (keyManager.KeyPressed(Keys.W))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveUp();
                    }
                    else if (keyManager.KeyPressed(Keys.S))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.modeDown();
                    }
                    if (keyManager.isKeyDown(Keys.D))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveRight();
                    }
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel)
                {
                    SoundManager.pauseSound(ambientWavesInstance);


                    if (keyManager.KeyPressed(Keys.Space))
                    {
                        lvlProgress = global::PodPanic.GameState.LevelProgress.StartingLevel;
                        if (!(CurrentLevel + 1 >= Levels.Length))
                        {
                            CurrentLevel += 1;
                            GameObjects.Player.Speed += 1;
                        }
                        else ;
                            //Signal End Game
                    }
                }

                if (keyManager.KeyPressed(Keys.Q))
                    thePlayer.increaseHP(10);
                backTemp.Update(gameTime);
                //Update Enemy Position

                if (keyManager.KeyPressed(Keys.O))
                    score.Add(500);


                //if (!doOnce)
                //{
                //    Objects.Add(new global::PodPanic.GameObjects.Enemy((int)SCREEN_SIZE.X, getYChannel(global::PodPanic.GameState.Channel.Top), BadGuy1, 1, this));
                //    Objects.Add(new global::PodPanic.GameObjects.Fish(800, getYChannel(global::PodPanic.GameState.Channel.Middle), fishTest, fishTest, this));
                    
                //    doOnce = true;
                //}
                //if (doOnce && Objects.Count < 3)
                //{
                //    Objects.Add(new global::PodPanic.GameObjects.Enemy((int)SCREEN_SIZE.Y, getYChannel(global::PodPanic.GameState.Channel.Middle), BadGuy2, 1, this));
                //    //testing fish

                //    Objects.Add(new global::PodPanic.GameObjects.Fish(800, getYChannel(global::PodPanic.GameState.Channel.Bottom), fishTest, fishTest, this));
                //    //end testing fish
                //}
                foreach (GameObjects.GameObject obj in Objects)
                {
                    obj.Update(gameTime);
                    //Collision Detection
                    if(new Rectangle((int)obj.getPosition().X + 25, (int)obj.getPosition().Y + 25, 150, 100).Intersects(new Rectangle((int)thePlayer.getPosition().X, (int)getYChannel(thePlayer.currChannel), 200, 150)))
                    {
                        //has collided with object - friend or foe?
                        if(obj.GetType() == typeof(GameObjects.Enemy))
                        {
                            if (((GameObjects.Enemy)(obj)).hasHitPlayer == false)
                            {
                                thePlayer.reduceHP(((GameObjects.Enemy)(obj)).getDamage());
                                ((GameObjects.Enemy)(obj)).hasHitPlayer = true;
                            } 
                        }
                        else
                        {

                        }
                        if (obj.getPosition().X < -obj.getTexture().Width)
                            Objects.Remove(obj);
                        
                    }
                }
                //Update Player Position
                thePlayer.Update(gameTime);
                //Collision Detection
            }

            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                if (keyManager.KeyPressed(Keys.Space))
                {
                    curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                    score.Start();
                }

                if (keyManager.KeyPressed(Keys.Escape))
                    this.Exit();
                //Update text of pause state
            }

            if (keyManager.KeyPressed(Keys.Z)) //triggers DevMode
            {
                if (DevMode)
                    DevMode = false;
                else
                    DevMode = true;
            }

            AlphaShader.Update(gameTime);
            AlphaBlinker.Update(gameTime);

            score.Update();

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
                if (DevMode)
                    spriteBatch.DrawString(devFont, "Loading", Vector2.Zero, Color.White);
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                backTemp.Draw(gameTime);
                if (DevMode)
                    spriteBatch.DrawString(devFont, "Menu State", Vector2.Zero, Color.White);
                mainMenu.draw(spriteBatch);
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                helpDraw(gameTime);
                if (lvlProgress == global::PodPanic.GameState.LevelProgress.StartingLevel)
                {
                    string line0 = "Level:" + Levels[CurrentLevel].LevelNumber;
                    string line1 = "\"" + Levels[CurrentLevel].LevelName.ToLower() + "\"";
                    spriteBatch.DrawString(PausedFont, line0, new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line0).X / 2, SCREEN_SIZE.Y / 4 - PausedFont.MeasureString(line0).X / 2), Color.White);
                    spriteBatch.DrawString(PausedFont, line1, new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line1).X / 2, SCREEN_SIZE.Y / 2 - PausedFont.MeasureString(line1).Y / 2), Color.White);
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.RunningLevel)
                {
                    //draw special running things...?
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel)
                {
                    string line0 = "Level:" + Levels[CurrentLevel].LevelNumber;
                    string line1 = "COMPLETE";
                    score.Stop();
                    spriteBatch.DrawString(PausedFont, line0, new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line0).X / 2, SCREEN_SIZE.Y / 4 - PausedFont.MeasureString(line0).Y / 2), Color.White);
                    spriteBatch.DrawString(PausedFont, line1, new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line1).X / 2, SCREEN_SIZE.Y / 2 - PausedFont.MeasureString(line1).Y / 2), Color.White);
                }

                if (DevMode)
                {
                    spriteBatch.DrawString(devFont, "Running Player HP: " + thePlayer.CurrHP, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(devFont, "Level: " + Levels[CurrentLevel].LevelNumber, new Vector2(0, 20), Color.White);
                } 
                
                //Draw background
                //Draw player
                
                //Draw enemies
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                helpDraw(gameTime);
            
                if (DevMode)
                {
                    spriteBatch.DrawString(devFont, "AlphaShader value: " + AlphaShader.AlphaVal, Vector2.Zero, Color.White);
                }
                spriteBatch.DrawString(PausedFont, "PAUSED", new Vector2(SCREEN_SIZE.X / 2, SCREEN_SIZE.Y / 3) - new Vector2(PausedFont.MeasureString("PAUSED").X * 0.5f, PausedFont.MeasureString("PAUSED").Y * 0.5f), new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });
                spriteBatch.DrawString(PausedFont, "press esc to exit", new Vector2(SCREEN_SIZE.X / 2, 2 * SCREEN_SIZE.Y / 3) - new Vector2(PausedFont.MeasureString("press esc to exit").X * 0.5f, PausedFont.MeasureString("press esc to exit").Y * 0.5f), new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });
                //Update text of pause state
                
            }

            score.draw(spriteBatch);

            if (DevMode)
                spriteBatch.DrawString(devFont, "Completed: " + Levels[CurrentLevel].PercentCompleted() + "%", new Vector2(0, 40), Color.White);
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        private void helpDraw(GameTime gameTime)
        {
            Color drawColor = Color.White;
            if (curState == global::PodPanic.GameState.GameStateEnum.GamePause || lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel) drawColor = Color.Gray;
            backTemp.drawColor = drawColor;
            backTemp.Draw(gameTime);
            thePlayer.drawColor = drawColor;
            thePlayer.Draw(gameTime);
            foreach (GameObjects.GameObject obj in Objects)
            {
                obj.drawColor = drawColor;
                obj.Draw(gameTime);
            }
        }

        public int getYChannel(global::PodPanic.GameState.Channel arg0)
        {
            switch (arg0)
            {
                case global::PodPanic.GameState.Channel.Top: return TOPCHANNEL_Y;
                case global::PodPanic.GameState.Channel.Middle: return MIDCHANNEL_Y;
                case global::PodPanic.GameState.Channel.Bottom: return BOTCHANNEL_Y;
            }
            return 0;
        }


    }
}
