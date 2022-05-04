using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiMove : MonoBehaviour, IDragHandler //���� ���� ���� �� ���� �߰�
{

    [SerializeField] RectTransform dragRectTransform;
    [SerializeField] Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


}