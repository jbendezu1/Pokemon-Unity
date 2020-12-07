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
    [SerializeField] Text dialogText;
    [SerializeField] GameObject inputField;
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
        inputField.SetActive(false);
        yield return StartCoroutine(RunIntro());
    }

    public IEnumerator RunIntro()
    {
        var sequence = DOTween.Sequence();
        new WaitForSeconds(2f);
        yield return sequence.Join(DeveloperText.DOFade(0f, 2f));
        yield return sequence.Join(DeveloperImage.DOFade(0f, 2f));
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
        yield return (BG.DOFade(0f, 5f).SetDelay(2));
        new WaitForSeconds(1.5f);
        int count = 0;
        int changeColor = 0;
        while (count == 0)
        {
            if (changeColor % 2 == 0)
            {
                yield return sequence.Append(PushToStart.DOColor(Color.black, 0.1f));
                changeColor++;
            }
            if (changeColor % 2 == 1)
            {
                yield return sequence.Append(PushToStart.DOColor(Color.black, 0.1f));
                changeColor++;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                count++;
                yield return sequence.Join(IntroLogo.DOFade(0f, 2f));
                yield return sequence.Join(PushToStart.DOFade(0f, 2f));
            }
        }
        IntroBackground.gameObject.SetActive(true);
        yield return (BG2.DOFade(0f, 5f)).SetDelay(2);
        new WaitForSeconds(2f);
        yield return StartCoroutine(TypeDialog(""));
        yield return StartCoroutine(TypeDialog("Hello there and welcome to the world of Pokemon. I'm terribly sorry for what happened, your father was a great man and an excellent trainer."));
        yield return StartCoroutine(TypeDialog("Who am I? Why I'm Professor Carbone of course, leading expert in Pokemon and programming. I'm also the person in charge to hand you your partner pokemon."));
        yield return StartCoroutine(TypeDialog("I'm sure now you must want to set off on your own adventure and continue your fathers legacy as one of the elite trainers here in the Noraldo region."));
        yield return StartCoroutine(TypeDialog("Now ummm remind me again, what was your name?"));
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
    public IEnumerator GetPlayerName()
    {
        yield return null;
    }
    public void ConnectRegion()
    {

    }
}
