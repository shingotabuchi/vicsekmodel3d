using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FovKnob : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rct;
    public RectTransform rct1;
    public bool dragOnSurfaces = true;
    private RectTransform m_DraggingPlane;
    Vector3 dragStartPos;
    public InputField fovfield;
    public Slider fovslider;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out dragStartPos);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        // if (m_DraggingIcon != null)
        SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            float angle = Mathf.Acos((rct.position.y-globalMousePos.y)/Mathf.Sqrt((globalMousePos.y-rct.position.y)*(globalMousePos.y-rct.position.y) + (globalMousePos.x-rct.position.x)*(globalMousePos.x-rct.position.x)));
            rct.rotation = Quaternion.Euler(0,0,-angle*180f/Mathf.PI);
            rct1.rotation = Quaternion.Euler(0,0,angle*180f/Mathf.PI);
            fovfield.text = (180f-angle*180f/Mathf.PI).ToString();
            fovslider.value = 180f-angle*180f/Mathf.PI;
        }
    }
    public void ChangeField(){
        float textFloat;
        bool success = float.TryParse( fovfield.text ,out textFloat );
        if(success){
            if(0<=textFloat&&textFloat<=180){
                fovslider.value = textFloat;
                Boid.fov = textFloat;
                ChangeSlider();
            }
            else{
                fovfield.text = fovslider.value.ToString();
            }
        }
        else{
            fovfield.text = fovslider.value.ToString();
        }
    }
    public void ChangeSlider(){
        fovfield.text = fovslider.value.ToString();
        Boid.fov = fovslider.value;
        rct.rotation = Quaternion.Euler(0,0,fovslider.value-180f);
        rct1.rotation = Quaternion.Euler(0,0,-fovslider.value+180f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
