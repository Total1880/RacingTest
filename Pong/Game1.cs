using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Pong
{
    public class Game1 : Game
    {
        private Texture2D _trackTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Cyclist> _cyclists;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _cyclists = new List<Cyclist>();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 300;
            _graphics.ApplyChanges();

            _cyclists.Add(new Cyclist(0, new Vector2(0, 50), 0f, 200f));
            _cyclists.Add(new Cyclist(1, new Vector2(0, 50), 0f, 200f));
            _cyclists.Add(new Cyclist(2, new Vector2(0, 50), 0f, 200f));
            _cyclists.Add(new Cyclist(3, new Vector2(0, 50), 0f, 200f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _cyclists[0].CyclistTexture = Content.Load<Texture2D>("cyclist");
            _cyclists[1].CyclistTexture = Content.Load<Texture2D>("cyclist");
            _cyclists[2].CyclistTexture = Content.Load<Texture2D>("cyclist");
            _cyclists[3].CyclistTexture = Content.Load<Texture2D>("cyclist");
            _trackTexture = Content.Load<Texture2D>("track_01");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var cyclist in _cyclists)
            {
                if (_cyclists.Any(c => 
                c.CyclistPosition.X >= cyclist.CyclistPosition.X 
                && c.CyclistPosition.X <= (cyclist.CyclistPosition.X + cyclist.CyclistTexture.Width)
                && c.CyclistPosition.Y == cyclist.CyclistPosition.Y
                && c.Id != cyclist.Id
                ))
                {
                    cyclist.UpdateCyclistPosition(gameTime, _graphics.PreferredBackBufferWidth, false, true);
                }
                else if (!_cyclists.Any(c =>
                c.CyclistPosition.X >= cyclist.CyclistPosition.X - cyclist.CyclistTexture.Width
                && c.CyclistPosition.X <= (cyclist.CyclistPosition.X + cyclist.CyclistTexture.Width * 2)
                && c.Id != cyclist.Id
                ) && cyclist.CyclistPosition.Y > 50)
                {
                    cyclist.UpdateCyclistPosition(gameTime, _graphics.PreferredBackBufferWidth, true, false);
                }
                else
                {
                    cyclist.UpdateCyclistPosition(gameTime, _graphics.PreferredBackBufferWidth, false, false);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            int latestTrackX = 0;
            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            do
            {
                _spriteBatch.Draw(_trackTexture, new Vector2(latestTrackX, 0), Color.White);
                latestTrackX += _trackTexture.Width;
            } while (latestTrackX < _graphics.PreferredBackBufferWidth);

            foreach (var car in _cyclists)
            {
                _spriteBatch.Draw(car.CyclistTexture, car.CyclistPosition, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class Cyclist
    {
        private int _id;
        private Texture2D _cyclistTexture;
        private Vector2 _cyclistPosition;
        private float _minSpeed;
        private float _maxSpeed;

        public Texture2D CyclistTexture { get => _cyclistTexture; set { _cyclistTexture = value; } }
        public Vector2 CyclistPosition { get => _cyclistPosition; set { _cyclistPosition = value; } }
        public int Id { get => _id; }

        public Cyclist(int id, Vector2 cyclistPosition, float minSpeed, float maxSpeed)
        {
            _id = id;
            _cyclistPosition = cyclistPosition;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
        }

        public void UpdateCyclistPosition(GameTime gameTime, int width, bool goToLeft, bool goToRight)
        {
            //_cyclistPosition.Y = 50;

            _cyclistPosition.X += RandomCyclistSpeed(_minSpeed, _maxSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_cyclistPosition.X + _cyclistTexture.Width > width)
                _cyclistPosition.X = 0;

            if (goToRight)
                _cyclistPosition.Y += _cyclistTexture.Height;

            if (goToLeft)
                _cyclistPosition.Y -= _cyclistTexture.Height;
        }

        static float RandomCyclistSpeed(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }
    }
}
