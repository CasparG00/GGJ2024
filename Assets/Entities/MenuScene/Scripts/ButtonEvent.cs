using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    [SerializeField] private List<Image> selectButtonList;
    private int currentSelectButtonIndex;

    private Player owner;

    private void Update()
    {
        if (owner is null)
            return;

        InputAction moveAction = owner.PlayerInput.actions.FindAction("Move");
        if (moveAction.triggered)
        {
            float input = moveAction.ReadValue<Vector2>().y;
            if (input < -0.1f)
            {
                currentSelectButtonIndex = (currentSelectButtonIndex + 1) % selectButtonList.Count;

                UpdateSelectedButton();
            }
            else if (input > 0.1f)
            {
                currentSelectButtonIndex--;

                if (currentSelectButtonIndex < 0)
                {
                    currentSelectButtonIndex = selectButtonList.Count - 1;
                }

                UpdateSelectedButton();
            }
        }

        if (owner.PlayerInput.actions.FindAction("Punch").triggered)
        {
            selectButtonList[currentSelectButtonIndex].GetComponent<ButtonSend>().sceneChange();
        }
    }

    private void UpdateSelectedButton()
    {
        for (int i = 0; i < selectButtonList.Count; i++)
        {
            selectButtonList[i].sprite = i == currentSelectButtonIndex ? selectedSprite : unselectedSprite;
        }
    }
}
