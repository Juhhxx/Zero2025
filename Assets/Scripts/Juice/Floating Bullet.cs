using UnityEngine;
using UnityEngine.Rendering;

public class FloatingBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatRotation;
    private float startY;
    private float startRotationZ;
    private RectTransform rect;
    private float phaseOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();
        startY = rect.anchoredPosition.y;

        startRotationZ = rect.rotation.eulerAngles.z;

        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time * speed + phaseOffset;

        float offset = Mathf.Sin(t) * floatHeight;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, startY + offset);

        float angle = startRotationZ + Mathf.Sin(t) * floatRotation;
        rect.rotation = Quaternion.Euler(0, 0, angle);

    }
}
