using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiMove : MonoBehaviour, IDragHandler //쓸일 있음 영상 더 보고 추가
{

    [SerializeField] RectTransform dragRectTransform;
    [SerializeField] Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


}