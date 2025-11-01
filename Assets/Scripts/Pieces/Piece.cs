using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    public Tilemap tilemap;
    public LayerMask pieceLayer;
    public Transform pieceRoot;

    private bool isDragging;
    private Collider2D[] colliders;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerClick");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("PointerMove");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
    }

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
