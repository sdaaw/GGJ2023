using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ChatPopup : MonoBehaviour
{
    public static List<string> Messages = new List<string>();

    public List<string> messages_to_set = new List<string>();

    public TMP_Text textField;
    private Canvas canvas;
    private Camera _cam;

    private bool isPlaying;

    private void Start()
    {
        if(messages_to_set.Count > 0)
        {
            foreach(string msg in messages_to_set)
            {
                if(!Messages.Contains(msg))
                {
                    Messages.Add(msg);
                }
            }
        }

        textField = GetComponentInChildren<TMP_Text>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        _cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if(canvas)
            canvas.transform.LookAt(canvas.transform.position + _cam.transform.rotation * Vector3.back,
                                       _cam.transform.rotation * Vector3.down);
    }

    public void DisplayMessage()
    {
        if(!isPlaying)
            StartCoroutine(DisplayMessageTime(GetRandomMessage(), 1));
    }

    private IEnumerator DisplayMessageTime(string message, float time)
    {
        isPlaying = true;
        canvas.gameObject.SetActive(true);
        textField.text = message;
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(false);
        isPlaying = false;
    }

    public string GetRandomMessage()
    {
        Debug.Log(Messages.Count);
        string message = Messages[Random.Range(0, Messages.Count)];
        return message;
    }




}
