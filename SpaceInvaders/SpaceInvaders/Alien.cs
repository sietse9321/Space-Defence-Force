using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace SpaceInvaders
{
    internal class Alien
    {
        private Texture2D _alien;
        private Texture2D _bulletTexture;
        private Vector2 _position;
        private float _shootDelay = 1;
        private float _moveDelay = 4;
        private float _elapsedTime;
        private bool _right = true;
        private GraphicsDeviceManager _graphics;
        private Ship _ship;

        private List<Bullet> _bullets;

        public Alien(Texture2D alien, Texture2D bulletTexture, Vector2 position, GraphicsDeviceManager grapics,Ship ship)
        {
            _alien = alien;
            _bulletTexture = bulletTexture;
            _position = position;
            _graphics = grapics;
            _ship = ship;

            _bullets = new List<Bullet>();
        }
        public Vector2 Position
        {
            get { return _position; }
        }
        private bool CheckCollision(Bullet bullet, Ship ship)
        {
            Rectangle bulletRect = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, 8, 8);
            Rectangle shipRect = new Rectangle((int)ship.Position.X - (40 / 2), (int)ship.Position.Y - (40 / 2), 40, 40);

            return bulletRect.Intersects(shipRect);
        }
        public void Draw(SpriteBatch pSpriteBatch)
        {
            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw(pSpriteBatch);
            }
            pSpriteBatch.Draw(_alien, _position, null, Color.White, 0f, new Vector2(_alien.Width / 2, _alien.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }

        public void AlienActions(GameTime gameTime)
        {
            Random rnd = new Random();
            int randomShoot = rnd.Next(1, 100);
            if (gameTime.TotalGameTime.TotalSeconds > _shootDelay && randomShoot == 2)
            {
                ShootBullet(_bulletTexture);
                _shootDelay = 5f + (float)gameTime.TotalGameTime.TotalSeconds;
                Console.WriteLine("spawning new bullet alien");
            }
            UpdateBullets(gameTime);

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _moveDelay && _right)
            {
                // Move to the right
                _position.X += 14 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {

                _right = false;

                _position.X -= 14 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            // Reset the counter if needed
            if (_elapsedTime >= _moveDelay * 2)
            {
                _elapsedTime = 0f;
                _right = true;
            }

        }

        public void ShootBullet(Texture2D bulletTexture)
        {
            Bullet newBullet = new Bullet(bulletTexture, new Vector2(_position.X, _position.Y + (_alien.Height / 2)), -200f, SpriteEffects.FlipVertically);
            _bullets.Add(newBullet);
        }
        public void UpdateBullets(GameTime gameTime)
        {
            // Update all bullets in the list
            foreach (Bullet bullet in _bullets.ToList())
            {
                bullet.Update(gameTime);

                if (CheckCollision(bullet, _ship))
                {
                    _bullets.Remove(bullet);
                    Console.WriteLine("Bullet hit an alien!");

                    _ship.SetHealth = 1;
                    Console.WriteLine("Removed alien");
                }

                // Remove bullets that are off-screen
                if (bullet.Position.Y < 0 || bullet.Position.Y >= _graphics.PreferredBackBufferHeight)
                {
                    _bullets.Remove(bullet);
                    Console.WriteLine("removing bullet");
                }
            }
        }
    }
}
