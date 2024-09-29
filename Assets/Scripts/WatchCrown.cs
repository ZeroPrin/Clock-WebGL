using UnityEngine;
using UnityEngine.EventSystems;

public class WatchCrown : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform _hourArrow;
    [SerializeField] private RectTransform _minuteArrow;
    [SerializeField] private RectTransform _secondArrow;

    private RectTransform rectTransform;
    private float previousAngle;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPivot = rectTransform.position;

        Vector3 mouseWorldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out mouseWorldPos);

        Vector2 dir = mouseWorldPos - worldPivot;
        previousAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPivot = rectTransform.position;

        Vector3 mouseWorldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out mouseWorldPos);

        Vector2 dir = mouseWorldPos - worldPivot;
        float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float angleDelta = Mathf.DeltaAngle(previousAngle, currentAngle);

        rectTransform.Rotate(Vector3.forward, angleDelta);

        _minuteArrow.Rotate(Vector3.forward, angleDelta);
        _hourArrow.Rotate(Vector3.forward, angleDelta / 12f);
        //_secondArrow.Rotate(Vector3.forward, angleDelta * 60f); //optional: seconds can be taken into account

        previousAngle = currentAngle;
    }
}
