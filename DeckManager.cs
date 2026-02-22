using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DeckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ApiManager apiManager;

    [Header("UI General")]
    [SerializeField] private TMP_InputField playerIdInput;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject loadingScreen;

    [Header("Cartas (3)")]
    [SerializeField] private RawImage[] cardImages;
    [SerializeField] private TextMeshProUGUI[] cardNames;
    [SerializeField] private TextMeshProUGUI[] cardSpecies;

    private const int MIN_PLAYER_ID = 1;
    private const int MAX_PLAYER_ID = 17;

    private void Start()
    {
        cardPanel.SetActive(false);
        errorText.text = "";
    }

    public void EnterID()
    {
        errorText.text = ""; 

        if (!int.TryParse(playerIdInput.text, out int playerId))
        {
            ShowInvalidId();
            return;
        }

        if (playerId < MIN_PLAYER_ID || playerId > MAX_PLAYER_ID)
        {
            ShowInvalidId();
            return;
        }

        apiManager.GetPlayer(playerId, OnPlayerReceived, ShowInvalidId);
    }

    private void OnPlayerReceived(Player player)
    {
        StartCoroutine(LoadingScreen());
     
        welcomeText.text = "Bienvenido " + player.name;
        errorText.text = "";

        for (int i = 0; i < 3; i++)
        {
            int index = i;

            apiManager.GetCharacter(player.cards[i],
                character =>
                {
                    cardNames[index].text = character.name;
                    cardSpecies[index].text = character.species;

                    apiManager.GetTexture(character.image,
                        texture => cardImages[index].texture = texture);
                },
                () => Debug.Log("Error cargando personaje")
            );
        }
    }

    private void ShowInvalidId()
    {
        cardPanel.SetActive(false);
        welcomeText.text = "";
        errorText.text = "ID Invalido, ingresa un id entre 1 y 17";
    }

    public void ClosePanel()
    {
        cardPanel.SetActive(false);
    }

    private IEnumerator LoadingScreen()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cardPanel.SetActive(true);
        loadingScreen.SetActive(false);
    }
}
