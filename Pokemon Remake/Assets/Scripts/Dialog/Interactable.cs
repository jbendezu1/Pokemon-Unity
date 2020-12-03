using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    void Interact(Transform initiator);
}

public interface InteractOcean
{
    void Interact();
}