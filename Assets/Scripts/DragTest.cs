using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform limitTransform;
    public RectTransform lowerlimitTransform;
    public bool dragOnSurfaces = true;
    // private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    Vector3 dragStartPos; 
    RectTransform rt;
    Vector3 initScale;
    float scaleLimit;
    float lowerScaleLimit = 0;
    public bool isZOA = false;
    public RectTransform boidIcon;
    public Slider ZoaSlider;
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        initScale = rt.localScale;
        if(isZOA) initScale = boidIcon.localScale;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        // m_DraggingIcon = new GameObject("icon");

        // m_DraggingIcon.transform.SetParent(canvas.transform, false);
        // m_DraggingIcon.transform.SetAsLastSibling();

        // var image = m_DraggingIcon.AddComponent<Image>();

        // image.sprite = GetComponent<Image>().sprite;
        // image.SetNativeSize();
        if(!isZOA)scaleLimit = limitTransform.localScale.x;
        if(!isZOA&&lowerlimitTransform!=null)lowerScaleLimit = lowerlimitTransform.localScale.x;
        if(isZOA) initScale = boidIcon.localScale;
        
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
            // rt.position = globalMousePos;
            // rt.rotation = m_DraggingPlane.rotation;
            float scalerate = Vector3.Distance(globalMousePos,rt.position)/Vector3.Distance(dragStartPos,rt.position);
            if(isZOA){
                float sliderValue = 500f*0.41f/(258.9391f * boidIcon.localScale.x);
                if(scalerate>1){
                    if(sliderValue<ZoaSlider.maxValue){
                        boidIcon.localScale = initScale * (1/scalerate);
                    }
                }
                else{
                    if(sliderValue>ZoaSlider.minValue){
                        boidIcon.localScale = initScale * (1/scalerate);
                    }
                }
            }
            else{
                if(scalerate>1){
                    if(rt.localScale.x<scaleLimit){
                        rt.localScale = initScale * scalerate;
                    }
                }
                else{
                    if(rt.localScale.x>=lowerScaleLimit){
                        rt.localScale = initScale * scalerate;
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        initScale = rt.localScale;
        if(isZOA) initScale = boidIcon.localScale;
        // if (m_DraggingIcon != null)
        //     Destroy(m_DraggingIcon);
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
