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
        private static string BKG_LOC = "Background/";
        public static Vector2 SCREEN_SIZE = new Vector2(800, 600);
        public const int TOPCHANNEL_Y = 150;
        public const int MIDCHANNEL_Y = 300;
        public const int BOTCHANNEL_Y = 450;
        GameState.InputManager keyManager;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; set; }
        GameState.GameStateEnum curState;
        public SpriteFont devFont { get; set; }
        SpriteFont PausedFont;
        GameObjects.Player thePlayer;
        GameState.AlphaShader AlphaShader;
        GameState.AlphaBlinker AlphaBlinker;
        GameObjects.Background backTemp;
        Texture2D Net;
        Texture2D[] OilSlicks;
        Texture2D Fish;
        Texture2D Fish_Sick;
        List<GameObjects.GameObject> Objects;
        //bool DevMode;
        GameObjects.Menu mainMenu;
        Random rand;
        GameObjects.Score score;
        LevelObjects.LevelLogic[] Levels;
        const int numberOfLevels = 4;
        int CurrentLevel;
        GameState.LevelProgress lvlProgress;
        int secondsSinceStart;
        int secondsSinceLastEvent;
        /// <summary>
        /// The current position in the level
        /// </summary>
        int distanceCovered;
        Color overlayColor = new Color(120,50,50,0);
        //Color crossColor = new Color(120, 50, 50, 0);
        //int targetA = 0;
        int counterFamine;

        int TimesWon;
        int TimesLost;

        Texture2D overlay;
        //Texture2D cross;
        Texture2D[] BonusTexturesArray;
        Texture2D BonusTexture;
        VideoPlayer Player;
        Video tutorialVideo;
        Texture2D[,] bkgArray;
        
        GameState.GameStateEnum prevState;


        #region Sound Effects
        SoundEffect ambientWavesEngine;
        SoundEffect barrelHitEngine;
        SoundEffect chompEngine;
        SoundEffect entrySplashEngine;
        SoundEffect finSplashEngine;
        SoundEffect gameStartEngine;
        SoundEffect netCaughtEngine;
        SoundEffect orcaWhineEngine;

        SoundEffectInstance ambientWavesInstance;
        SoundEffectInstance barrelHitInstance;
        SoundEffectInstance chompInstance;
        SoundEffectInstance entrySplashInstance;
        SoundEffectInstance finSplashInstance;
        SoundEffectInstance gameStartInstance;
        SoundEffectInstance netCaughtInstance;
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
            this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();
            Window.Title = "Pod Panic";
            keyManager = new global::PodPanic.GameState.InputManager(this);
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
            GameState.KeyMapping.CurrentKeyMap = GameState.KeyMapping.GetDefaultKeyMap();
            GameState.ButtonMapping.CurrentButtonMap = GameState.ButtonMapping.GetDefaultButtonMap();
            TimesWon = 0;
            TimesLost = 0;
            bkgArray = new Texture2D[4,3];
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            thePlayer = new global::PodPanic.GameObjects.Player(this.Content.Load<Texture2D>("Orca/OrcaFinal2"), this.Content.Load<Texture2D>("Orca/OrcaSkeletons"), this);

            //background texture loading logic
            for (int i = 0; i <= LevelObjects.LevelData.levelBKGs.GetUpperBound(0); i++)
            {
                bkgArray[i,0] = this.Content.Load<Texture2D>(BKG_LOC + LevelObjects.LevelData.levelBKGs[i,0]);
                bkgArray[i,1] = this.Content.Load<Texture2D>(BKG_LOC + LevelObjects.LevelData.levelBKGs[i,1]);
                bkgArray[i,2] = this.Content.Load<Texture2D>(BKG_LOC + LevelObjects.LevelData.levelBKGs[i,2]);
            }
            //bkgArray[3, 0] = this.Content.Load<Texture2D>(BKG_LOC + LevelObjects.LevelData.levelBKGs[3, 0]);
            backTemp.BackgroundTexture = bkgArray[0, 2];
            backTemp.MidegroundTexture = bkgArray[0, 1];
            backTemp.ForegroundTexture = bkgArray[0, 0];
            backTemp.LightareasTexture = this.Content.Load<Texture2D>(BKG_LOC + "WaterLights");
            backTemp.WaveArea = this.Content.Load<Texture2D>("Background/Wave");
            Net = this.Content.Load<Texture2D>("Enemies/Net_Final");
            OilSlicks = new Texture2D[]
            {
                this.Content.Load<Texture2D>("Enemies/OilSpill"),
                this.Content.Load<Texture2D>("Enemies/OilSpill2"),
                this.Content.Load<Texture2D>("Enemies/OilSpill3"),
            };
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            devFont = this.Content.Load<SpriteFont>("DevFont");
            PausedFont = this.Content.Load<SpriteFont>("PausedFont");
            Texture2D test = this.Content.Load<Texture2D>("MenuItem");
            Texture2D logo = this.Content.Load<Texture2D>("LOGO");
            mainMenu = new global::PodPanic.GameObjects.Menu((int)(SCREEN_SIZE.X / 2 - logo.Width/2), (int)(SCREEN_SIZE.Y / 5 - logo.Height / 2), new List<string>(new String[] { "Start", "Tutorial", "High Scores",  "About", "Credits", "Exit" }), test, logo, devFont);
            Levels = new global::PodPanic.LevelObjects.LevelLogic[numberOfLevels];
            score = new GameObjects.Score(new Vector2(600, 5), devFont);

            //List<Texture2D> fList = new List<Texture2D>();
            Fish = this.Content.Load<Texture2D>("Food/Salmon_Sprite");
            Fish_Sick = this.Content.Load<Texture2D>("Food/Salmon_Sprite_Sick");
            overlay = this.Content.Load<Texture2D>("Background/OverLay");
            //cross = this.Content.Load<Texture2D>("WhaleSkull");
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

            // to get to the levels (from LevelData class)
            XmlNode levelNode1 = GameState.LoadingManager.getXmlLevelNodeFromDocument(LevelObjects.LevelData.level1);
            level1.setDataFromXml(levelNode1);
            XmlNode levelNode2 = GameState.LoadingManager.getXmlLevelNodeFromDocument(LevelObjects.LevelData.level2);
            level2.setDataFromXml(levelNode2);
            XmlNode levelNode3 = GameState.LoadingManager.getXmlLevelNodeFromDocument(LevelObjects.LevelData.level3);
            level3.setDataFromXml(levelNode3);
            XmlNode levelNode4 = GameState.LoadingManager.getXmlLevelNodeFromDocument(LevelObjects.LevelData.level4);
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

            netCaughtEngine = Content.Load<SoundEffect>("Sounds/netCaught");
            netCaughtInstance = netCaughtEngine.CreateInstance();

            orcaWhineEngine = Content.Load<SoundEffect>("Sounds/orcaWhine");
            orcaWhineInstance = orcaWhineEngine.CreateInstance();
            //*******************************************************
            #endregion

            BonusTexturesArray = new Texture2D[6];
            //Set the contents of the array
            BonusTexturesArray[0] = Content.Load<Texture2D>("Slides/About");
            BonusTexturesArray[1] = Content.Load<Texture2D>("Slides/ExtinctScreen");
            BonusTexturesArray[2] = Content.Load<Texture2D>("Slides/gameOver");
            BonusTexturesArray[3] = Content.Load<Texture2D>("Slides/Victory");
            BonusTexturesArray[4] = Content.Load<Texture2D>("Slides/Credits");
            BonusTexturesArray[5] = Content.Load<Texture2D>("Slides/ComingSoon");
            BonusTexture = null;

            Player = new VideoPlayer();
            tutorialVideo = this.Content.Load<Video>("Movies/Tutorial");
            Player.Play(tutorialVideo);
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
            //System.Diagnostics.Trace.WriteLine("this :" + lvlProgress);

            keyManager.Update(gameTime);
            updateOverLay();

            GameObjects.Fish.currentLevel = CurrentLevel;

            if (curState == global::PodPanic.GameState.GameStateEnum.Menu)
            {
                
                SoundManager.playSound(gameStartInstance, 0.2f);
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.ActionKey))
                {
                    char firstChar = mainMenu.getItem().ToCharArray()[0];
                    if (firstChar == 'S') //
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                        CurrentLevel = 0;
                        setTextures(CurrentLevel);
                        backTemp.SignalBlendTo();
                        score.Show();
                        score.Start();
                        SoundManager.stopSound(gameStartInstance);
                        SoundManager.startLoopedSound(ambientWavesInstance, 0.05f);
                    }
                    else if (firstChar == 'T')
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.Tutorial;
                        Player.Play(tutorialVideo);
                        Player.IsLooped = false;
                    }
                    else if (firstChar == 'H')
                    {
                        BonusTexture = BonusTexturesArray[5];
                        prevState = curState;
                        curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
                        //curState = global::PodPanic.GameState.GameStateEnum.HighScores;
                    }
                    else if (firstChar == 'A')
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
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.MoveUp))
                    mainMenu.moveUp();
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.MoveDown))
                    mainMenu.moveDown();


                backTemp.Update(gameTime);
                //Check player cursor position
                //Highlight menu options
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.Tutorial)
            {
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.ActionKey) || Player.State == MediaState.Stopped)
                {
                    Player.Stop();
                    curState = global::PodPanic.GameState.GameStateEnum.Menu;
                }
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                //Do Key Detection

                if (lvlProgress == global::PodPanic.GameState.LevelProgress.StartingLevel)
                {
                    secondsSinceStart += (int)gameTime.ElapsedGameTime.Milliseconds;
                    //System.Diagnostics.Trace.WriteLine(gameTime.ElapsedRealTime.Milliseconds);
                    SoundManager.playSound(entrySplashInstance, 0.5f);
                    if (keyManager.isCommandPressed(GameState.KeyMapEnum.ExitKey))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                        score.Stop();
                        SoundManager.pauseSound(ambientWavesInstance);
                    }
                    if (secondsSinceStart >= 1250) //**************************************here
                    {
                        secondsSinceStart = 0;
                        lvlProgress = global::PodPanic.GameState.LevelProgress.RunningLevel;
                        score.Start();
                    }
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.RunningLevel)
                {
                    counterFamine += (int)gameTime.ElapsedGameTime.Milliseconds;
                    if (counterFamine >= 3500) //**************************************here
                    {
                        counterFamine = 0;
                        thePlayer.reduceHP(1);
                    }

                    //Event Spawning Logic - dooood, do shit here.
                    if (Levels[CurrentLevel].PercentCompleted() < 95) //don't spawn enemies that we'll never hit
                    {
                        secondsSinceLastEvent += 1;
                        if (secondsSinceLastEvent >= Levels[CurrentLevel].TimeBetweenEvents + rand.Next(0, 100))
                        {
                            int position = rand.Next(0, 9) / 3;
                            GameState.Channel newChannel = global::PodPanic.GameState.Channel.Middle;
                            if (position == 0)
                                newChannel = global::PodPanic.GameState.Channel.Top;
                            else if (position == 1)
                                newChannel = global::PodPanic.GameState.Channel.Middle;
                            else if (position == 2)
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
                                    newEnemy = new global::PodPanic.GameObjects.Enemy((int)SCREEN_SIZE.X, getYChannel(newChannel), OilSlicks[rand.Next(0, 3)], 5, this, GameState.EnemyType.Barrel);
                                Objects.Add(newEnemy);
                            }
                            else
                            {
                                GameObjects.Fish.setPollutionPercent(Levels[CurrentLevel].ProbabilityFishPollution);
                                GameObjects.Fish newFish = new global::PodPanic.GameObjects.Fish((int)SCREEN_SIZE.X, getYChannel(newChannel), Fish, Fish_Sick, this);
                                Objects.Add(newFish);
                            }
                        }
                    }//end enemy spawning logic

                    distanceCovered += GameObjects.Player.Speed;
                    if (distanceCovered >= Levels[CurrentLevel].LevelLength)
                    {
                        lvlProgress = global::PodPanic.GameState.LevelProgress.FinishedLevel;
                        distanceCovered = 0;
                        if (CurrentLevel + 1 >= Levels.Length)
                            setTextures(CurrentLevel);
                        else
                            setTextures(CurrentLevel + 1);
                        backTemp.SignalBlendTo();
                    }
                    if (keyManager.isCommandPressed(GameState.KeyMapEnum.ExitKey))
                    {
                        curState = global::PodPanic.GameState.GameStateEnum.GamePause;
                        score.Stop();
                        SoundManager.pauseSound(ambientWavesInstance);
                    }
                    if (keyManager.isCommandPressed(GameState.KeyMapEnum.MoveUp))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveUp();
                    }
                    else if (keyManager.isCommandPressed(GameState.KeyMapEnum.MoveDown))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveDown();
                    }
                    if (keyManager.isCommandDown(GameState.KeyMapEnum.MoveRight))
                    {
                        SoundManager.playSound(finSplashInstance, 0.1f);
                        thePlayer.moveRight();
                    }
                }
                else if (lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel)
                {
                    SoundManager.pauseSound(ambientWavesInstance);
                    SoundManager.playSound(orcaWhineInstance, 0.3f);

                    if (keyManager.isCommandPressed(GameState.KeyMapEnum.ActionKey))
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
                            removeObjectsFromGame(gameTime);
                            ResetGame();
                            TimesWon++;
                            prevState = global::PodPanic.GameState.GameStateEnum.Menu;
                            curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
                        }
                    }
                } //end FinishedLevel
                backTemp.Update(gameTime);

                if (!(lvlProgress == global::PodPanic.GameState.LevelProgress.FinishedLevel))
                {
                    //begin collision detection
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        GameObjects.GameObject obj = Objects[i];
                        obj.Update(gameTime);
                        //if (obj.isDead && obj.timeOfDeath >= GameObjects.GameObject.TIME_ON_SCREEN_AFTER_HIT || obj.signalRemoval)
                        //    Objects.Remove(obj);
                        //else
                        //{
                            //Collision Detection
                            //Rectangle objRect = new Rectangle((int)obj.getPosition().X + 25, (int)obj.getPosition().Y + 25, 150, 100);
                            if (thePlayer.Rect.Intersects(obj.Rect))
                            {
                                //has collided with object - friend or foe?
                                if (obj.GetType() == typeof(GameObjects.Enemy))
                                {
                                    GameObjects.Enemy enemy = obj as GameObjects.Enemy;
                                    if (enemy.hasHitPlayer == false && !enemy.isDead)
                                    {
                                        thePlayer.reduceHP(enemy.getDamage());
                                        enemy.hasHitPlayer = true;
                                        score.hitDuringLevel = true;

                                        // play the appropriate sounds for the enemy
                                        // update the score acordingly
                                        switch (enemy.type)
                                        {
                                            case GameState.EnemyType.Net:
                                                SoundManager.playSound(netCaughtInstance, 0.6f);
                                                score.modify(-50);
                                                break;
                                            case GameState.EnemyType.Barrel:
                                                SoundManager.playSound(barrelHitInstance, 0.8f);
                                                score.modify(-30);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                else // we've collided with some fish
                                {
                                    if (!obj.isDead)
                                    {
                                        SoundManager.playSound(chompInstance, 0.6f);
                                        GameObjects.Fish fish = obj as GameObjects.Fish;
                                        if (fish.hasHitPlayer == false)
                                        {
                                            if (fish.isSick())
                                            {
                                                thePlayer.reduceHP((int)(fish.FoodValue * 3));
                                                score.modify(-20);
                                                score.hitDuringLevel = true;
                                            }
                                            else
                                            {
                                                thePlayer.increaseHP(fish.FoodValue);
                                                score.modify(100);
                                            }
                                            fish.hasHitPlayer = true;
                                        }
                                    }
                                }
                            } //end intersection checking
                            //if (obj.getPosition().X < -obj.getTexture().Width)
                            //    Objects.Remove(obj);
                        }
                    //}//end collision detection
                }
                else  // we're at the end of the level, so remove all the enemies and fish
                {
                    removeObjectsFromGame(gameTime);
                }
                //Update Player Position
                thePlayer.Update(gameTime);
            }

            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.ActionKey))
                {
                    curState = global::PodPanic.GameState.GameStateEnum.GameRun;
                    score.Start();
                }

                if (keyManager.isCommandPressed(GameState.KeyMapEnum.ExitKey))
                {
                    curState = global::PodPanic.GameState.GameStateEnum.Menu;
                    removeObjectsFromGame(gameTime);
                    ResetGame();
                }
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.DisplayTexture)
            {
                if (keyManager.isCommandPressed(GameState.KeyMapEnum.ActionKey))
                {
                    curState = prevState;
                }
            }
