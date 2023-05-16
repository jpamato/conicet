using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject tooltip;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public bool setOver;

    private void Start() {
#if UNITY_WEBGL && !UNITY_EDITOR
        if(cursorTexture!=null)
            hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
#endif
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("OnPointerEnter");
        if(tooltip!=null)
            tooltip.SetActive(true);
        if(cursorTexture!=null)
#if UNITY_WEBGL && !UNITY_EDITOR        
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
#else
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
#endif


        if (setOver)
            transform.parent.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData) {
        //  Debug.Log("OnPointerExit");
        if (tooltip != null)
            tooltip.SetActive(false);
        if (cursorTexture != null)
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    void OnDestroy() {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
