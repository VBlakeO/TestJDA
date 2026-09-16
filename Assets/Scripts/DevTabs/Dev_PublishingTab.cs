using UnityEngine.UI;
using UnityEngine;

public class Dev_PublishingTab : DevelopmentTabs
{
    public ScrollRect scrollRect = null;
    [SerializeField] private RectTransform GameBoardRect = null;
    [Space]

    [SerializeField] private GameObject gameInfoPrefab = null;
    [SerializeField] private int ii = 0;

    public Sprite[] GameSprites = new Sprite[0];
    public Texture2D[] GameTexture {get; set;}

    public void LoadSprites()
    {
        GameTexture = null;
        GameTexture = new Texture2D[SavableGameData.gameByte.Count];
        GameSprites = new Sprite[0];
        GameSprites = new Sprite[GameTexture.Length];

        ii = SavableGameData.gameByte.Count;

        for (int i = 0; i < GameTexture.Length; i++)
        {
            GameTexture[i] = new Texture2D(1, 1);
            GameTexture[i].LoadImage(SavableGameData.gameByte[i]);
            GameSprites[i] = Sprite.Create(GameTexture[i], new Rect(0, 0, GetScreenSize(), GetScreenSize()), new Vector2(0, 0), .01f);
        }

       NewDevOp.Instance.UpdateBigScreen();
    }

    private int GetScreenSize() => (int)(Screen.width * (26.67f / 100f));

    void Start()
    {
        //LoadSprites();
    }

    public void ClearAllGames()
    {
       foreach (Transform child in GameBoardRect.transform)
       {
           Destroy(child.gameObject);
       }
    }

    public void OpenSavedGames()
    {
        LoadSprites();
        ClearAllGames();

        for (int i = 0; i < SavableGameData.gameName.Count; i++)
        {
            var gamme = Instantiate(gameInfoPrefab, GameBoardRect);
            GameInfo gameInfo = gamme.GetComponent<GameInfo>();

            gameInfo.gameIndex = i;
            if (GameSprites.Length > 0 && i < GameSprites.Length)
                gameInfo.gameImg.sprite = GameSprites[i];
            gameInfo.gameIsReady = SavableGameData.gameReady[i];
            gameInfo.gameNameText.text = SavableGameData.gameName[i];
            gameInfo.ShowGamePanel(SavableGameData.gamePublished[i]);
        }
    }
}