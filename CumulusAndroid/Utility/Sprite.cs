using CumulusAndroid.Utility.Tween;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusAndroid.Utility
{
    public class Sprite
    {
        public TweenPosition TPosition;
        public TweenValue TValue;
        public Texture2D Texture;
        public Vector2 Position;
        public float Scale;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Vector2 Origin => new Vector2(Texture.Width / 2, Texture.Height / 2);

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            TPosition = new TweenPosition(this);
            TValue = new TweenValue();
            Scale = 1f;
        }

        public virtual void Update(float time)
        {
            TValue.Update(time);
            TPosition.Update(time, ref Position);
        }

        public void EaseScale(float change, float duration, EaseFunction function = EaseFunction.Linear)
        {
            TValue.Move(Scale, change, duration, function, (float value) => { Scale = value; });
        }

        public virtual void Draw(SpriteBatch batch)
        {
            //batch.Draw(Texture, Position, Color.White);
            batch.Draw(Texture, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 1f);
            ////DECOMMENTER SI BESOIN DE DESSINER LES HITBOX
            //Texture2D tex = Assets.CreateTexture(hitbox.Width, hitbox.Height, new Color(255, 0, 0, 50));
            //if (tex != null)
            //    batch.Draw(tex, hitbox, Color.White);
        }
    }
}