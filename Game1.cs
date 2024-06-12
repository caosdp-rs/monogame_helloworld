using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ProjectName;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D target_Sprite;
    Texture2D crosshairs_Sprite;
    Texture2D background_Sprite;
    Texture2D buttonTexture;
    SpriteFont game_Font;
    Vector2 targetPosition = new Vector2(300, 300);
    int TARGET_RADIUS = 45;
    MouseState mState;
    bool mReleased = true;
    float mouseTargetDist;
    int score = 0;
    float timer = 10f;
    int acerto = 0;
    private Rectangle restartButton;
    private bool showRestartButton;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        restartButton = new Rectangle(150, 0, 150, 80);
        showRestartButton = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        target_Sprite = Content.Load<Texture2D>("target");
        crosshairs_Sprite = Content.Load<Texture2D>("crosshairs");
        background_Sprite = Content.Load<Texture2D>("sky");
        game_Font = Content.Load<SpriteFont>("galleryFont");
        buttonTexture = Content.Load<Texture2D>("RestartHoverSprite");


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (timer > 0)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            timer = 0;
            showRestartButton = true;
        }
        mState = Mouse.GetState();


        mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));

        if (mState.LeftButton == ButtonState.Pressed  && mReleased == true)
        {
            // Verificar se o clique foi dentro do botão de reinício
            if (showRestartButton && restartButton.Contains(mState.Position))
            {
                // Reiniciar o jogo
                score = 0;
                timer = 10f; // Reiniciar o cronômetro

                // Definir nova posição inicial do alvo
                Random rand = new Random();
                targetPosition = new Vector2(
                    rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1),
                    rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1)
                );

                // Esconder o botão de reinício
                showRestartButton = false;
            }
        }

        if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
        {
            if (mouseTargetDist < TARGET_RADIUS && timer > 0)
            {
                float maxDistance = TARGET_RADIUS; // Distância máxima do centro que ainda conta pontos
                float scoreMultiplier = 1.0f; // Multiplicador de pontuação, pode ajustar conforme necessário

                // A pontuação será maior quanto mais próximo do centro
                int additionalScore = (int)((maxDistance - mouseTargetDist) * scoreMultiplier);
                score += additionalScore;
                acerto = additionalScore;
                //score++;
                Random rand = new Random();
                targetPosition.X = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                targetPosition.Y = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
            }
            Debug.WriteLine("Score: " + score);
            mReleased = false;
        }

        if (mState.LeftButton == ButtonState.Released)
        {
            mReleased = true;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(background_Sprite, Vector2.Zero, Color.White);
        if (timer > 0)
        {
            _spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
        }
        _spriteBatch.DrawString(game_Font, "Score:" + score.ToString(), new Vector2(3, 3), Color.Red);
        _spriteBatch.DrawString(game_Font, "Time:" + Math.Ceiling(timer).ToString(), new Vector2(3, 40), Color.White);
        _spriteBatch.DrawString(game_Font, "Acerto:" + acerto.ToString(), new Vector2(3, 200), acerto < 25 ? Color.Red : Color.Blue);
        

        if (showRestartButton)
        {
            _spriteBatch.Draw(buttonTexture, restartButton, Color.White);
        }
        _spriteBatch.Draw(crosshairs_Sprite, new Vector2(mState.X - 25, mState.Y - 25), Color.White);
        
        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
