using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Game1 : Game
    {
        private Texture2D spaceShip, bulletTexture, alienShip;
        private Vector2 shipPos, alienPos;
        private Vector2 shipDirection;
        private float alienPosExtender = 100f;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Alien> _alienList;
        Ship ship;
        Alien alien;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            shipPos = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - 50f);
            alienPos = new Vector2(50f, 100f);
            _alienList = new List<Alien>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceShip = Content.Load<Texture2D>("spaceship");
            bulletTexture = Content.Load<Texture2D>("bullet");
            alienShip = Content.Load<Texture2D>("alien");
            // TODO: use this.Content to load your game content
            LoadStartingContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            ship.MoveShip(gameTime);
            ship.UpdateBullets(gameTime);
            foreach (Alien alien in _alienList)
            {
                alien.AlienActions(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            ship.Draw(_spriteBatch);
            alien.Draw(_spriteBatch);
            foreach (Alien alien in _alienList)
            {
                alien.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// makes a new ship and multiple aliens
        /// </summary>
        void LoadStartingContent()
        {
            ship = new Ship(spaceShip, bulletTexture, shipPos, 200f, _spriteBatch, _graphics);

            for (int i = 0; i < 10; i++)
            {
                alien = new Alien(alienShip, bulletTexture, alienPos, _spriteBatch, _graphics);
                alienPos.X += 100f;
                _alienList.Add(alien);
            }
        }
    }
}