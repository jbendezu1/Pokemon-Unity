using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroFades : MonoBehaviour
{
    [SerializeField] Image DeveloperImage;
    [SerializeField] Text DeveloperText;
    [SerializeField] Image IntroLogo;
    [SerializeField] Text PushToStart;

    Image DevIm;
    Text DevText;
    Image IntLogo;
    Text PTS;

    private void Awake()
    {
        DevIm = DeveloperImage.GetComponent<Image>();
        DevText = DeveloperText.GetComponent<Text>();
        IntLogo = IntroLogo.GetComponent<Image>();
        PTS = PushToStart.GetComponent<Text>();
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
        var sequence = DOTween.Sequence();
        new WaitForSeconds(1.5f);
        sequence.Join(DeveloperImage.DOFade(0f, 0.5f));
        sequence.Join(DeveloperText.DOFade(0f, 0.5f));
        new WaitForSeconds(1.5f);
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
        new WaitForSeconds(1.5f);
        int count = 0;
        while (count == 0)
        {
            sequence.Append(PushToStart.DOColor(Color.gray, 0.3f));
            sequence.Append(PushToStart.DOColor(Color.black, 0.3f));
            if (Input.GetKeyDown(KeyCode.Return))
            {
                count++;
                sequence.Join(IntroLogo.DOFade(0f, 0.5f));
                sequence.Join(PushToStart.DOFade(0f, 0.5f));
            }
        }
    }
    public void ConnectRegion()
    {

    }
}
