using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameInfo : MonoBehaviour
{
    public int gameIndex = 0;
    public bool gameIsReady = false;
    [Space]

    public Image gameImg = null;
    [SerializeField] private Image backgrounImg = null;
    [Space]

    public TextMeshProUGUI gameNameText = null;
    [SerializeField] private TextMeshProUGUI gamePublishedText = null;
    [Space]

    [SerializeField] private Color[] backgrounColor;
    [Space]
    
    [SerializeField] private Button publishButton;

    public void ShowGamePanel(bool status)
    {
        if(status) // O jogo foi publicado
        {
            GameIsPublished();
        }
        else // O jogo não foi publicado
        {
            if (gameIsReady)
            {
                GameIsNotPublished();
            }
            else
            {
                gamePublishedText.text = GameManager.languageId switch
                {
                    0 => "in production",
                    1 => "em produ��o",
                    _ => "in production",
                };
                
                backgrounImg.color = backgrounColor[0];
            }
        }
    }

    void GameIsPublished()
    {
        publishButton.interactable = false;

        gamePublishedText.text = GameManager.languageId switch
        {
            0 => "published",
            1 => "publicado",
            _ => "published",
        };

        backgrounImg.color = backgrounColor[1];
    }

    void GameIsNotPublished()
    {
        publishButton.interactable = true;

        gamePublishedText.text = GameManager.languageId switch
        {
            0 => "publish",
            1 => "publicar",
            _ => "publish",
        };

        backgrounImg.color = backgrounColor[1];
        InvokeRepeating(nameof(Blink), 0f, 0.9f);
    }

    // Test
    public void SellPanel(bool active)
    {
        NewDevOp.Instance.SetGameInfo(this);
        NewDevOp.Instance.EnableSellPanel(active, gameIndex);
    }

    public void SavePublished()
    {
        CancelInvoke();
        backgrounImg.color = new Color(128, 128, 128, 0.2f);

        GameIsPublished();
        SavableGameData.gamePublished[gameIndex] = true;

        NewDevOp.Instance.buttonsManager.MyGamesButtonEvent();
    }

    public void Blink()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.6f, "to", 0.1f, "time", 0.8f, "onupdate", "blink"));
    }

    void blink(float value)
    {
        backgrounImg.color = new Color(128, 128, 128, value);
    }
}
