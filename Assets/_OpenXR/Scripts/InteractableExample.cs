using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableExample : XRGrabInteractable
{
    // Variables
    public Gradient gradient = null;
    private MeshRenderer meshRenderer = null;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        // If the object is held
        if (isSelected)
        {
            // During Update
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                UpdateMesh();
        }

    }

    private void UpdateMesh()
    {
        // We need to make sure we are using an action-based controller
        if (IsControllerActionBased(out ActionBasedController controller))
        {
            // Now that we know we have the right controller, get the value of the activate (trigger-pull)
            float activateValue = GetActionValue(controller.activateAction);

            // Apply that value to the object
            ApplyGradient(activateValue);
            ApplyScale(activateValue);
        }

    }

    private bool IsControllerActionBased(out ActionBasedController controller)
    {
        controller = null;

        // Needs to at least by a Base Controller Interactor
        if (selectingInteractor is XRBaseControllerInteractor interactor)
        {
            // Make sure that Controller is Action-Based
            if (interactor.xrController is ActionBasedController actionController)
                controller = actionController;
        }


        // Return a bool so we don't need this null-check else where
        return controller != null ;
    }

    private float GetActionValue(InputActionProperty inputAction)
    {
        // Read the float value, this can be a more advanced function with generics
        return inputAction.action.ReadValue<float>();
    }

    private void ApplyGradient(float value)
    {
        Color newColor = gradient.Evaluate(value);
        meshRenderer.material.color = newColor;
    }

    private void ApplyScale(float value)
    {
        Vector3 newScale = Vector3.one + (Vector3.one * value);
        transform.localScale = newScale;
    }
}