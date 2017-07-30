using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pinpon.Device
{
    class InputState
    {
        //フィールド
        private Vector2 p1Velocity = Vector2.Zero; // PL1の移動量の宣言と生成
        private Vector2 p2Velocity = Vector2.Zero; // PL2の移動量の宣言と生成

        private KeyboardState currentKey; // 現在のキー
        private KeyboardState previousKey; // 1つ前のキー

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputState() { }

        /// <summary>
        /// PL1の移動量の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 P1Velocity()
        {
            return p1Velocity;
        }

        /// <summary>
        /// PL2の移動量の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 P2Velocity()
        {
            return p2Velocity;
        }

        /// <summary>
        /// 移動量の更新
        /// </summary>
        /// <param name="keyState"></param>
        public void UpdateVelocity(KeyboardState keyState)
        {
            p1Velocity = Vector2.Zero; // 毎ループ初期化
            p2Velocity = Vector2.Zero;
            //PL1の入力処理
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                p2Velocity.Y -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                p2Velocity.Y += 1.0f;   
            }

            //PL2の入力処理
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                p1Velocity.Y -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                p1Velocity.Y += 1.0f;
            }


            //velocityの長さが0でない時に正規化する ∴移動している時
            if (p1Velocity.Length() != 0.0f)
            {
                p1Velocity.Normalize();
            }
            if (p2Velocity.Length() != 0.0f)
            {
                p2Velocity.Normalize(); // 正規化メソッド
            }
        }

        /// <summary>
        /// キーの更新
        /// </summary>
        /// <param name="keyState"></param>
        public void UpdateKey(KeyboardState keyState)
        {
            previousKey = currentKey;
            currentKey = keyState;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            var keyState = Keyboard.GetState(); // keystateに所得したキーボード入力を代入
            UpdateVelocity(keyState); // ↑keyStateを引数にVelocityUpdateを実行し、更新する
            UpdateKey(keyState);
        }

        /// <summary>
        /// キーが押されているか
        /// </summary>
        /// <param name="key">調べるキー</param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            //現在チェックしたいキーが押されているか
            bool current = currentKey.IsKeyDown(key);
            //１フレーム前に押されていたか
            bool previous = previousKey.IsKeyDown(key);
            //currentがtrueでpreviousがfalseならtrue
            return current && !previous;
        }

        /// <summary>
        /// キー入力のトリガー判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns>１フレーム前に押されていたらtrue</returns>
        public bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }

        /// <summary>
        /// キー入力の状態判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns>おされていたら</returns>
        public bool GetKeyState(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }
    }
}
