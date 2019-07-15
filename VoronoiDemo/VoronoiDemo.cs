using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiLib;
using VoronoiLib.Structures;

namespace VoronoiDemo
{
    //Demo for VoronoiLib
    public class VoronoiDemo : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D t;
        private SpriteFont sf;
        private List<FortuneSite> points;
        private LinkedList<VEdge> edges;
        private List<Tuple<Vector2, Vector2>> delaunay;
        private KeyboardState keyboard;
        private MouseState mouse;
        private bool wiggle = true,
            showVoronoi = true,
            showCells = false,
            showDelaunay = true,
            showHelp = true;

        private Random r;
        private Rectangle help;
        private Color helpColor;
        private const int GEN_COUNT = 500;

        public VPoint testPoint = null;

        public VoronoiDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //set full screen
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.ToggleFullScreen();
            IsMouseVisible = true;
            points = new List<FortuneSite>();
            edges = new LinkedList<VEdge>();
            delaunay = new List<Tuple<Vector2, Vector2>>();
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            r = new Random(100);
            help = new Rectangle(0, 0, 285, 200);
            helpColor = new Color(Color.Black, (float).5);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // create 1x1 texture for line drawing
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });// fill the texture with white
            sf = Content.Load<SpriteFont>("Courier New");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var newKeys = Keyboard.GetState();
            var newMouse = Mouse.GetState();
            if (keyboard.IsKeyDown(Keys.H) && newKeys.IsKeyUp(Keys.H))
                showHelp = !showHelp;
            if (keyboard.IsKeyDown(Keys.G) && newKeys.IsKeyUp(Keys.G))
                GeneratePoints();
            if (keyboard.IsKeyDown(Keys.C) && newKeys.IsKeyUp(Keys.C))
                ClearPoints();
            if (keyboard.IsKeyDown(Keys.W) && newKeys.IsKeyUp(Keys.W))
                wiggle = !wiggle;
            if (keyboard.IsKeyDown(Keys.V) && newKeys.IsKeyUp(Keys.V))
                showVoronoi = !showVoronoi;
            if (keyboard.IsKeyDown(Keys.B) && newKeys.IsKeyUp(Keys.B))
                showCells = !showCells;
            if (keyboard.IsKeyDown(Keys.D) && newKeys.IsKeyUp(Keys.D))
                showDelaunay = !showDelaunay;
            if (mouse.LeftButton == ButtonState.Pressed && newMouse.LeftButton == ButtonState.Released)
                AddPoint(mouse.X, mouse.Y);
            if (mouse.RightButton == ButtonState.Pressed && newMouse.RightButton == ButtonState.Released)
                testPoint = new VPoint(mouse.X, mouse.Y);
            if (wiggle && points.Count > 0)
                WigglePoints();
            keyboard = newKeys;
            mouse = newMouse;
            ////mouse = newMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (showVoronoi)
            {
                foreach (var edge in edges)
                {
                    DrawLine(spriteBatch, edge);
                }
            }

            if (showCells)
            {
                foreach (var point in points)
                {
                    for (int i = 0; i < point.Cell.Points.Count - 1; i++)
                    {
                        DrawLine(spriteBatch, point.Cell.Points[i], point.Cell.Points[i + 1]);
                    }
                }
            }

            if (showDelaunay)
            {
                foreach (var edge in delaunay)
                {
                    DrawLine(spriteBatch, edge);
                }
            }

            foreach (var point in points)
            {
                DrawPoint(spriteBatch, point);
            }

            if (showHelp)
                DrawHelp(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ClearPoints()
        {
            points.Clear();
            edges.Clear();
            delaunay.Clear();
        }



        private void AddPoint(int x, int y)
        {
            var newPoints = new List<FortuneSite>();
            if (points.Count > 0)
            {
                newPoints.AddRange(points.Select(point => new FortuneSite(point.X, point.Y)));
            }

            newPoints.Add(new FortuneSite(x, y));
            points = newPoints;

            edges = FortunesAlgorithm.Run(points, 0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            GenerateDelaunay();
        }

        private void GeneratePoints()
        {
            points.Clear();

            //generate points
            var w = graphics.GraphicsDevice.Viewport.Width;
            var h = graphics.GraphicsDevice.Viewport.Height;
            for (var i = 0; i < GEN_COUNT; i++)
            {
                points.Add(new FortuneSite(
                    r.Next((int)(w / 20.0), (int)(19 * w / 20.0)),
                    r.Next((int)(h / 20.0), (int)(19 * h / 20.0))));
            }

            //uniq the points
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

            var unique = new List<FortuneSite>(points.Count / 2);
            var last = points.First();
            unique.Add(last);
            for (var index = 1; index < points.Count; index++)
            {
                var point = points[index];
                if (!last.X.ApproxEqual(point.X) ||
                    !last.Y.ApproxEqual(point.Y))
                {
                    unique.Add(point);
                    last = point;
                }
            }
            points = unique;

            edges = FortunesAlgorithm.Run(points, 0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            GenerateDelaunay();
            //convert ajd list to edge list... edges get double added
        }

        private void GenerateDelaunay()
        {
            var processed = new HashSet<FortuneSite>();
            delaunay.Clear();
            foreach (var site in points)
            {
                foreach (var neighbor in site.Neighbors)
                {
                    if (!processed.Contains(neighbor))
                    {
                        delaunay.Add(
                            new Tuple<Vector2, Vector2>(
                                new Vector2((float)site.X, (float)site.Y),
                                new Vector2((float)neighbor.X, (float)neighbor.Y)
                            ));
                    }
                }
                processed.Add(site);
            }
        }

        private void WigglePoints()
        {
            var newPoints = new List<FortuneSite>(points.Count);
            if (points.Count > 0)
                newPoints.AddRange(points.Select(point => new FortuneSite(point.X + 5 * r.NextDouble() - 2.5, point.Y + 5 * r.NextDouble() - 2.5)));
            points = newPoints;

            edges = FortunesAlgorithm.Run(points, 0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            GenerateDelaunay();
        }

        private void DrawHelp(SpriteBatch sb)
        {
            sb.Draw(t, help, helpColor);
            sb.DrawString(sf, "Controls: ", new Vector2(5, 10), Color.White);
            sb.DrawString(sf, "[H] Show/ Hide Help", new Vector2(10, 30), Color.White);
            sb.DrawString(sf, "[Click] Add Point", new Vector2(10, 50), Color.White);
            sb.DrawString(sf, "[G] Generate Points", new Vector2(10, 70), Color.White);
            sb.DrawString(sf, "[C] Clear Points", new Vector2(10, 90), Color.White);
            sb.DrawString(sf, "[V] Show/ Hide Voronoi Cells", new Vector2(10, 110), Color.White);
            sb.DrawString(sf, "[B] Show/ Hide Cells based on Points", new Vector2(10, 130), Color.White);
            sb.DrawString(sf, "[D] Show/ Hide Delaunay Triangulation", new Vector2(10, 150), Color.White);
            sb.DrawString(sf, "[Esc] Exit", new Vector2(10, 170), Color.White);
        }

        private void DrawPoint(SpriteBatch sb, FortuneSite point)
        {
            var size = 5;
            var rect = new Rectangle((int)(point.X - size / 2.0), (int)(point.Y - size / 2.0), size, size);

            var color = Color.Green;
            if (testPoint != null && point.Cell.Contains(testPoint))
            {
                color = Color.Red;
            }

           sb.Draw(t, rect, color);
        }

        private void DrawLine(SpriteBatch sb, VPoint start, VPoint end)
        {
            var startV = new Vector2((float)start.X, (float)start.Y);
            var endV = new Vector2((float)end.X, (float)end.Y);

            var vector = endV - startV;
            var angle = (float)Math.Atan2(vector.Y, vector.X);
            var rect = new Rectangle((int)start.X, (int)start.Y, (int)vector.Length(), 1);

            sb.Draw(t,
                rect, //width of line, change this to make thicker line
                null,
                Color.LimeGreen, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
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
            var rect = new Rectangle((int)edge.Item1.X, (int)edge.Item1.Y, (int)diff.Length(), 1);
            sb.Draw(t,
                rect,
                null,
                Color.YellowGreen,
                (float)angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }
    }

    internal static class FortuneMongoGameExtensions
    {
        public static Vector2 ToVector2(this FortuneSite site)
        {
            return new Vector2((float)site.X, (float)site.Y);
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
            return new Rectangle((int)edge.Start.X, (int)edge.Start.Y, (int)vector.Length(), 1);
        }
    }
}
