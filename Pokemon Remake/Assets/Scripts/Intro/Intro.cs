using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] Image DeveloperImage;
    [SerializeField] Text DeveloperText;
    [SerializeField] Image IntroLogo;
    [SerializeField] Text PushToStart;
    [SerializeField] Image IntroBackground;
    [SerializeField] Image Professor;
    [SerializeField] Image DialogBox;
    [SerializeField] Image BG;
    [SerializeField] Image BG2;
    [SerializeField] Image BG3;
    [SerializeField] Image BG4;
    [SerializeField] Image StarterSelectionScreen;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject inputField;
    [SerializeField] InputField inputFieldText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;
    [SerializeField] BattleDialogBox dialogBox;
    PartyMemberUI[] memberSlots;
    public List<Pokemon> pokemons;
    [SerializeField] List<Text> actionTexts;


    Color DevIm;
    Color LogoIn;
    Color DevT;
    Color PTS;
    string playerName = "";
    Pokemon _pokemon;
    int currentMember;

    public IEnumerator Start()
    {
        DeveloperImage.gameObject.SetActive(true);
        DeveloperText.gameObject.SetActive(true);
        IntroLogo.gameObject.SetActive(false);
        PushToStart.gameObject.SetActive(false);
        BG.gameObject.SetActive(true);
        BG2.gameObject.SetActive(true);
        BG3.gameObject.SetActive(true);
        BG4.gameObject.SetActive(true);
        StarterSelectionScreen.gameObject.SetActive(true);
        IntroBackground.gameObject.SetActive(true);
        yield return StartCoroutine(BeforeIntro());
        yield return StartCoroutine(RunIntro());
    }
    public IEnumerator BeforeIntro()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
            yield return null;
        yield return GetPlayerName();
        inputField.SetActive(false);
        yield return BG3.DOFade(0f, 1f);
    }
    public IEnumerator RunIntro()
    {
        var sequence = DOTween.Sequence();
        yield return new WaitForSeconds(2);
        //while (!Input.GetKeyDown(KeyCode.Return))
            //yield return null;
        yield return sequence.Join(DeveloperText.DOFade(0f, 2f));
        yield return sequence.Join(DeveloperImage.DOFade(0f, 2f));
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
        
        yield return (BG.DOFade(0f, 2f).SetDelay(2));
        new WaitForSeconds(1.5f);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        yield return sequence.Join(IntroLogo.DOFade(0f, 2f));
        yield return sequence.Join(PushToStart.DOFade(0f, 2f));
        IntroBackground.gameObject.SetActive(true);
        yield return (BG2.DOFade(0f, 5f)).SetDelay(2);
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(TypeDialog(""));
        yield return StartCoroutine(TypeDialog("Hello there and welcome to the world of Pokemon. I'm terribly sorry for what happened, your father was a great man and an excellent trainer."));
        yield return StartCoroutine(TypeDialog("Who am I? Why I'm Professor Carbone of course, leading expert in Pokemon and programming. I'm also the person in charge to hand you your partner pokemon."));
        yield return StartCoroutine(TypeDialog("I'm sure now you must want to set off on your own adventure and continue your fathers legacy as one of the elite trainers here in the Noraldo region."));
        yield return StartCoroutine(TypeDialog("Now "+playerName + ", why don't we get you your partner Pokemon?"));
        yield return sequence.Append(IntroBackground.DOFade(0f, 2f));
        yield return sequence.Join(Professor.DOFade(0f,2f));
        yield return sequence.Join(DialogBox.DOFade(0f, 2f));
        yield return (BG4.DOFade(0f, 5f)).SetDelay(1);
        StarterSelection();
        
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
    public void StarterSelection()
    {
        HandlePartySelection();
        ConnectRegion();
    }
    public string GetPlayerName()
    {
        playerName = inputFieldText.text;
        return playerName;
    }
    
    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        currentMember = Mathf.Clamp(currentMember, 0, 2);

        dialogBox.UpdateActionSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentMember == 0)
            {

            }
            if (currentMember == 1)
            {

            }
            if (currentMember == 2)
            {

            }

        }
    }
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
                actionTexts[i].color = Color.black;
        }
    }
    public void ConnectRegion()
    {
        SceneManager.LoadScene("Region 1");
    }
}
