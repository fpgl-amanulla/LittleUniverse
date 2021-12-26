using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CollectablePopUp : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI txtScoreValue;

    public void ShowPopUp()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(Random.Range(-50, 30), 150);
        rectTransform.DOAnchorPos3DY(250, .5f).OnComplete(delegate
        {
            rectTransform.DOShakeScale(.15f, .1f).OnComplete(delegate
            {
                Destroy(this.gameObject);
            });

        });
    }

}
