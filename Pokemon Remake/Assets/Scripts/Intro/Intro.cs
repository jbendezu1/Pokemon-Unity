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
      

    public IEnumerator Start()
    {
        DeveloperImage.gameObject.SetActive(true);
        DeveloperText.gameObject.SetActive(true);
        IntroLogo.gameObject.SetActive(false);
        PushToStart.gameObject.SetActive(false);
        new WaitForSeconds(5f);
        yield return StartCoroutine(RunIntro());
    }

    public IEnumerator RunIntro()
    {
        var sequence = DOTween.Sequence();
        new WaitForSeconds(2f);
        yield return sequence.Join(DeveloperText.DOFade(0f, 2f));
        yield return sequence.Join(DeveloperImage.DOFade(0f, 2f));
        new WaitForSeconds(1.5f);
        IntroLogo.gameObject.SetActive(true);
        PushToStart.gameObject.SetActive(true);
        new WaitForSeconds(1.5f);
        int count = 0;
        while (count == 0)
        {
            yield return sequence.Append(PushToStart.DOColor(Color.gray, 2f));
            yield return sequence.Append(PushToStart.DOColor(Color.blue, 2f));
            if (Input.GetKeyDown(KeyCode.Return))
            {
                count++;
                yield return sequence.Join(IntroLogo.DOFade(0f, 2f));
                yield return sequence.Join(PushToStart.DOFade(0f, 2f));
            }
        }
    }
    public void ConnectRegion()
    {

    }
}
