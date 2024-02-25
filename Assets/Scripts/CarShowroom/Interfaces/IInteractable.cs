using PajamaNinja.CarShowRoom;
using System;

public interface IInteractable
{
    public void Interact(InteractionHandler interHandler, float time, Action onComplete = null);
}
