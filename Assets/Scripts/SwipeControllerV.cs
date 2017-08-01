using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
public class SwipeControllerV : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform Content;
    public ScrollRect ScrollRect;
    public GameObject Dots;

    public int StartPlane;
    public Color DotsNonSelectionColor;
    public Color DotsSelectionColor;

    [Tooltip("if we swipped more than SwipeDelay then we move")]
    public int SwipeDelay;
    [Tooltip("enables dots")]
    public bool WithDots;

    private float _sizeOfImage;
    private float _initialSizeOfImage;
    private float _beginDrag; // to know in what direction did we swipe

    private int _amountOfImages;
    private RectTransform ScrollRectRectTransform;
 
    void Start()
    {
        Content.offsetMin = Vector2.zero;
        Content.offsetMax = Vector2.zero;
        _initialSizeOfImage = GetComponent<RectTransform>().rect.width;
        _amountOfImages = Content.childCount - 1;
        ScrollRectRectTransform = ScrollRect.GetComponent<RectTransform>();

        if (WithDots)
        {
            SetDotsColor();
        }
        else
        {
            Dots.SetActive(false);
        }
    }

    public void NextSlide()
    {
        if (StartPlane >= _amountOfImages)
        {
            return;
        }
        _sizeOfImage -= _initialSizeOfImage;
        StartPlane++;
        Move();
    }

    public void PreviousSlide()
    {
        if (StartPlane <= 0)
        {
            return;
        }
        _sizeOfImage += _initialSizeOfImage;
        StartPlane--;
        Move();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ScrollRect.OnEndDrag(eventData);
        if (Mathf.Abs(Content.offsetMin.x - _beginDrag) >= SwipeDelay) // if we swipped more than SwipeDelay then we move
        {
            if (_beginDrag > Content.offsetMin.x) // right
            {
                if (StartPlane >= _amountOfImages)
                {
                    return;
                }
                _sizeOfImage -= _initialSizeOfImage;
                StartPlane++;
            }
            else // left
            {
                if (StartPlane <= 0)
                {
                    return;
                }
                _sizeOfImage += _initialSizeOfImage;
                StartPlane--;
            }

            Move();
        }
        else // if we did not swipe
        {
            //Debug.Log("No");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ScrollRect.OnBeginDrag(eventData);
        _beginDrag = Content.offsetMin.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ScrollRect.OnDrag(eventData);
    }

    private void Move()
    {
        ScrollRectRectTransform.offsetMin = new Vector2(_sizeOfImage, ScrollRectRectTransform.offsetMin.y);
        ScrollRectRectTransform.offsetMax = new Vector2(_sizeOfImage, ScrollRectRectTransform.offsetMax.y);
        if (WithDots)
        {
            SetDotsColor();
        }
    }

    private void SetDotsColor()
    {
        for (int i = 0; i < Dots.transform.childCount; i++)
        {
            if (i != StartPlane)
            {
                Dots.transform.GetChild(i).GetComponent<Image>().color = DotsNonSelectionColor;
            }
            else
            {
                Dots.transform.GetChild(i).GetComponent<Image>().color = DotsSelectionColor;
            }
        }
    }
}
