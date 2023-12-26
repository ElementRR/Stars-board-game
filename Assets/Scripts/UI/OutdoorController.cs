using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This is the script who controlls the messages that appear in the superior screen
public class OutdoorController : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Animator anim;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        ShowTurnAction.OnMessageSent += ShowMessage;
        WhoFirstLogic.OnMessageSent += ShowMessage;
        anim = GetComponent<Animator>();
    }

    private IEnumerator MessageRoutine(string message, float time)
    {
        text.text = message;
        anim.Play("Outdoor");
        yield return new WaitForSeconds(time);
        text.text = "";
    }

    void ShowMessage(string message)
    {
        StartCoroutine(MessageRoutine(message, 1.5f));
    }

    private void OnDestroy()
    {
        ShowTurnAction.OnMessageSent -= ShowMessage;
    }
}
