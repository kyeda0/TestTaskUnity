using UnityEngine;

public class WallOpeningPlacementManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [Header("Door Settings")]
    [SerializeField] private float doorWidth = 1f;
    [SerializeField] private float doorHeight = 2f;

    [Header("Window Settings")]
    [SerializeField] private float windowWidth = 1.2f;
    [SerializeField] private float windowHeight = 1f;
    [SerializeField] private float windowBottomOffset = 1f;

    [Header("Material")]
    [SerializeField] private Material wallMaterial;


    private enum Mode { None, Door, Window }
    private Mode currentMode = Mode.None;

    public void EnableDoorMode()
    {
        currentMode = Mode.Door;
    }

    public void EnableWindowMode()
    {
        currentMode = Mode.Window;
    }

    private void Update()
    {
        if (currentMode == Mode.None) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceOpening();
        }
    }

    private void TryPlaceOpening()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Wall wall = hit.collider.GetComponent<Wall>();

            if (wall != null)
            {
                Vector3 localPoint =
                    wall.transform.InverseTransformPoint(hit.point);

                if (currentMode == Mode.Door)
                {
                    CreateOpening(wall, localPoint.x,
                        doorWidth,
                        doorHeight,
                        0); 
                }
                else if (currentMode == Mode.Window)
                {
                    CreateOpening(wall, localPoint.x,
                        windowWidth,
                        windowHeight,
                        windowBottomOffset);
                }

                currentMode = Mode.None;
            }
        }
    }

    private void CreateOpening(
        Wall wall,
        float centerX,
        float openingWidth,
        float openingHeight,
        float bottomOffset)
    {
        Transform t = wall.transform;

        float wallWidth = wall.width;
        float wallHeight = wall.height;
        float wallThickness = wall.thickness;

        float leftWidth = (wallWidth / 2f) + centerX - openingWidth / 2f;
        float rightWidth = wallWidth - leftWidth - openingWidth;

        float topHeight = wallHeight - bottomOffset - openingHeight;

        Destroy(wall.gameObject);

        // Левая часть
        CreateWallPart(t, leftWidth, wallHeight, wallThickness,
            -wallWidth / 2f + leftWidth / 2f, 0);

        // Правая часть
        CreateWallPart(t, rightWidth, wallHeight, wallThickness,
            centerX + openingWidth / 2f + rightWidth / 2f, 0);

        // Нижняя часть (если окно)
        if (bottomOffset > 0)
        {
            CreateWallPart(t, openingWidth, bottomOffset, wallThickness,
                centerX,
                bottomOffset / 2f - wallHeight / 2f);
        }

        // Верхняя часть
        CreateWallPart(t, openingWidth, topHeight, wallThickness,
            centerX,
            bottomOffset + openingHeight + topHeight / 2f - wallHeight / 2f);
    }

    private void CreateWallPart(
        Transform original,
        float width,
        float height,
        float thickness,
        float localX,
        float localY)
    {
        if (width <= 0 || height <= 0) return;

        GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.transform.SetParent(original.parent);

        part.transform.localScale = new Vector3(width, height, thickness);
        part.transform.localRotation = original.localRotation;
        part.GetComponent<Renderer>().material = wallMaterial;

        part.transform.localPosition =
            original.localPosition +
            original.localRotation * new Vector3(localX, localY, 0);

        Wall wallData = part.AddComponent<Wall>();
        wallData.width = width;
        wallData.height = height;
        wallData.thickness = thickness;
    }
}