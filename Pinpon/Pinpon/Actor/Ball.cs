using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pinpon.Device;

namespace Pinpon.Actor
{
    //Ballの向き
    enum Direction
    {
        RIGHTDOWN, RIGHTUP, LEFTDOWN, LEFTUP
    }

    class Ball : GameObject
    {
        private Vector2 velocity; // 速度

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">初期位置</param>
        public Ball(Vector2 position) : base("ball")
        {
            this.position = position; // 初期位置
            width = 32; height = 32; // サイズ
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            velocity = new Vector2(-1, 1); // 初期移動量
            speed = 5.0f; // 初期スピード
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            speed += 0.015f; // スピードの上昇
            velocity.Normalize(); // 移動量の正規化

            position += velocity * speed; // 移動処理
        }

        /// <summary>
        /// 衝突判定
        /// </summary>
        /// <param name="rect">対象の矩形情報</param>
        /// <param name="delta">差分</param>
        /// <returns></returns>
        public bool IsCollision(Rectangle rect, Vector2 delta)
        {
            var center = GetRectangle(delta).Center;
            Vector2 closet = new Vector2(MathHelper.Clamp(center.X, rect.Left, rect.Right),
                MathHelper.Clamp(center.Y, rect.Top, rect.Bottom));

            float distanceSquared = Vector2.DistanceSquared(closet, new Vector2(center.X, center.Y));
            return (distanceSquared < (16 * 16));
        }

        /// <summary>
        /// X反射処理
        /// </summary>
        /// <param name="revisionX"></param>
        public void ReflectX(float revisionX)
        {
            velocity.X *= -1;
            position.X -= revisionX;
        }

        /// <summary>
        /// Y反射処理
        /// </summary>
        /// <param name="revisionY"></param>
        public void ReflectY(float revisionY)
        {
            velocity.Y *= -1;
            position.Y -= revisionY;
        }

        /// <summary>
        /// Y補正
        /// </summary>
        /// <param name="revisionY"></param>
        public void RevisionY(float revisionY)
        {
            position.Y += revisionY;
        }

        /// <summary>
        /// X補正
        /// </summary>
        /// <param name="revisionX"></param>
        public void RevisionX(float revisionX)
        {
            position.X += revisionX;
        }

        /// <summary>
        ///現在の向きの取得
        /// </summary>
        /// <returns></returns>
        public Direction GetCurrentDirection()
        {
            if (velocity.X > 0 && velocity.Y > 0)
            {
                return Direction.RIGHTDOWN;
            }
            else if (velocity.X > 0 && velocity.Y < 0)
            {
                return Direction.RIGHTUP;
            }
            else if (velocity.X < 0 && velocity.Y > 0)
            {
                return Direction.LEFTDOWN;
            }
            else
            {
                return Direction.LEFTUP;
            }
        }

        /// <summary>
        /// 差分込みの矩形情報の取得
        /// </summary>
        /// <param name="delta">差分</param>
        /// <returns></returns>
        public Rectangle GetRectangle(Vector2 delta)
        {
            return new Rectangle((int)(position.X + delta.X), (int)(position.Y + delta.Y), 32, 32);
        }

        /// <summary>
        /// 移動量の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMovement()
        {
            return velocity * speed;
        }

        /// <summary>
        /// 位置の設定
        /// </summary>
        /// <param name="setPosition"></param>
        public void SetPosition(Vector2 setPosition)
        {
            position = setPosition;
        }
    }
}

