 using System;
using TMPro;
using UnityEngine;

public class InputForUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldForX;
    [SerializeField] private TMP_InputField inputFieldForY;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private GameObject canvas;
    public event Action<string,string> onInputText;

    public void OnGenerateRoomButton()
    {
        if(float.TryParse(inputFieldForX.text, out float width) &&
        float.TryParse(inputFieldForY.text, out float length))
        {
            roomGenerator.GenerateRoom(width, length);
        }

        canvas.SetActive(false);
    }



}
