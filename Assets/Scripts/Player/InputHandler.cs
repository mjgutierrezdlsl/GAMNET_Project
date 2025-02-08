using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector2 _direction;
    public Vector2 Direction => _direction;

    public delegate void OnKeyPressed();
    public event OnKeyPressed OnAttackPress;

    private void Update()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnAttackPress?.Invoke();
        }
    }
}
