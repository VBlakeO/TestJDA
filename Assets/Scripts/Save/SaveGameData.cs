using System.Collections.Generic;

[System.Serializable]
public class SaveGameData
{
    public int[] copiesSold; // Número de copias vendidas.
    public int[] employeeId; // Identificação de funcionarios contratados.
    public float reputation; // Reputação atual do jogador.
    public float currentMoney; // Valor monetario atual do jogador

    public List<string> gameName; // Lista de nomes dos jogos criados.
    public List<float> gamePrice; // Lista de preços dos jogos criados.
    public List<bool> gameReady; // Lista de jogos prontos ou não. 
    public List<bool> gamePublished; // Lista de jogos publicados.
    public List<byte[]> gameByte; // Lista de texturas byte.
    public List<int[]> savadGamesInfo; // Lista de texturas byte
    public List<bool> newspaperStatusForGame; // Estado da publicação em jornal durante a vendo do jogo atual.
    public List<int> estimatedPrice; // Preço estimado de cada jogo

    public string studioName; // Nome de estudio
    public byte[] studioByte;

    public float gameSalePrice; // Valor do ultimo jogo vendido.
    public float necessaryProgress; // progresso necessário para conclusão de um jog
    public bool[] hiredMarketing; // Array de marketings contratados.
    public bool[] activatedMarketing; // Array de marketings contratados.
    public float[] marketingValue = new float[4] { 0.03f, 0.08f, 0.13f, 0.19f};

    public bool readySelection;
    public int selectedCharacter;

    public bool droneRewardReceived;
    public bool basketBallRewardReceived;
    public bool bestBossRewardReceived;

    public SaveGameData()
    {
        copiesSold = SavableGameData.copiesSold;
        employeeId = SavableGameData.employeeId;
        reputation = SavableGameData.reputation;
        currentMoney = SavableGameData.CurrentMoney;

        gameName = SavableGameData.gameName;
        gamePrice = SavableGameData.gamePrice;
        gameReady = SavableGameData.gameReady;
        gamePublished = SavableGameData.gamePublished;
        gameByte = SavableGameData.gameByte;
        savadGamesInfo = SavableGameData.savadGamesInfo;
        estimatedPrice = SavableGameData.estimatedPrice;

        studioName = SavableGameData.studioName;
        studioByte = SavableGameData.studioByte;

        gameSalePrice = SavableGameData.gameSalePrice;
        necessaryProgress = SavableGameData.necessaryProgress;

        hiredMarketing = SavableGameData.hiredMarketing;
        activatedMarketing = SavableGameData.activatedMarketing;
        marketingValue = SavableGameData.marketingValue;

        newspaperStatusForGame = SavableGameData.newspaperStatusForGame;

        readySelection = SavableGameData.readySelection;
        selectedCharacter = SavableGameData.selectedCharacter;
        
        droneRewardReceived = SavableGameData.droneRewardReceived;
        basketBallRewardReceived = SavableGameData.basketBallRewardReceived;
        bestBossRewardReceived = SavableGameData.bestBossRewardReceived;
    }
}