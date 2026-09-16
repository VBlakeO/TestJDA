using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance;
    #endregion

   [SerializeField] private bool saveEnabled = true;
   public float startMoney = 2000f;
   [Space]
   
   [SerializeField] private Hud_Manager hudManager = null;
   [SerializeField] private EmployeeManager employeeManager = null;
   [SerializeField] private NewspaperManager newspaperManager = null;
   [Space]

    public List<Employee> employeeList = new();
    public List<SalesSystem> salesSystem = new();
    public EmployeeCard[] employeeCards = null;

    public static UnityAction OnFinishLoad = null;

    private void Awake() {
        Instance = this;
        SavableGameData.Init();
    }

    private void Start() {
       
        GameManager.OnQuitApplication += PrepareToSave;

        if (SaveSystem.MainSaveExists())
            LoadGame();
        else
            OnFinishLoad?.Invoke();
    }

    public void PrepareToSave()
    {
        SavableGameData.employeeId = new int[employeeList.Count];
        SavableGameData.copiesSold = new int[salesSystem.Count];

        for (int i = 0; i < salesSystem.Count; i++)
           SavableGameData.copiesSold[i] = salesSystem[i].RemainingCopies;

        for (int i = 0; i < employeeList.Count; i++)
           SavableGameData.employeeId[i] = employeeList[i].id;

        if (saveEnabled)
            SaveGame();
    }

    private void SaveGame()
    {
        SaveSystem.SaveMainGame();
        GameManager.OnQuitApplication -= PrepareToSave;
    }

    private void LoadGame()
    {
        SaveGameData data = SaveSystem.LoadMainGame();

        SavableGameData.copiesSold = data.copiesSold;
        SavableGameData.employeeId = data.employeeId;
        SavableGameData.reputation = data.reputation;
        SavableGameData.CurrentMoney = data.currentMoney;

        SavableGameData.gameName = data.gameName;
        SavableGameData.gamePrice = data.gamePrice;
        SavableGameData.gameReady = data.gameReady;
        SavableGameData.gamePublished = data.gamePublished;
        SavableGameData.gameByte = data.gameByte;
        SavableGameData.savadGamesInfo = data.savadGamesInfo;
        SavableGameData.estimatedPrice = data.estimatedPrice;
        SavableGameData.estimatedReputation = data.estimatedReputation ?? new List<float>();

        SavableGameData.studioName = data.studioName;
        SavableGameData.studioByte = data.studioByte;

        SavableGameData.gameSalePrice = data.gameSalePrice;
        SavableGameData.necessaryProgress = data.necessaryProgress;

        SavableGameData.hiredMarketing = data.hiredMarketing;
        SavableGameData.activatedMarketing = data.activatedMarketing;
        SavableGameData.marketingValue = data.marketingValue;

        SavableGameData.newspaperStatusForGame = data.newspaperStatusForGame;

        SavableGameData.readySelection = data.readySelection;
        SavableGameData.selectedCharacter = data.selectedCharacter;

        SavableGameData.droneRewardReceived = data.droneRewardReceived;
        SavableGameData.basketBallRewardReceived = data.basketBallRewardReceived;
        SavableGameData.bestBossRewardReceived = data.bestBossRewardReceived;
    
        hudManager.UpdateMoneyText();
        InstantiateSavedFiles();

        OnFinishLoad?.Invoke();
    }

    private void InstantiateSavedFiles()
    {
        for (int i = 0; i < SavableGameData.copiesSold.Length; i++)
            InstantiateSales(SavableGameData.gamePrice[i], SavableGameData.copiesSold[i]);

        for (int i = 0; i < SavableGameData.employeeId.Length; i++)
            InstantiateEmployee(SavableGameData.employeeId[i]);  

        newspaperManager.LoadNewspapers();
    }

    public void InstantiateSales(float price, int sales)
    {
        var sale = (GameObject)Instantiate(Resources.Load("SalesSystem"));
        SalesSystem m_salesSystem = sale.GetComponent<SalesSystem>();
        
        salesSystem.Add(sale.GetComponent<SalesSystem>());
        m_salesSystem.StartCoroutine(m_salesSystem.Sale(price, sales));
    }

    // RestoreEmployee already registers the employee in employeeList, so it must not be added here again
    public void InstantiateEmployee(int id)
    {
        GameObject _employee = employeeManager.RestoreEmployee(id);

        if (_employee == null)
            return;

        if (Programming.Instance.IsProgrammingAllowed())
            _employee.GetComponent<Employee>().StartWorking();

        employeeCards[id].hiredEmployee = true;
    }
} 

