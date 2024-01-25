using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Game1 : Game
    {
        private Texture2D spaceShip, bulletTexture, alienShip;
        private Vector2 shipPos, alienPos;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Alien> _alienList;
        private SpriteFont _mainFont;
        private bool _beginGame = false;
        Ship ship;
        Alien alien;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 896;
            //_graphics.IsFullScreen = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            shipPos = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - 100f);
            alienPos = new Vector2(42f, 100f);
            _alienList = new List<Alien>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceShip = Content.Load<Texture2D>("spaceship");
            bulletTexture = Content.Load<Texture2D>("bullet");
            alienShip = Content.Load<Texture2D>("alien");
            _mainFont = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic
            if (_beginGame == true)
            {
                ship.MoveShip(gameTime);
                ship.UpdateBullets(gameTime, _alienList);
                foreach (Alien alien in _alienList)
                {
                    alien.AlienActions(gameTime);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            MainMenu();
            if (_beginGame == true)
            {
                ship.Draw(_spriteBatch);
                foreach (Alien alien in _alienList)
                {
                    alien.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        void MainMenu()
        {
            if (_beginGame == false)
            {
                _spriteBatch.DrawString(_mainFont, "You are the ship at the bottom\nYou need to kill the aliens\nControls: A/D = move, spacebar = shoot\n\nPress ENTER to start", new Vector2(380,400), Color.White);

                KeyboardState kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Enter))
                {
                    LoadStartingContent();
                    _beginGame = true;
                }
            }

        }
        /// <summary>
        /// makes a new ship and multiple aliens
        /// </summary>
        void LoadStartingContent()
        {
            ship = new Ship(spaceShip, bulletTexture, shipPos, _mainFont, 200f, _graphics);

            for (int i = 0; i < 11; i++)
            {
                alien = new Alien(alienShip, bulletTexture, alienPos, _graphics, ship);
                alienPos.X += 90f;
                _alienList.Add(alien);
            }
        }
    }
}