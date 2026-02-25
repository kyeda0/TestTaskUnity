using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private Material wallMaterial;
    public void GenerateRoom(float width, float length)
    {
        float wallHeight = 3f;
        float wallThickness = 0.1f;

        foreach (Transform child in room.transform)
        {
            Destroy(child.gameObject);
        }

        // Пол 
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.AddComponent<BoxCollider>();
        floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(width, 0.1f, length);
        floor.transform.localPosition = Vector3.zero;
        floor.GetComponent<Renderer>().material =wallMaterial;

        // Левая
        CreateWall(
            new Vector3(length, wallHeight, wallThickness),
            new Vector3(-width / 2f, wallHeight / 2f, 0),
            Quaternion.Euler(0, 90, 0)
        );

        // Правая
        CreateWall(
            new Vector3(length, wallHeight, wallThickness),
            new Vector3(width / 2f, wallHeight / 2f, 0),
            Quaternion.Euler(0, 90, 0)
        );

        // Передняя
        CreateWall(
            new Vector3(width, wallHeight, wallThickness),
            new Vector3(0, wallHeight / 2f, -length / 2f),
            Quaternion.identity
        );

        // Задняя
        CreateWall(
            new Vector3(width, wallHeight, wallThickness),
            new Vector3(0, wallHeight / 2f, length / 2f),
            Quaternion.identity
        );
    }

    private void CreateWall(Vector3 scale, Vector3 localPosition, Quaternion rotation)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.AddComponent<BoxCollider>();
        wall.transform.SetParent(room.transform);
        wall.transform.localScale = scale;
        wall.transform.localPosition = localPosition;
        wall.transform.localRotation = rotation;
        wall.GetComponent<Renderer>().material = wallMaterial;
        
        Wall wallData = wall.AddComponent<Wall>();
        wallData.width = scale.x;
        wallData.height = scale.y;
        wallData.thickness = scale.z;
    }
}