using System.Collections;
using UnityEngine;

public class NewspaperManager : MonoBehaviour
{
    [SerializeField] private GameObject NewsPaperPrefab = null;
    [SerializeField] private Transform spawnPoint = null;
    [Space]

    [SerializeField] private NewDevOp devOp = null;
    [SerializeField] private Dev_PublishingTab devPublishingTab = null;

    private Sprite logo = null;
    private string description = null;

    // Start is called before the first frame update
    void Start()
    {
        devOp = NewDevOp.Instance;
        devOp.OnSellingGame += PublishTheLastReleasedGame;
    }

    public void LoadNewspapers()
    {
        StartCoroutine(InstantiateLoadedNewspapers());
    }

    void PublishTheLastReleasedGame()
    {
        SavableGameData.newspaperStatusForGame.Add(SavableGameData.activatedMarketing[0]);

        if (!SavableGameData.activatedMarketing[0])
            return;
            
        devPublishingTab.LoadSprites();

        logo = devPublishingTab.GameSprites[SavableGameData.gameName.Count - 1];
        description = devOp.GetGameDescription(SavableGameData.savadGamesInfo.Count - 1);

        InstantiateNewsPaper(logo, description);
    }

    // Update is called once per frame
    void InstantiateNewsPaper(Sprite _logo, string _description)
    {        
        GameObject npg = Instantiate(NewsPaperPrefab, spawnPoint.position, spawnPoint.rotation);
        Newspaper np = npg.GetComponent<Newspaper>();

        np.UpdateInfo(_logo, _description);
    }

    IEnumerator InstantiateLoadedNewspapers()
    {
        WaitForSeconds wfs = new(0.3f);
        devPublishingTab.LoadSprites();

        int z  = 0;

        if (SavableGameData.savadGamesInfo.Count > 5 && SavableGameData.activatedMarketing[0])
            z = SavableGameData.savadGamesInfo.Count - 6;

        for (int i = z; i < SavableGameData.savadGamesInfo.Count; i++)
        {
            if(SavableGameData.newspaperStatusForGame.Count == 0)
                break;

            if (SavableGameData.newspaperStatusForGame[i])
            {
                description = devOp.GetGameDescription(i);
                logo = devPublishingTab.GameSprites[i];

                InstantiateNewsPaper(logo, description);
                yield return wfs;
            }
        }
    }
}
