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


namespace PodPanic.GameObjects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Texture2D BackgroundTexture { get; set; }
        public Texture2D MidegroundTexture { get; set; }
        public Texture2D ForegroundTexture { get; set; }
        public Texture2D LightareasTexture { get; set; }
        public Texture2D WaveArea { get; set; }
        public Color drawColor { get; set; } 
        private float curXPosBack;
        private float curXPosMide;
        private float curXPosFore;
        private int alphaLeft;
        private int leftLeft;
        private bool isLeftPosit;
        private int alphaRight;
        private int leftRight;
        private bool isRightPosit;
        private byte alphaBlend;
        private Texture2D[] blendToTextures;
        private bool hasSignalledBlendTo;

        public Background(Game game)
            : base(game)
        {
            drawColor = Color.White;
            alphaLeft = 23;
            alphaRight = 217;
            leftLeft = -111;
            leftRight = 125;
            isLeftPosit = true;
            isRightPosit = false;
            alphaBlend = 0;
            blendToTextures = new Texture2D[3];
            hasSignalledBlendTo = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            curXPosFore += Player.Speed;
            //leftWave += Player.Speed;
            //if (leftWave >= WaveArea.Width)
            //    leftWave = 0;
            if (curXPosFore >= ForegroundTexture.Width)
                curXPosFore = 0;
            curXPosMide += (float)(Player.Speed) * 2 / 3;
            if (curXPosMide >= BackgroundTexture.Width)
                curXPosMide = 0;
            curXPosBack += (float)(Player.Speed) * 1 / 3;
            if (curXPosBack >= BackgroundTexture.Width)
                curXPosBack = 0;
            if (isLeftPosit) leftLeft++; else leftLeft--;
            if (isRightPosit) leftRight++; else leftRight--;
            if (leftLeft >= 200) isLeftPosit = false; else if (leftLeft <= -200) isLeftPosit = true;
            if (leftRight >= 200) isRightPosit = false; else if (leftRight <= -200) isRightPosit = true;
            alphaLeft -= 1;
            alphaRight -= 1;
            if (alphaLeft <= -255)
                alphaLeft = 255;
            if (alphaRight <= -255)
                alphaRight = 255;
            base.Update(gameTime);
        }

        public void DrawLights(GameTime gameTime)
        {
            PodPanic curGame = (PodPanic)(this.Game);
            Color colLeft = drawColor;
            colLeft.A = (byte)Math.Abs(alphaLeft);
            Color colRight = drawColor;
            colRight.A = (byte)Math.Abs(alphaRight);
            curGame.spriteBatch.Draw(LightareasTexture, new Vector2(leftLeft, 0), colLeft);
            curGame.spriteBatch.Draw(LightareasTexture, new Rectangle(leftRight, 0, (int)PodPanic.SCREEN_SIZE.X, (int)PodPanic.SCREEN_SIZE.Y), null, colRight, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public override void Draw(GameTime gameTime)
        {
            PodPanic curGame = (PodPanic)(this.Game);
            if (hasSignalledBlendTo)
            {
                alphaBlend += 1;
                if (alphaBlend == 255)
                {
                    BackgroundTexture = blendToTextures[2];
                    MidegroundTexture = blendToTextures[1];
                    ForegroundTexture = blendToTextures[0];
                    hasSignalledBlendTo = false;
                }
                Color drawNewColor = new Color() { A = alphaBlend, R = drawColor.R, B = drawColor.B, G = drawColor.G };

                curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(-curXPosBack, -35), drawColor);
                curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawColor);
                //curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawColor);
                curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(-curXPosMide, -35), drawColor);
                curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawColor);
                //curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawColor);
                curGame.spriteBatch.Draw(ForegroundTexture, new Vector2(-curXPosFore, -35), drawColor);
                curGame.spriteBatch.Draw(ForegroundTexture, new Vector2(ForegroundTexture.Width - curXPosFore, -35), drawColor);

                curGame.spriteBatch.Draw(blendToTextures[2], new Vector2(-curXPosBack, -35), drawNewColor);
                curGame.spriteBatch.Draw(blendToTextures[2], new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawNewColor);
                //curGame.spriteBatch.Draw(blendToTextures[2], new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawNewColor);
                curGame.spriteBatch.Draw(blendToTextures[1], new Vector2(-curXPosMide, -35), drawNewColor);
                curGame.spriteBatch.Draw(blendToTextures[1], new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawNewColor);
                //curGame.spriteBatch.Draw(blendToTextures[1], new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawNewColor);
                curGame.spriteBatch.Draw(blendToTextures[0], new Vector2(-curXPosFore, -35), drawNewColor);
                curGame.spriteBatch.Draw(blendToTextures[0], new Vector2(ForegroundTexture.Width - curXPosFore, -35), drawNewColor);
                    
            }
            else
            {
                alphaBlend = 0;
                curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(-curXPosBack, -35), drawColor);
                curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawColor);
                //curGame.spriteBatch.Draw(BackgroundTexture, new Vector2(ForegroundTexture.Width - curXPosBack, -35), drawColor);
                curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(-curXPosMide, -35), drawColor);
                curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawColor);
                //curGame.spriteBatch.Draw(MidegroundTexture, new Vector2(ForegroundTexture.Width - curXPosMide, -35), drawColor);
                curGame.spriteBatch.Draw(ForegroundTexture, new Vector2(-curXPosFore, -35), drawColor);
                curGame.spriteBatch.Draw(ForegroundTexture, new Vector2(ForegroundTexture.Width - curXPosFore, -35), drawColor);
            }
            //base.Draw(gameTime);
        }

        public void SignalBlendTo()
        {
            if(!hasSignalledBlendTo)
                hasSignalledBlendTo = true;
        }

        public bool HasSignalledBlendTo()
        {
            return hasSignalledBlendTo;
        }

        public void SetBlendToTextures(Texture2D[] NewTextures)
        {
            blendToTextures[0] = NewTextures[0];
            blendToTextures[1] = NewTextures[1];
            blendToTextures[2] = NewTextures[2];
        }
    }
}