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

    Image DevIm;
    Text DevText;
    Image IntLogo;
    Text PTS;
    Color DIoriginalColor;
    Color DToriginalColor;
    Color ILoriginalColor;
    Color PTSoriginalColor;
    Image image;

    private void Awake()
    {
        DevIm = DeveloperImage.GetComponent<Image>();
        DevText = DeveloperText.GetComponent<Text>();
        IntLogo = IntroLogo.GetComponent<Image>();
        PTS = PushToStart.GetComponent<Text>();
        DIoriginalColor = DevIm.color;
        DToriginalColor = DevText.color;
        ILoriginalColor = IntLogo.color;
        PTSoriginalColor = PTS.color;
    }

    public void Start()
    {
        DeveloperImage.gameObject.SetActive(true);
        DeveloperText.gameObject.SetActive(true);
        IntroLogo.gameObject.SetActive(false);
        PushToStart.gameObject.SetActive(false);
    }

    public void HandleUpdate()
    {
        if (DeveloperImage.IsActive())
            FadeInDevIntro();
        if (IntroLogo.IsActive())
            FadeInIntroLogo();
    }

    public void FadeInDevIntro()
    {
        var sequence = DOTween.Sequence();
        new WaitForSeconds(1.5f);
        sequence.Join(DevIm.DOFade(0f, 0.5f));
        sequence.Join(DevText.DOFade(0f, 0.5f));
        new WaitForSeconds(1.5f);
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
    }

    public void FadeInIntroLogo()
    {
        var sequence = DOTween.Sequence();
        new WaitForSeconds(1.5f);
        int count = 0;
        while (count == 0)
        {
            sequence.Append(PTS.DOColor(Color.gray, 0.3f));
            sequence.Append(PTS.DOColor(Color.black, 0.3f));
            if (Input.GetKeyDown(KeyCode.Return))
            {
                count++;
                sequence.Join(DevIm.DOFade(0f, 0.5f));
                sequence.Join(DevText.DOFade(0f, 0.5f));
            }
        }
        
    }

    public void ConnectRegion()
    {

    }
}
