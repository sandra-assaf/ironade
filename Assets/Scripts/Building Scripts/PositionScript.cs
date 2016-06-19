﻿using UnityEngine;
using System.Collections;

public class PositionScript : State
{
    public float movementSpeed;
    private BuildingProperties properties;
    private SpriteRenderer mRenderer;
    private bool canPlace;

    // Use this for initialization
    void Start()
    {
        mRenderer = gameObject.GetComponent<SpriteRenderer>();
        properties = gameObject.GetComponent<BuildingProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            GridOperations.sharedInstance.WriteBuildingToGrid(properties.topLeftCellX, properties.topLeftCellY, properties.baseCellsWidth, properties.baseCellsHeight);
            mRenderer.color = Color.white;
            nextState = gameObject.GetComponent<LookForEnemiesScript>();
        }
        else
        {
            followCursor();
        }
    }

    private void changeColorForPlacement()
    {
        canPlace = GridOperations.sharedInstance.CheckFreeSpace(properties.topLeftCellX, properties.topLeftCellY, properties.baseCellsWidth, properties.baseCellsHeight);
        if (canPlace)
        {
            mRenderer.color = new Color(198.0f / 255.0f, 238.0f / 255.0f, 196.0f / 255.0f);
        }
        else
        {
            mRenderer.color = new Color(238.0f / 255.0f, 161.0f / 255.0f, 188.0f / 255.0f);
        }
    }

    private void followCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float cursorColumn = Mathf.Abs(mousePosition.x - GridOperations.sharedInstance.coordinates[Coordinate.Left]) / GridOperations.sharedInstance.cellWidth;
        float cursorRow = Mathf.Abs(GridOperations.sharedInstance.coordinates[Coordinate.Top] - mousePosition.y) / GridOperations.sharedInstance.cellHeight;

        if ((int)cursorColumn != properties.topLeftCellX || (int)cursorRow != properties.topLeftCellY)
        {
            properties.topLeftCellX = Mathf.FloorToInt(cursorColumn);
            properties.topLeftCellY = Mathf.FloorToInt(cursorRow);

            float pDisplacedX = cursorColumn - properties.topLeftCellX;
            float pDisplacedY = cursorRow - properties.topLeftCellY;

            float centerX = mousePosition.x + GridOperations.sharedInstance.cellWidth * ((float)properties.baseCellsWidth / 2.0f - pDisplacedX);
            float centerY = mousePosition.y + GridOperations.sharedInstance.cellHeight * (-(float)properties.baseCellsHeight / 2.0f + pDisplacedY + properties.noOfCellsHigh / 2.0f);

            float scaledHeight = (centerY - GridOperations.sharedInstance.coordinates[Coordinate.Top]) / GridOperations.sharedInstance.mapHeight;

            transform.position = new Vector3(centerX, centerY, scaledHeight);
            changeColorForPlacement();
        }
    }
}