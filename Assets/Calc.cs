using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Calc : MonoBehaviour
{
    public InputField height;
    public InputField weight;
    public Text result;
    public Image image;
    // Start is called before the first frame update
    void Start() {

    }

    public void BtClick() {
        string h = height.text;
        string w = weight.text;
        StartCoroutine(HttpConnect(h,w));
    }

    IEnumerator HttpConnect(string h, string w) {
        WWWForm form = new WWWForm();
        form.AddField("height", h);
        form.AddField("weight", w);

        string url = "http://localhost:8080/HttpLesson/CalcBMI";
        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();
        if (uwr.isHttpError || uwr.isNetworkError) {
            Debug.Log(uwr.error);
            yield break;
        } else {
            BMI bmi = JsonUtility.FromJson<BMI>(uwr.downloadHandler.text);
            result.text = bmi.bmi.ToString();
            url = bmi.result;
        }
        
        uwr = UnityWebRequestTexture.GetTexture(url);
        yield return uwr.SendWebRequest();
        if (uwr.isHttpError || uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            Texture texture = DownloadHandlerTexture.GetContent(uwr);
            image.sprite =Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update() {
    }
}
class BMI {
    public float height;
    public float weight;
    public float bmi;
    public string result;
}
