using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace SpaceInvaders
{
    internal class Bullet
    {
        private Texture2D _bulletTexture;
        private Vector2 _position;
        private float _speed;
        private SpriteEffects _spriteEffects;

        public Bullet(Texture2D bulletTexture, Vector2 position, float speed, SpriteEffects spriteEffects)
        {
            _bulletTexture = bulletTexture;
            _position = position;
            _speed = speed;
            _spriteEffects = spriteEffects;
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public void Update(GameTime gameTime)
        {
            //! remove
            Console.WriteLine(_position);
            // Update the bullet's position based on its speed
            _position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the bullet at its current position
            spriteBatch.Draw(_bulletTexture, _position, null, Color.White, 0f, new Vector2(_bulletTexture.Width / 2, _bulletTexture.Height / 2), Vector2.One, _spriteEffects, 0f);

        }
    }
}