using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VoronoiLib;
using VoronoiLib.Structures;

namespace VoronoiDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D t;
        private List<FortuneSite> points;
        private List<VEdge> edges;

        public Game1()
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
            points = new List<FortuneSite>
            {
                new FortuneSite(200, 150),
                new FortuneSite(200, 200),
                new FortuneSite(100, 100),
                new FortuneSite(300, 125),
                new FortuneSite(500, 425),
                new FortuneSite(190, 325),
                new FortuneSite(600, 120)
            };
            edges = FortunesAlgorithm.Run(points, 0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // create 1x1 texture for line drawing
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });// fill the texture with white
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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

            foreach (var point in points)
                DrawPoint(spriteBatch, point);
            foreach (var edge in edges)
            {
                DrawLine(spriteBatch, edge);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawPoint(SpriteBatch sb, FortuneSite point)
        {
            var size = 10;
            var r = new Rectangle((int) (point.X - size /2.0), (int) (point.Y - size /2.0), size, size);
            sb.Draw(t, r, Color.Green);
        }

        private void DrawLine(SpriteBatch sb, VEdge vEdge)
        {
            float angle;
            var rect = vEdge.ToRectangle(out angle);
            sb.Draw(t,
                rect, //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }

    internal static class FortuneMongoGameExtensions
    {
        public static Vector2 ToVector2(this FortuneSite site)
        {
            return new Vector2((float) site.X, (float) site.Y);
        }

        public static Vector2 ToVector2(this VPoint site)
        {
            return new Vector2((float)site.X, (float)site.Y);
        }

        public static Vector2 ToVector2(this VEdge edge)
        {
            return edge.End.ToVector2() - edge.Start.ToVector2();
        }


        public static Rectangle ToRectangle(this VEdge edge, out float angle)
        {
            var vector = edge.ToVector2();
            angle = (float)Math.Atan2(vector.Y, vector.X);
            return new Rectangle((int) edge.Start.X, (int) edge.Start.Y, (int)vector.Length(), 1);
        }
    }
}
