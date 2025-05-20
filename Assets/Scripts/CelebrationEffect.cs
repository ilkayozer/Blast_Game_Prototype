using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CelebrationEffect : MonoBehaviour
{
   
    public GameObject goldStarImage; 

    public CanvasGroup canvasGroup;           
    public RectTransform panelRectTransform;  

    private Vector2 originalAnchoredPos;

    void Start()
    {      
        goldStarImage.transform.localScale = Vector3.zero;

        originalAnchoredPos = panelRectTransform.anchoredPosition;
    
        panelRectTransform.anchoredPosition = originalAnchoredPos + new Vector2(0f, 80f);
        canvasGroup.alpha = 0f;
    }

    public void PlayCelebration()
    {
        Debug.Log("celebration girdi");

        Tween.Scale(goldStarImage.transform, Vector3.one * 2f, duration: 0.5f, Ease.OutBack, endDelay: 0.4f).OnComplete(() =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }

    public void ShowGameOver()
    {
        panelRectTransform.gameObject.SetActive(true);

        Tween.Alpha(canvasGroup, 1f, 0.4f, Ease.OutQuad);

        Tween.UIAnchoredPosition(panelRectTransform, originalAnchoredPos, 0.5f, Ease.OutBack);
    }
}