//            if (keyManager.KeyPressed(Keys.Z)) //triggers DevMode
//            {

//                DevMode = !DevMode;
                //if (DevMode)
                //    DevMode = false;
                //else
                //    DevMode = true;
//            }

            AlphaShader.Update(gameTime);
            AlphaBlinker.Update(gameTime);
            score.Update();

            base.Update(gameTime);
        }


        /// <summary>
        /// Removed the objects from the game.
        /// This method gets called when you end a level or you go from in-game straight to the main menu.
        /// </summary>
        /// <param name="gameTime">the gametime to update</param>
        private void removeObjectsFromGame(GameTime gameTime)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                GameObjects.GameObject obj = Objects[i];
                obj.Update(gameTime);
                if ((obj is GameObjects.Enemy) || (obj is GameObjects.Fish))
                {
                    Objects.Remove(obj);
                }
            }
        }

        /// <summary>
        /// Clears the objects
        /// </summary>
        private void clearObjects()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                GameObjects.GameObject obj = Objects[i];
                if ((obj is GameObjects.Enemy) || (obj is GameObjects.Fish))
                {
                    Objects.Remove(obj);
                }
            }
        }

        private void ResetGame()
        {
            clearObjects();
            for (int i = 0; i < numberOfLevels; i++)
            {
                Levels[i].CurrentPosition = 0;
            }
            CurrentLevel = 0;
            lvlProgress = global::PodPanic.GameState.LevelProgress.StartingLevel;
            distanceCovered = 0;
            thePlayer.resetPlayer();
            score.resetScore();
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
                //if (DevMode)
                //    spriteBatch.DrawString(devFont, "Menu State", Vector2.Zero, Color.White);
                mainMenu.draw(spriteBatch);
                //drawCredits(spriteBatch,25,250); // /// creadits
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GameRun)
            {
                helpDraw(gameTime);
                if (lvlProgress == global::PodPanic.GameState.LevelProgress.StartingLevel)
                {
                    score.beginLevel();
                    string line0 = "Level:" + Levels[CurrentLevel].LevelNumber;
                    string line1 = "\"" + Levels[CurrentLevel].LevelName + "\"";
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
                    score.Stop();
                    if (!score.updatedEndOfLevel)
                    {
                        score.endLevel(thePlayer, Levels[CurrentLevel].LevelNumber);
                    }
                    // get the strings from the score class, then draw them
                    spriteBatch.DrawString(PausedFont, line0, new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line0).X / 2, SCREEN_SIZE.Y / 4 - PausedFont.MeasureString(line0).Y / 2), Color.White);
                    Vector2 completed = new Vector2(SCREEN_SIZE.X / 2 - PausedFont.MeasureString(line1).X / 2, SCREEN_SIZE.Y / 2 - PausedFont.MeasureString(line1).Y / 2);
                    spriteBatch.DrawString(PausedFont, line1, completed, Color.White);
                    string thisLevel = "" + score.levelScore + " points earned this level";
                    Vector2 drawAt = new Vector2(completed.X, SCREEN_SIZE.Y / 2 - PausedFont.MeasureString(thisLevel).Y / 2);
                    drawAt.Y += 80;
                    spriteBatch.DrawString(devFont, thisLevel, drawAt, Color.White);
                    if (!score.hitDuringLevel)
                    {
                        string bonusPoints = "2500 bonus points! No obstacles hit!";
                        drawAt.Y += 25;
                        spriteBatch.DrawString(devFont, bonusPoints, drawAt, Color.White);
                    }
                    string totalPoints = "Total: " + score.totalScore + " points earned so far. Keep it up!";
                    drawAt.Y += 25;
                    spriteBatch.DrawString(devFont, totalPoints, drawAt, Color.White);
                }
                score.draw(spriteBatch);

                //if (DevMode)
                //{
                //    spriteBatch.DrawString(devFont, "Running Player HP: " + thePlayer.CurrHP, Vector2.Zero, Color.White);
                //    spriteBatch.DrawString(devFont, "Level: " + Levels[CurrentLevel].LevelNumber, new Vector2(0, 20), Color.White);
                //    spriteBatch.DrawString(devFont, "times won: " + TimesWon, new Vector2(0,60), Color.White);
                //    spriteBatch.DrawString(devFont, "times lost: " + TimesLost, new Vector2(0,80), Color.White);
                //} 
                
                //Draw background
                //Draw player
                
                //Draw enemies
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.GamePause)
            {
                helpDraw(gameTime);
            
                //if (DevMode)
                //{
                //    spriteBatch.DrawString(devFont, "AlphaShader value: " + AlphaShader.AlphaVal, Vector2.Zero, Color.White);
                //}
                spriteBatch.DrawString(PausedFont, "PAUSED", new Vector2(SCREEN_SIZE.X / 2, SCREEN_SIZE.Y / 3) - new Vector2(PausedFont.MeasureString("PAUSED").X * 0.5f, PausedFont.MeasureString("PAUSED").Y * 0.5f), new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });
                Vector2 stringLoc = new Vector2(SCREEN_SIZE.X / 2, 2 * SCREEN_SIZE.Y / 3);

                stringLoc.Y -= 50;
                string menu = "Back = Main Menu";
                spriteBatch.DrawString(PausedFont, menu,  stringLoc - new Vector2(PausedFont.MeasureString(menu).X * 0.5f, PausedFont.MeasureString(menu).Y * 0.5f), new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });

                stringLoc.Y += 50;
                string resume = "Action = Resume";
                spriteBatch.DrawString(PausedFont, resume, stringLoc - new Vector2(PausedFont.MeasureString(resume).X * 0.5f, PausedFont.MeasureString(resume).Y * 0.5f), new Color() { A = (byte)AlphaShader.AlphaVal, B = 255, G = 255, R = 255 });
                
                
                //Update text of pause state
                
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.DisplayTexture)
            {
                spriteBatch.Draw(BonusTexture, new Rectangle(0,0,(int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), Color.White);
            }
            else if (curState == global::PodPanic.GameState.GameStateEnum.Tutorial)
            {
                spriteBatch.Draw(Player.GetTexture(), new Rectangle(0, 0, (int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), Color.White);
            }

            //score.draw(spriteBatch);

            //if (DevMode)
            //    spriteBatch.DrawString(devFont, "Completed: " + Levels[CurrentLevel].PercentCompleted() + "%", new Vector2(0, 40), Color.White);

            drawOverLay(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        private void updateOverLay()
        {
            //if (thePlayer.getHealthPercent() < 25)
            //{
             //   if (thePlayer.getHealthPercent() > 12)
             //       targetA = (byte)((25 - thePlayer.getHealthPercent()) * 4);
             //   else
             //       targetA = (byte)(50);
            //
             //   if (crossColor.A < targetA)
             //   {
             //       crossColor.A++;
             //   }

            //}

            //else
            //{
             //   crossColor.A = 0;
            //}

            if (thePlayer.getHealthPercent() < 60)
            {
                overlayColor.A = (byte)((60 - thePlayer.getHealthPercent()) * 3);
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
            //crossColor.R = thePlayer.drawColor.R;
            //crossColor.A = 255;
            //crossColor.B = 100;


            spriteBatch.Draw(overlay, new Rectangle(0, 0, (int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), overlayColor);//overlayColor
            //spriteBatch.Draw(cross, new Rectangle(0, 0, (int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y), crossColor); //thePlayer.drawColor
            //spriteBatch.
            
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
            backTemp.DrawLights(gameTime);
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

        public void endGameFail()
        {
            BonusTexture = BonusTexturesArray[1];
            ResetGame();
            TimesLost++;
            prevState = global::PodPanic.GameState.GameStateEnum.Menu;
            curState = global::PodPanic.GameState.GameStateEnum.DisplayTexture;
        }
        public void setTextures(int levelChoice)
        {
            backTemp.SetBlendToTextures(new Texture2D[] { bkgArray[levelChoice, 0], bkgArray[levelChoice, 1], bkgArray[levelChoice, 2] });
        }
    }
}
