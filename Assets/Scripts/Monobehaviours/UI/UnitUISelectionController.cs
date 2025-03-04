using System;
using UnityEngine;

public class UnitUISelectionController : MonoBehaviour
{
    [SerializeField] private UnitSelectionController unitSelectionController = null;
    [SerializeField] private RectTransform selectionAreaTransform = null;
    [SerializeField] private Canvas canvas = null;


    private void Start()
    {
        selectionAreaTransform.gameObject.SetActive(false);

        unitSelectionController.OnSelectionAreaStarted += OnSelectionAreaStarted;
        unitSelectionController.OnSelectionAreaEnded += OnSelectionAreaEnded;
    }

    private void Update()
    {
        if(selectionAreaTransform.gameObject.activeSelf) UpdateVisualAreaSelection();
    }

    private void OnSelectionAreaStarted(object sender, System.EventArgs e)
    {
        selectionAreaTransform.gameObject.SetActive(true);
        UpdateVisualAreaSelection();
    }
    private void OnSelectionAreaEnded(object sender, EventArgs e)
    {
        selectionAreaTransform.gameObject.SetActive(false);
    }

    private void UpdateVisualAreaSelection()
    {
        float canvasScale = canvas.transform.localScale.x;
        Rect selectionAreaRect = unitSelectionController.GetSelectionAreaRect();
        selectionAreaTransform.anchoredPosition = new Vector2(selectionAreaRect.x, selectionAreaRect.y) / canvasScale;
        selectionAreaTransform.sizeDelta = new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
    }
}
