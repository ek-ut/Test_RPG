using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoistikController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image JoystickBG;
    private Image Joystick;
    private Vector2 importVector;

    private void Start()
    {
        JoystickBG = GetComponent<Image>();
        Joystick = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        importVector = Vector2.zero;
        Joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBG.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / JoystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / JoystickBG.rectTransform.sizeDelta.y);
            if (pos.x < -1) pos.x = -1;
            if (pos.x > 1) pos.x = 1;
            if (pos.y < -1) pos.y = -1;
            if (pos.y > 1) pos.y = 1;
        }
        importVector = new Vector2(pos.x , pos.y);
        Joystick.rectTransform.anchoredPosition = new Vector2(importVector.x * (JoystickBG.rectTransform.sizeDelta.x/2), importVector.y * (JoystickBG.rectTransform.sizeDelta.y / 2));
        //print(pos);
    }

    public float Horizontal()
    {
        if(importVector.x !=0)
        {
            return importVector.x;
        }else
        {
           return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (importVector.y != 0)
        {
            return importVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
