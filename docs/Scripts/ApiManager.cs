using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField]
    private string fakeApiURL =
        "https://my-json-server.typicode.com/JuanSMarin2/api-baraja/players";

    [SerializeField]
    private string characterURL =
        "https://rickandmortyapi.com/api/character";

    public void GetPlayer(int id, Action<Player> onSuccess, Action onError)
    {
        StartCoroutine(GetPlayerCoroutine(id, onSuccess, onError));
    }

    private IEnumerator GetPlayerCoroutine(int id, Action<Player> onSuccess, Action onError)
    {
        UnityWebRequest www = UnityWebRequest.Get(fakeApiURL + "/" + id);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke();
        }
        else
        {
            Player player = JsonUtility.FromJson<Player>(www.downloadHandler.text);
            onSuccess?.Invoke(player);
        }
    }

    public void GetCharacter(int id, Action<Character> onSuccess, Action onError)
    {
        StartCoroutine(GetCharacterCoroutine(id, onSuccess, onError));
    }

    private IEnumerator GetCharacterCoroutine(int id, Action<Character> onSuccess, Action onError)
    {
        UnityWebRequest www = UnityWebRequest.Get(characterURL + "/" + id);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke();
        }
        else
        {
            Character character = JsonUtility.FromJson<Character>(www.downloadHandler.text);
            onSuccess?.Invoke(character);
        }
    }

    public void GetTexture(string url, Action<Texture2D> onSuccess)
    {
        StartCoroutine(GetTextureCoroutine(url, onSuccess));
    }

    private IEnumerator GetTextureCoroutine(string url, Action<Texture2D> onSuccess)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            onSuccess?.Invoke(texture);
        }
    }
}
