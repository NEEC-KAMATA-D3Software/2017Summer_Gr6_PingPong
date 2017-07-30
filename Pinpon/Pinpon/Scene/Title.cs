using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Pinpon.Device;


namespace Pinpon.Scene
{
    class Title : IScene
    {
        private InputState input; // 入力デバイス
        private Sound sound; // 音
        private bool isEnd; // 終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice">ゲームデバイス</param>
        public Title(GameDevice gameDevice)
        {
            input = gameDevice.GetInputState(); // ゲームデバイスの取得
            sound = gameDevice.GetSound(); // 音の取得
            isEnd = false; // 終了フラグ
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEnd = false; // 終了フラグ
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //BGM再生
            sound.PlayBGM("BGM1");
            //スペースが押されたら
            if (input.IsKeyDown(Keys.Space))
            {
                //決定音再生
                sound.PlaySE("decisionse");
                //終了し、次のシーンへ
                isEnd = true;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            //タイトル画像 PLの画像の表示
            renderer.DrawTexture("title", Vector2.Zero);
            renderer.DrawTexture("player1", new Vector2(60, 200), new Vector2(1.75f, 1.75f));
            renderer.DrawTexture("player2", new Vector2(685, 200), new Vector2(1.75f, 1.75f));
            renderer.End();
        }

        /// <summary>
        /// 終了時処理
        /// </summary>
        public void Shutdown()
        {
            sound.StopBGM();
        }

        /// <summary>
        /// 終了しているか？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEnd;
        }

        /// <summary>
        /// 次のシーン名
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            return Scene.GamePlay;
        }
    }
}
