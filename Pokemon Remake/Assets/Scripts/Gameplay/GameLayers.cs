using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask Ocean;
    [SerializeField] LayerMask Foreground;
    [SerializeField] LayerMask Grass;
    [SerializeField] LayerMask Door;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask Player;
    [SerializeField] LayerMask Fov;

    public static GameLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask OceanL
    {
        get => Ocean;
    }

    public LayerMask ForegroundL
    {
        get => Foreground;
    }

    public LayerMask GrassL
    {
        get => Grass;
    }

    public LayerMask DoorL
    {
        get => Door;
    }

    public LayerMask interactableLayerL
    {
        get => interactableLayer;
    }

    public LayerMask PlayerL
    {
        get => Player;
    }

    public LayerMask FovL
    {
        get => Fov;
    }

}
