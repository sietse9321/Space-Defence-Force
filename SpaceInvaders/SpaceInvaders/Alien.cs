using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    internal class Alien
    {
        private Texture2D _alien;
        private Texture2D _bulletTexture;
        private Vector2 _position;
        private float _delay = 1;
        private GraphicsDeviceManager _graphics;

        private List<Bullet> _bullets;

        public Alien(Texture2D alien, Texture2D bulletTexture, Vector2 position, GraphicsDeviceManager grapics)
        {
            _alien = alien;
            _bulletTexture = bulletTexture;
            _position = position;
            _graphics = grapics;

            _bullets = new List<Bullet>();
        }
        public Vector2 Position
        {
            get { return _position; }
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
            int randomShoot = rnd.Next(1,100);
            if (gameTime.TotalGameTime.TotalSeconds > _delay && randomShoot == 2)
            {
                ShootBullet(_bulletTexture);
                _delay = 5f + (float)gameTime.TotalGameTime.TotalSeconds;
                Console.WriteLine("spawning new bullet alien");
            }
            UpdateBullets(gameTime);
        }

        public void ShootBullet(Texture2D bulletTexture)
        {
            Bullet newBullet = new Bullet(bulletTexture, new Vector2(_position.X, _position.Y + (_alien.Height / 2)), -200f,SpriteEffects.FlipVertically);
            _bullets.Add(newBullet);
        }
        public void UpdateBullets(GameTime gameTime)
        {
            // Update all bullets in the list
            foreach (Bullet bullet in _bullets.ToList())
            {
                bullet.Update(gameTime);

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
