using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : InteractiveObject
{

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnInteractive()
    {
        Debug.Log("Starting Exit OnInteractive");
        SceneManager.LoadScene("End");
        // 발전기가 다 켜져있는지. 안켜져있으면 얼리 리턴
    }
}
