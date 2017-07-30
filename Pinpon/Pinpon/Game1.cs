using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pinpon.Actor;
using Pinpon.Device;
using Pinpon.Scene;

namespace Pinpon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsDeviceManager; // グラフィック管理
        private ContentManager contentManager; // コンテンツ管理
        private Renderer renderer;
        private GameDevice gameDevice;
        private SceneManager sceneManager;
        private Sound sound;

        private static Random rand;
        private Vector2 rocketPosition;
        private Vector2 rocketVelocity;
        private bool rocketDirection = true; // 正の向きがtrue
        private Vector2 sidewaysCharacterPosition;//横向きオブジェクトの位置
        private Vector2 sidewaysCharacterVelocity;// 横向きオブジェクトの移動量

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            contentManager = Content;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.height; // 画面縦幅
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.width; // 画面横幅
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //壁の生成
            gameDevice = new GameDevice(Content, GraphicsDevice);
            renderer = gameDevice.GetRenderer();
            sound = gameDevice.GetSound();


            sceneManager = new SceneManager();
            sceneManager.Add(Scene.Scene.Title, new Title(gameDevice));

            IScene gamePlay = new GamePlay(gameDevice);
            sceneManager.Add(Scene.Scene.GamePlay, gamePlay);
            sceneManager.Add(Scene.Scene.Ending, new Ending(gameDevice, gamePlay));
            sceneManager.Change(Scene.Scene.Title);

            rand = new Random();
            rocketPosition = new Vector2(rand.Next(Screen.width - 32), -20.0f);
            rocketVelocity = new Vector2(0.0f, 2.0f);

            sidewaysCharacterPosition = new Vector2(Screen.width + 200.0f, rand.Next(Screen.height - 32));
            sidewaysCharacterVelocity = new Vector2(-2.0f, 0.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
            renderer.LoadTexture("ball1");
            renderer.LoadTexture("ball2");
            renderer.LoadTexture("ball3");
            renderer.LoadTexture("barrier1");
            renderer.LoadTexture("barrier2");
            renderer.LoadTexture("barrier3");
            renderer.LoadTexture("bird");
            renderer.LoadTexture("ending1");
            renderer.LoadTexture("ending2");
            renderer.LoadTexture("fish");
            renderer.LoadTexture("number");
            renderer.LoadTexture("player1");
            renderer.LoadTexture("player2");
            renderer.LoadTexture("rocket");
            renderer.LoadTexture("stage1");
            renderer.LoadTexture("stage2");
            renderer.LoadTexture("stage3");
            renderer.LoadTexture("start");
            renderer.LoadTexture("title");
            renderer.LoadTexture("wall1");
            renderer.LoadTexture("wall2");
            renderer.LoadTexture("wall3");
            renderer.LoadTexture("warp1");
            renderer.LoadTexture("warp2");
            renderer.LoadTexture("warp3");
            sound.LoadBGM("BGM1");
            sound.LoadBGM("BGM2");
            sound.LoadBGM("BGM3");
            sound.LoadSE("decisionse");
            sound.LoadSE("hitse");
            sound.LoadSE("hit2se");
            sound.LoadSE("resetse");
            sound.LoadSE("scenese");
            sound.LoadSE("fanfarese");
            sound.LoadSE("goalse");
            sound.LoadSE("warpse");
            sound.LoadSE("whistlese");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            renderer.UnLoad();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            //escapeキーで終了
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            gameDevice.Update(gameTime);
            sceneManager.Update(gameTime);

            //ロケットの移動処理
            rocketPosition += rocketVelocity;
            if (rocketPosition.Y > Screen.height + 400 || rocketPosition.Y < 0.0 - 400)
            {
                rocketPosition.X = rand.Next(0, Screen.width - 32);
                rocketVelocity.Y *= -1;
                rocketDirection = !rocketDirection;
            }

            //横向きキャラクターの移動処理
            sidewaysCharacterPosition += sidewaysCharacterVelocity;
            if (sidewaysCharacterPosition.X < 0.0 - 200)
            {
                sidewaysCharacterPosition.Y = rand.Next(0 + 50, Screen.height - 32 - 50);
                sidewaysCharacterPosition.X = Screen.width + 200;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            renderer.Begin();
            //背景画像の表示
            switch (StageSet.stageSet)
            {
                case 1:
                    renderer.DrawTexture("stage1", Vector2.Zero);
                    //ロケットの表示
                    if (rocketDirection)
                    {
                        renderer.DrawTexture("rocket", rocketPosition, new Rectangle(0, 0, 32, 32), (float)MathHelper.ToRadians(180), 1.0f);
                    }
                    else
                    {
                        renderer.DrawTexture("rocket", rocketPosition);
                    }
                    break;
                case 2:
                    {
                        renderer.DrawTexture("stage2", Vector2.Zero);
                        renderer.DrawTexture("fish", sidewaysCharacterPosition);
                    }
                    break;
                default:
                    {
                        renderer.DrawTexture("stage3", Vector2.Zero);
                        renderer.DrawTexture("bird", sidewaysCharacterPosition);
                    }
                    break;
            }
            renderer.End();

            sceneManager.Draw(renderer);

            base.Draw(gameTime);
        }

    }
}
