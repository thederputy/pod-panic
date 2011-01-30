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
        GameState.KeyMapping KeyMapping;
        GameState.ButtonMapping ButtonMapping;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; set; }
        GameState.GameStateEnum curState;
        SpriteFont devFont;
        SpriteFont PausedFont;
        GameObjects.Player thePlayer;
        GameState.AlphaShader AlphaShader;
        GameState.AlphaBlinker AlphaBlinker;
        GameObjects.Background backTemp;
        Texture2D Net;
        Texture2D OilBarrel;
        Texture2D Fish;
        List<GameObjects.GameObject> Objects;
        bool DevMode;
        GameObjects.Menu mainMenu;
        Random rand;
        LevelObjects.LevelLogic[] Levels;
        int CurrentLevel;
        GameState.LevelProgress lvlProgress;
        int secondsSinceStart;
        int secondsSinceLastEvent;
        /// <summary>
        /// The current position in the level
        /// </summary>
        int distanceCovered;
        Color overlayColor = new Color(255,100,100,0);
        Color crossColor = new Color(255, 100, 100, 0);
        Texture2D overlay;
        Texture2D cross;
        Texture2D[] BonusTexturesArray;
        Texture2D BonusTexture;
        

        GameState.GameStateEnum prevState;


        #region Sound Effects
        SoundEffect ambientWavesEngine;
        SoundEffect barrelHitEngine;
        SoundEffect chompEngine;
        SoundEffect entrySplashEngine;
        SoundEffect finSplashEngine;
        SoundEffect gameStartEngine;
        SoundEffect orcaWhineEngine;

        SoundEffectInstance ambientWavesInstance;
        SoundEffectInstance barrelHitInstance;
        SoundEffectInstance chompInstance;
        SoundEffectInstance entrySplashInstance;
        SoundEffectInstance finSplashInstance;
        SoundEffectInstance gameStartInstance;
        SoundEffectInstance orcaWhineInstance;

        
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
            curState = global::PodPanic.GameState.GameStateEnum.Menu;
            AlphaShader = new global::PodPanic.GameState.AlphaShader();
            AlphaBlinker = new global::PodPanic.GameState.AlphaBlinker();
            backTemp = new global::PodPanic.GameObjects.Background(this);
            Objects = new List<global::PodPanic.GameObjects.GameObject>();
            lvlProgress = global::PodPanic.GameState.LevelProgress.StartingLevel;
            CurrentLevel = 0;
            GameObjects.Player.Speed = 1;
            secondsSinceStart = 0;
            secondsSinceLastEvent = 0;
            rand = new Random();
            KeyMapping = GameState.KeyMapping.GetDefaultKeyMap();
            ButtonMapping = GameState.ButtonMapping.GetDefaultButtonMap();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            thePlayer = new global::PodPanic.GameObjects.Player(this.Content.Load<Texture2D>("Orca/OrcaFinal2"), this.Content.Load<Texture2D>("Orca/OrcaSkeletons"), this);
            backTemp.ForegroundTexture = this.Content.Load<Texture2D>("Background/Water_Final");
            backTemp.MidegroundTexture = this.Content.Load<Texture2D>("Background/MidGround");
            backTemp.BackgroundTexture = this.Content.Load<Texture2D>("Background/SkyandDepth");
            Net = this.Content.Load<Texture2D>("Enemies/Net_Test");
            OilBarrel = this.Content.Load<Texture2D>("Enemies/OilBarrel");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            devFont = this.Content.Load<SpriteFont>("DevFont");
            PausedFont = this.Content.Load<SpriteFont>("PausedFont");
            Texture2D test = this.Content.Load<Texture2D>("MenuItem");
            Texture2D logo = this.Content.Load<Texture2D>("LOGO");
            mainMenu = new global::PodPanic.GameObjects.Menu((int)(SCREEN_SIZE.X / 2 - logo.Width/2), (int)(SCREEN_SIZE.Y / 4 - logo.Height / 2), new List<string>(new String[] { "Start", "How To Play", "Credits", "Exit" }), test, logo, devFont);
            Levels = new global::PodPanic.LevelObjects.LevelLogic[4];

            //List<Texture2D> fList = new List<Texture2D>();
            Fish = this.Content.Load<Texture2D>("Food/Salmon_Sprite");
            overlay = this.Content.Load<Texture2D>("Background/OverLay");
            cross = this.Content.Load<Texture2D>("WhaleSkull");
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
            //***************************************************
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

            orcaWhineEngine = Content.Load<SoundEffect>("Sounds/orcaWhine");
            orcaWhineInstance = orcaWhineEngine.CreateInstance();
            //*******************************************************
            #endregion

            BonusTexturesArray = new Texture2D[5];
            //Set the contents of the array
            BonusTexturesArray[0] = Content.Load<Texture2D>("Slides/HowToPlay");
            BonusTexturesArray[1] = Content.Load<Texture2D>("Slides/epicFAIL");
            BonusTexturesArray[2] = Content.Load<Texture2D>("Slides/gameOver");
            BonusTexturesArray[3] = Content.Load<Texture2D>("Slides/Victory");
            BonusTexturesArray[4] = Content.Load<Texture2D>("Slides/Credits");
            BonusTexture = null;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyManager.Update(gameTime);
            updateOverLay();
            
            if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                SoundManager.playSound(gameStartInstance, 0.2f);
                if (keyManager.KeyPressed(KeyMapping.ActionKey) || keyManager.ButtonPressed(ButtonMapping.ActionKey))
                {
                    char firstChar = mainMenu.getItem().ToCharArray()[0];
                    if (firstChar == 'S')
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                    }
                    else if (firstChar == 'H')
                    {
                        BonusTexture = BonusTexturesArray[0];
                        prevState = curState;
                        curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
                    }
                    else if (firstChar == 'C')
                    {
                        BonusTexture = BonusTexturesArray[4];
                        prevState = curState;
                        curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
                    }
                    else if (firstChar == 'E')
                    {
                        this.Exit();
                    }
                }
                if (keyManager.KeyPressed(KeyMapping.MoveUp) || keyManager.ButtonPressed(ButtonMapping.MoveUp))
                    mainMenu.moveUp();
                if (keyManager.KeyPressed(KeyMapping.MoveDown) || keyManager.ButtonPressed(ButtonMapping.MoveDown))
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
                    secondsSinceStart += (int)gameTime.ElapsedGameTime.Milliseconds;
                    //System.Diagnostics.Trace.WriteLine(gameTime.ElapsedRealTime.Milliseconds);
                    SoundManager.playSound(entrySplashInstance, 0.5f);
                    if (keyManager.KeyPressed(KeyMapping.ExitKey) || keyManager.ButtonPressed(ButtonMapping.ExitKey))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
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
                    //Event Spawning Logic - dooood, do shit here. Now.
                    secondsSinceLastEvent += 1;
                    if (secondsSinceLastEvent >= Levels[CurrentLevel].TimeBetweenEvents + rand.Next(0, 100))
                    {
                        int position = rand.Next(0, 9)/3;
                        GameState.Channel newChannel = global::PodPanic.GameState.Channel.Middle;
                        if(position == 0)
                            newChannel = global::PodPanic.GameState.Channel.Top;
                        else if(position == 1)
                            newChannel = global::PodPanic.GameState.Channel.Middle;
                        else if(position == 2)
                            newChannel = global::PodPanic.GameState.Channel.Bottom;
                        secondsSinceLastEvent = 0;
                        float chance = rand.Next(0, 20) / 20.0f;
                        if (chance < Levels[CurrentLevel].ProbabilityEnemyFish)
                        {
                            chance = rand.Next(0, 20) / 20.0f;
                            GameObjects.Enemy newEnemy;
                            if (chance < Levels[CurrentLevel].ProbabilityEnemyType)
                                newEnemy = new global::PodPanic.GameObjects.Enemy((int)SCREEN_SIZE.X, getYChannel(newChannel), Net, 5, this, GameState.EnemyType.Net);
                            else
                                newEnemy = new global::PodPanic.GameObjects.Enemy((int)SCREEN_SIZE.X, getYChannel(newChannel), OilBarrel, 5, this, GameState.EnemyType.Barrel);
                            Objects.Add(newEnemy);
                        }
                        else
                        {
                            GameObjects.Fish newFish = new global::PodPanic.GameObjects.Fish((int)SCREEN_SIZE.X, getYChannel(newChannel), Fish, Fish, this);
                            Objects.Add(newFish);
                        }
                    }
                    distanceCovered += GameObjects.Player.Speed;
                    if (distanceCovered >= Levels[CurrentLevel].LevelLength)
                    {
                        lvlProgress = global::PodPanic.GameState.LevelProgress.FinishedLevel;
                        distanceCovered = 0;
                    }
                    if (keyManager.KeyPressed(KeyMapping.ExitKey) || keyManager.ButtonPressed(ButtonMapping.ExitKey))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                        SoundManager.pauseSound(ambientWavesInstance);
                    }
                    if (keyManager.KeyPressed(KeyMapping.MoveUp) || keyManager.ButtonPressed(ButtonMapping.MoveUp))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveUp();
                    }
                    else if (keyManager.KeyPressed(KeyMapping.MoveDown) || keyManager.ButtonPressed(ButtonMapping.MoveDown))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.modeDown();
                    }
                    if (keyManager.isKeyDown(KeyMapping.MoveRight) || keyManager.isButtonDown(ButtonMapping.MoveRight))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveRight();
                    }
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel)
                {
                    SoundManager.pauseSound(ambientWavesInstance);
                    SoundManager.playSound(orcaWhineInstance, 0.6f);

                    if (keyManager.KeyPressed(KeyMapping.ActionKey) || keyManager.ButtonPressed(ButtonMapping.ActionKey))
                    {
                        lvlProgress = global::PodPanic.GameState.LevelProgress.StartingLevel;
                        if (!(CurrentLevel + 1 >= Levels.Length))
                        {
                            CurrentLevel += 1;
                            GameObjects.Player.Speed += 3;
                        }
                        else
                        {
                            BonusTexture = BonusTexturesArray[thePlayer.whatVictory()];

                            Reset();

                            prevState = global::PodPanic.GameState.GameStateEnum.Menu;
                            curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
                        }
                    }
                }
                backTemp.Update(gameTime);
                for ( int i = 0; i < Objects.Count; i++)
                {
                    GameObjects.GameObject obj = Objects[i];
                    obj.Update(gameTime);
                    //Collision Detection
                    if(new Rectangle((int)obj.getPosition().X + 25, (int)obj.getPosition().Y + 25, 150, 100).Intersects(new Rectangle((int)thePlayer.getPosition().X, (int)getYChannel(thePlayer.currChannel), 200, 150)))
                    {
                        //has collided with object - friend or foe?
                        if(obj.GetType() == typeof(GameObjects.Enemy))
                        {
                            GameObjects.Enemy enemy = obj as GameObjects.Enemy;
                            if (enemy.hasHitPlayer == false)
                            {
                                thePlayer.reduceHP(enemy.getDamage());
                                enemy.hasHitPlayer = true;

                                // play the appropriate sounds
                                switch (enemy.type)
                                {
                                    case GameState.EnemyType.Net:
                                        //SoundManager.playSound(netHitInstance, 0.6f);
                                        break;
                                    case GameState.EnemyType.Barrel:
                                        SoundManager.playSound(barrelHitInstance, 0.6f);
                                        break;
                                    default:
                                        break;
                                }
                            } 
                        }
                        else // we've collided with some fish
                        {
                            SoundManager.playSound(chompInstance, 0.6f);
                            GameObjects.Fish fish = obj as GameObjects.Fish;
                            if (fish.hasHitPlayer == false)
                            {
                                if (fish.isSick())
                                {
                                    thePlayer.reduceHP((int)(fish.FoodValue * 3));
                                }
                                else
                                {
                                    thePlayer.increaseHP(fish.FoodValue);
                                }
                                fish.hasHitPlayer = true;
                            }
                        }
                    } // end collision detection
                    if (obj.getPosition().X < -obj.getTexture().Width)
                        Objects.Remove(obj);
                    if (obj.signalRemoval)
                        Objects.Remove(obj);   
                }
                //Update Player Position
                thePlayer.Update(gameTime);
            }

            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                if (keyManager.KeyPressed(KeyMapping.ActionKey) || keyManager.ButtonPressed(ButtonMapping.ActionKey))
                {
                    curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                }

                if (keyManager.KeyPressed(KeyMapping.ExitKey) || keyManager.ButtonPressed(ButtonMapping.ExitKey))
                    this.Exit();
                //Update text of pause state
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.DisplayTexture)
            {
                if (keyManager.KeyPressed(KeyMapping.ActionKey) || keyManager.ButtonPressed(ButtonMapping.ActionKey))
                {
                    curState = prevState;
                }
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

            base.Update(gameTime);
        }

        private void Reset()
        {
            CurrentLevel = 0;
            thePlayer.reset();
        }

        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                backTemp.Draw(gameTime);
                if (DevMode)
                    spriteBatch.DrawString(devFont, "Menu State", Vector2.Zero, Color.White);
                mainMenu.draw(spriteBatch);
                //drawCredits(spriteBatch,25,250); // /// creadits
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
                    //draw special running things...
                    //drawing level progress
                    float fontHeight = devFont.LineSpacing;
                    float fontY = 5;
                    float progressPercent = ((float)distanceCovered / Levels[CurrentLevel].LevelLength) * 100;
                    string percentString = "Level " + Levels[CurrentLevel].LevelNumber;
                    percentString += ": " + (int)progressPercent + "%";
                    spriteBatch.DrawString(devFont, percentString, new Vector2(5, fontY), Color.White);

                    //draw lives
                    fontY += fontHeight;
                    spriteBatch.DrawString(devFont, "Lives: " + thePlayer.LivesOwned, new Vector2(5, fontY), Color.White);

                    //draw HP
                    fontY += fontHeight;
                    spriteBatch.DrawString(devFont, "Current HP: " + thePlayer.CurrHP, new Vector2(5, fontY), Color.White);
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel)
                {
                    string line0 = "Level:" + Levels[CurrentLevel].LevelNumber;
                    string line1 = "COMPLETE";
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
            else if (curState == global::PodPanic.GameState.GameStateEnum.DisplayTexture)
            {
                spriteBatch.Draw(BonusTexture, Vector2.Zero, Color.White);
            }

            if (DevMode)
                spriteBatch.DrawString(devFont, "Completed: " + Levels[CurrentLevel].PercentCompleted() + "%", new Vector2(0, 40), Color.White);

            drawOverLay(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        private void updateOverLay()
        {
            if (thePlayer.getHealthPercent() < 25)
            {
                if (thePlayer.getHealthPercent() > 12)
                    crossColor.A = (byte)((25 - thePlayer.getHealthPercent()) * 20);
                else
                    crossColor.A = (byte)(240);
                
            }
            else
            {
                crossColor.A = 0;
            }

            if (thePlayer.getHealthPercent() < 60)
            {
                overlayColor.A = (byte)((60 - thePlayer.getHealthPercent()) * 4);
                //System.Diagnostics.Trace.WriteLine(overlayColor.A);
            }
            else
            {
                overlayColor.A = 0 ;
            }

             
        }

        private void drawOverLay(SpriteBatch spriteBatch)
        {
            overlayColor.R = thePlayer.drawColor.R;
            crossColor.R = thePlayer.drawColor.R;
            //crossColor.A = 255;
            //crossColor.B = 100;


            spriteBatch.Draw(overlay, new Rectangle(0, 0, (int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), overlayColor);//overlayColor
            spriteBatch.Draw(cross, new Rectangle(0, 0, (int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), crossColor); //thePlayer.drawColor
      
            
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
            backTemp.DrawForeground(gameTime);
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


        public void drawCredits(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.DrawString(devFont, "Designer:", new Vector2(x, y + 40 ), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 60 ), Color.White);

            spriteBatch.DrawString(devFont, "Art:", new Vector2(x, y + 100 ), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 120), Color.White);

            spriteBatch.DrawString(devFont, "Sound:", new Vector2(x, y + 160), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 180), Color.White);

            spriteBatch.DrawString(devFont, "Programing:", new Vector2(x, y + 220), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 240), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 260), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 280), Color.White);
            spriteBatch.DrawString(devFont, "NAME", new Vector2(x + 20, y + 300), Color.White);
        }
    }
}
