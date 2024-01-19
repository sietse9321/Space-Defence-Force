using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders
{
    internal class Ship
    {
        private Texture2D _ship;
        private Texture2D _bulletTexture;
        private int _health = 3;
        private Vector2 _position;
        private float _speed;
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private float _delay = 1;

        private List<Bullet> _bullets; // Add this field

        public Ship(Texture2D ship, Texture2D bulletTexture, Vector2 position, float speed, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _ship = ship;
            _position = position;
            _speed = speed;
            _spriteBatch = spriteBatch;
            _graphics = graphics;
            _bulletTexture = bulletTexture;

            _bullets = new List<Bullet>(); // Initialize the list
        }

        public void MoveShip(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.A))
            {
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //if ship pos is bigger then the screen set it as the position
            if (_position.X > _graphics.PreferredBackBufferWidth - _ship.Width / 2)
            {
                _position.X = _graphics.PreferredBackBufferWidth - _ship.Width / 2;
            }
            else if (_position.X < _ship.Width / 2)
            {
                _position.X = _ship.Width / 2;
            }
            if (kstate.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalSeconds > _delay)
            {
                ShootBullet(_bulletTexture);
                _delay = 0.5f + (float)gameTime.TotalGameTime.TotalSeconds;
            }
        }
        public void ShootBullet(Texture2D bulletTexture)
        {
            // Create a new instance of Bullet and add it to the list
            Bullet newBullet = new Bullet(bulletTexture, new Vector2(_position.X, _position.Y - (_ship.Height / 2)), 200f,SpriteEffects.None);
            _bullets.Add(newBullet);
        }
        public void UpdateBullets(GameTime gameTime)
        {
            // Update all bullets in the list
            foreach (Bullet bullet in _bullets.ToList())
            {
                bullet.Update(gameTime);

                // Remove bullets that are off-screen
                if (bullet.Position.Y < 0 || bullet.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    _bullets.Remove(bullet);
                    Console.WriteLine("removing bullet");
                }
            }
        }
        public void Draw(SpriteBatch pSpriteBatch)
        {
            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw(pSpriteBatch);
            }
            pSpriteBatch.Draw(_ship, _position, null, Color.White, 0f, new Vector2(_ship.Width / 2, _ship.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}