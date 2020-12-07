using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    [SerializeField] Image DeveloperImage;
    [SerializeField] Text DeveloperText;
    [SerializeField] Image IntroLogo;
    [SerializeField] Text PushToStart;
    [SerializeField] Image IntroBackground;
    [SerializeField] Image BG;
    [SerializeField] Image BG2;
    [SerializeField] Image BG3;
    [SerializeField] Image BG0;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;


    Color DevIm;
    Color LogoIn;
    Color DevT;
    Color PTS;
    int waiter = 0;

    public IEnumerator Start()
    {
        DevIm = DeveloperImage.color;
        LogoIn = IntroLogo.color;
        DevT = DeveloperText.color;
        PTS = PushToStart.color;
        DeveloperImage.gameObject.SetActive(true);
        DeveloperText.gameObject.SetActive(true);
        IntroLogo.gameObject.SetActive(false);
        PushToStart.gameObject.SetActive(false);
        BG.gameObject.SetActive(true);
        BG2.gameObject.SetActive(true);
        BG3.gameObject.SetActive(true);
        IntroBackground.gameObject.SetActive(false);
        yield return StartCoroutine(RunIntro());
    }

    public IEnumerator RunIntro()
    {
        var sequence = DOTween.Sequence();
        yield return (BG0.DOFade(0f, 5f));
        yield return sequence.Join(DeveloperText.DOFade(0f, 2f)).SetDelay(2);
        yield return sequence.Join(DeveloperImage.DOFade(0f, 2f)).SetDelay(2);
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
        yield return (BG.DOFade(0f, 5f).SetDelay(2));
        int count = 0;
        while (count == 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                count++;
                yield return sequence.Join(IntroLogo.DOFade(0f, 2f));
                yield return sequence.Join(PushToStart.DOFade(0f, 2f));
            }
        }
        IntroBackground.gameObject.SetActive(true);
        yield return sequence.Append(BG2.DOFade(0f, 5f)).SetDelay(2);
        StartCoroutine (TypeDialog(""));
        StartCoroutine (TypeDialog("Hello there ummm kid and welcome to the world of Pokemon. I'm terribly sorry for what happened, your father was a great man and an excellent trainer.\n" +
            "I'm sure now you must want to set off on yourown adventure and continue your fathers legacy as one of the elite trainers here in the Noraldo region"));
    }
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }
    public void ConnectRegion()
    {

    }
}
