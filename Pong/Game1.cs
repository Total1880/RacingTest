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
        private List<Car> _cars;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _cars = new List<Car>();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 300;
            _graphics.ApplyChanges();

            _cars.Add(new Car(0, new Vector2(0, 50), 0f, 200f));
            _cars.Add(new Car(1, new Vector2(0, 50), 0f, 200f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _cars[0].CarTexture = Content.Load<Texture2D>("car_10a");
            _cars[1].CarTexture = Content.Load<Texture2D>("car_2a");
            _trackTexture = Content.Load<Texture2D>("track_01");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var car in _cars)
            {
                if (_cars.Any(c => 
                c.CarPosition.X >= car.CarPosition.X 
                && c.CarPosition.X <= (car.CarPosition.X + car.CarTexture.Width)
                && c.CarPosition.Y == car.CarPosition.Y
                && c.Id != car.Id
                ))
                {
                    car.UpdateCarPosition(gameTime, _graphics.PreferredBackBufferWidth, false, true);
                }
                else if (!_cars.Any(c =>
                c.CarPosition.X >= car.CarPosition.X
                && c.CarPosition.X <= (car.CarPosition.X + car.CarTexture.Width)
                && c.Id != car.Id
                ) && car.CarPosition.Y > 50)
                {
                    car.UpdateCarPosition(gameTime, _graphics.PreferredBackBufferWidth, true, false);
                }
                else
                {
                    car.UpdateCarPosition(gameTime, _graphics.PreferredBackBufferWidth, false, false);
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

            foreach (var car in _cars)
            {
                _spriteBatch.Draw(car.CarTexture, car.CarPosition, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class Car
    {
        private int _id;
        private Texture2D _carTexture;
        private Vector2 _carPosition;
        private float _minSpeed;
        private float _maxSpeed;

        public Texture2D CarTexture { get => _carTexture; set { _carTexture = value; } }
        public Vector2 CarPosition { get => _carPosition; set { _carPosition = value; } }
        public int Id { get => _id; }

        public Car(int id, Vector2 carPosition, float minSpeed, float maxSpeed)
        {
            _id = id;
            _carPosition = carPosition;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
        }

        public void UpdateCarPosition(GameTime gameTime, int width, bool goToLeft, bool goToRight)
        {
            //_carPosition.Y = 50;

            _carPosition.X += RandomCarSpeed(_minSpeed, _maxSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_carPosition.X + _carTexture.Width > width)
                _carPosition.X = 0;

            if (goToRight)
                _carPosition.Y += _carTexture.Height;

            if (goToLeft)
                _carPosition.Y -= _carTexture.Height;
        }

        static float RandomCarSpeed(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }
    }
}