public static class SavableGameData
{
    [Header("GamePlayData")]
    public static int[] copiesSold; // Número de copias vendidas.
    public static int[] employeeId; // Identificação de funcionarios contratados.
    public static float reputation; // Reputação atual do jogador.
    public static float currentMoney; // Valor monetario atual do jogador.

    [Header("PcData")]

    public static List<string> gameName; // Lista de nomes dos jogos criados.
    public static List<float> gamePrice; // Lista de preços dos jogos criados.
    public static List<bool> gameReady; // Lista de jogos prontos ou não. 
    public static List<bool> gamePublished; // Lista de jogos publicados.
    public static List<byte[]> gameByte; // Lista de texturas byte.
    public static List<int[]> savadGamesInfo; // Lista de texturas byte.
    public static List<int> estimatedPrice; // Lista de preços estimados de jogo.
    public static List<float> estimatedReputation; // Estimated reputation of each game, same index as gameName
    public static List<bool> newspaperStatusForGame;// Lista de jogos publicados.

    public static string studioName; // Nome do Estudio
    public static byte[] studioByte;

    public static float gameSalePrice; // Valor do ultimo jogo vendido.
    public static float necessaryProgress; // progresso necessário para conclusão de um jogo

    public static UnityAction OnWithdrawMoney; // Evento chamado ao gastar dinheiro
    public static UnityAction OnDepositMoney; // Evento chamado ao receber dinheiro
    public static UnityAction<float> OnMoneyChange; // Evento chamado ao receber dinheiro

    public static bool[] hiredMarketing; // Array de marketings contratados.
    public static bool[] activatedMarketing; // Array de marketings contratados.
    public static float[] marketingValue = new float[4] {0.03f, 0.08f, 0.13f, 0.19f};

    public static bool readySelection;
    public static int selectedCharacter;

    public static bool droneRewardReceived = false;
    public static bool basketBallRewardReceived = false;
    public static bool bestBossRewardReceived = false;

    public static float CurrentMoney
    {
        get { return currentMoney; }
        set
        {
            if (value < 0)
                value = 0;

            currentMoney = value;
        }
    }

    public static void Init()
    {
        copiesSold = new int[0]; // inicializa os arrays com um tamanho padrão
        employeeId = new int[0];
        hiredMarketing = new bool[4];
        activatedMarketing = new bool[4];

        gameName = new List<string>();
        gamePrice = new List<float>();
        gameReady = new List<bool>();
        gamePublished = new List<bool>();
        gameByte = new List<byte[]>();
        savadGamesInfo = new List<int[]>();
        estimatedPrice = new List<int>();
        estimatedReputation = new List<float>();
        newspaperStatusForGame = new List<bool>();

        studioName = "Default";
        studioByte = null;

        readySelection = false;
        selectedCharacter = 0;

        CurrentMoney = SaveManager.Instance.startMoney;
    }

    public static void DepositMoney(float value)
    {
        currentMoney += value;
        OnDepositMoney?.Invoke();
        OnMoneyChange?.Invoke(CurrentMoney);
    }

    public static void WithdrawMoney(float value)
    {
        CurrentMoney -= value;
        OnWithdrawMoney?.Invoke();
        OnMoneyChange?.Invoke(CurrentMoney);
    }
}

public static class KnowledgeManager
{
    public static float[] employeePayment = new float[6] {1500f, 2000f, 2500f, 3000f, 3500f, 4000f};
}