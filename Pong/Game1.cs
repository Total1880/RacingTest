using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pong
{
    public class Game1 : Game
    {
        private Texture2D _carTexture1;
        private Texture2D _carTexture2;
        private Texture2D _trackTexture;
        private Vector2 _carPosition1;
        private Vector2 _carPosition2;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 300;
            _graphics.ApplyChanges();

            _carPosition1 = new Vector2(0, 50);
            _carPosition2 = new Vector2(0, 100);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _carTexture1 = Content.Load<Texture2D>("car_10a");
            _carTexture2 = Content.Load<Texture2D>("car_2a");
            _trackTexture = Content.Load<Texture2D>("track_01");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _carPosition1.X += RandomCarSpeed(0f, 100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _carPosition2.X += RandomCarSpeed(0f, 100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_carPosition1.X + _carTexture1.Width > _graphics.PreferredBackBufferWidth)
                _carPosition1.X = 0;
            if (_carPosition2.X + _carTexture2.Width > _graphics.PreferredBackBufferWidth)
                _carPosition2.X = 0;

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

            _spriteBatch.Draw(_carTexture1, _carPosition1, Color.White);
            _spriteBatch.Draw(_carTexture2, _carPosition2, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        static float RandomCarSpeed(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }
    }
}
