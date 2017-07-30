using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Pinpon.Device
{
    class Renderer
    {
        private ContentManager contentManager; // コンテンツ管理者
        private GraphicsDevice graphicsDevice; // グラフィック機器
        private SpriteBatch spriteBatch; // スプライト一括処理
        private Dictionary<string, Texture2D> textures
            = new Dictionary<string, Texture2D>(); // Dictionaryによる画像の管理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }
        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath"></param>
        public void LoadTexture(string name, string filepath = "./Texture/")
        {
            if (textures.ContainsKey(name)) // すでに登録されているか
            {
                //DEBUGモードなら
#if DEBUG
                System.Console.WriteLine("この" + name + "はKeyですでに登録しています");
#endif
                return; //処理終了 
            }
            textures.Add(name, contentManager.Load<Texture2D>(filepath + name)); // Dictionaryに追加
        }

        /// <summary>
        /// 画像の登録
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="texture"></param>
        public void LoadTexture(string name, Texture2D texture)
        {
            if (textures.ContainsKey(name)) // すでに登録されているか
            {
                //DEBUGモードなら
#if DEBUG
                System.Console.WriteLine("この" + name + "はKeyですでに登録しています");
#endif
                return; //処理終了 
            }
            textures.Add(name, texture); // Dictionaryに追加

        }

        public void UnLoad()
        {
            textures.Clear(); // 登録情報をクリア
        }
        /// <summary>
        /// 描画開始 
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();
        }
        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        ///描画処理 
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">描画位置</param>
        /// <param name="Alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, float Alpha = 1.0f)
        {
            // 登録されていなければエラーメッセージ表示
            Debug.Assert(textures.ContainsKey(name),
                "アセット名が間違っていませんか？\n" +
                "大文字小文字を間違えていませんか？\n" +
                "LoadTextureで読み込んでいますか？\n" +
                "プログラムを確認してください" + name);
            spriteBatch.Draw(textures[name], position, Color.White * Alpha);
        }

        /// <summary>
        /// 画像の表示（範囲指定）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">画像の切り出し範囲</param>
        /// <param name="Alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, Rectangle rect, float Alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(name),
                "アセット名が間違っていませんか？\n" +
                  "大文字小文字を間違えていませんか？\n" +
                   "LoadTextureで読み込んでいますか？\n" +
                    "プログラムを確認してください\n");
            spriteBatch.Draw(textures[name], position, rect, Color.White * Alpha);
        }

        public void DrawTexture(string name, Vector2 position, Vector2 scale, float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(name),
               "アセット名が間違っていませんか？\n" +
                "大文字小文字を間違えていませんか？\n" +
                "LoadTextureで読み込んでいますか？\n" +
                    "プログラムを確認してください");
            spriteBatch.Draw(
                textures[name], //アセット名
                position, // 位置
                null, // 切り取り範囲
                Color.White * alpha, // 透過度
                0.0f, // 回転
                Vector2.Zero, // 回転軸
                scale, // 拡大縮小
                SpriteEffects.None, // 表示反転効果 
                0.0f // スプライト表示深度
                );
        }

        public void DrawTexture(string name, Vector2 position, Rectangle rect, float rotation, float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(name),
               "アセット名が間違っていませんか？\n" +
                "大文字小文字を間違えていませんか？\n" +
                "LoadTextureで読み込んでいますか？\n" +
                    "プログラムを確認してください");
            spriteBatch.Draw(
                textures[name], //アセット名
                position, // 位置
                rect, // 切り取り範囲
                Color.White * alpha, // 透過度
                rotation, // 回転
                Vector2.Zero, // 回転軸
                1.0f,// 拡大縮小
                SpriteEffects.None, // 表示反転効果 
                0.0f // スプライト表示深度
                );
        }

        /// <summary>
        /// 数字の描画簡易版
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">表示させる数字</param>
        /// <param name="Alpha">透明度</param>
        public void DrawNumber(string name, Vector2 position, int number, float Alpha = 1.0f)
        {
            // 登録されていなければエラーメッセージ表示
            Debug.Assert(textures.ContainsKey(name),
                "アセット名が間違っていませんか？\n" +
                "大文字小文字を間違えていませんか？\n" +
                "LoadTextureで読み込んでいますか？\n" +
                "プログラムを確認してください");
            if (number < 0)
            {
                number = 0; // 0未満をなくす
            }
            foreach (var n in number.ToString())
            {
                spriteBatch.Draw(textures[name], position,
                    new Rectangle((n - '0') * 32, 0, 32, 64), Color.White * Alpha);
                position.X += 32;
            }
        }
        /// <summary>
        /// 数字の描画詳細版
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">表示させる数字（文字列）</param>
        /// <param name="digit">桁数</param>
        /// <param name="Alpha">透明度</param>
        public void DrawNumber(string name, Vector2 position, Color color, string number, int digit, float scale, float Alpha = 1.0f)
        {
            // 登録されていなければエラーメッセージ表示
            Debug.Assert(textures.ContainsKey(name),
                "アセット名が間違っていませんか？\n" +
                "大文字小文字を間違えていませんか？\n" +
                "LoadTextureで読み込んでいますか？\n" +
                "プログラムを確認してください");
            for (int i = 0; i < digit; i++)
            {
                if (number[i] == '.')
                {
                    spriteBatch.Draw(textures[name], position,
                        new Rectangle(10 * 32, 0, 32, 64), color * Alpha, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    char n = number[i];
                    spriteBatch.Draw(textures[name],
                        position, new Rectangle((n - '0') * 32, 0, 32, 64), color * Alpha, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);

                }
                position.X += 32;
            }
        }
    }
}
