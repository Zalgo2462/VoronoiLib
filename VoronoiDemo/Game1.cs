using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Utilities.Png;
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
        private LinkedList<VEdge> edges;
        private List<Tuple<Vector2, Vector2>> delaunay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.ToggleFullScreen();

            points = new List<FortuneSite>();
            var r = new Random();
            for (var i = 0; i < 500; i++)
            {
                points.Add(new FortuneSite(r.Next(1, graphics.GraphicsDevice.Viewport.Width), r.Next(1, graphics.GraphicsDevice.Viewport.Height)));
            }
            points.Add(new FortuneSite(200, 0));
            points.Sort((p1, p2) =>
            {
                if (p1.X.ApproxEqual(p2.X))
                {
                    if (p1.Y.ApproxEqual(p2.Y))
                        return 0;
                    if (p1.Y < p2.Y)
                        return -1;
                    return 1;
                }
                if (p1.X < p2.X)
                    return -1;
                return 1;
            });
            for (var i = points.Count - 1; i > 0; i--)
            {
                if (points[i].X.ApproxEqual(points[i - 1].X) &&
                    points[i].Y.ApproxEqual(points[i - 1].Y))
                {
                    points.RemoveAt(i);
                }
            }
            edges = FortunesAlgorithm.Run(points, 0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            delaunay = new List<Tuple<Vector2, Vector2>>();

            foreach (var site in points)
            {
                foreach (var neighbor in site.Neighbors)
                {
                    delaunay.Add(
                        new Tuple<Vector2, Vector2>(
                        new Vector2((float) site.X, (float) site.Y),
                        new Vector2((float) neighbor.X, (float) neighbor.Y)
                        ));
                }
            }
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // create 1x1 texture for line drawing
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });// fill the texture with white
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            foreach (var edge in edges)
            {
                DrawLine(spriteBatch, edge);
            }
            foreach (var edge in delaunay)
            {
                DrawLine(spriteBatch, edge);
            }
            foreach (var point in points)
            {
                DrawPoint(spriteBatch, point);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawPoint(SpriteBatch sb, FortuneSite point)
        {
            var size = 5;
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

        private void DrawLine(SpriteBatch sb, Tuple<Vector2, Vector2> edge)
        {
            var diff = edge.Item2 - edge.Item1;
            var angle = Math.Atan2(diff.Y, diff.X);
            var rect = new Rectangle((int) edge.Item1.X, (int) edge.Item1.Y, (int) diff.Length(), 1);
            sb.Draw(t,
                rect,
                null,
                Color.YellowGreen,
                (float) angle,
                new Vector2(0, 0),
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
