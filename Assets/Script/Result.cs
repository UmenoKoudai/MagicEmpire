using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    private EventSystem _system;
    [SerializeField]
    private Button _selectButton;

    // Start is called before the first frame update
    void Start()
    {
        _system.SetSelectedGameObject(_selectButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
