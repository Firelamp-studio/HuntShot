using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

class InputActionManager
{
    internal enum EventType
    {
        STARTED,
        CANCELED,
        PERFORMED
    }

    internal struct ActionEvent
    {
        public readonly string name;
        public readonly EventType eventType;
        public readonly Action<InputAction.CallbackContext> callbackAction;

        public ActionEvent(string name, EventType eventType, Action<InputAction.CallbackContext> callbackAction)
        {
            this.name = name;
            this.eventType = eventType;
            this.callbackAction = callbackAction;
        }
    }

    private PlayerInput playerInput;

    public InputActionManager(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
    }

    private List<ActionEvent>
        _disposeEventList = new List<ActionEvent>();

    public void RegisterActionEvent(string name, EventType eventType,
        Action<InputAction.CallbackContext> callbackAction)
    {
        ActionEvent actionEvent = new ActionEvent(name, eventType, callbackAction);

        _disposeEventList.Add(actionEvent);

        switch (actionEvent.eventType)
        {
            case EventType.STARTED:
                playerInput.actions.FindAction(actionEvent.name).started += actionEvent.callbackAction;
                break;

            case EventType.CANCELED:
                playerInput.actions.FindAction(actionEvent.name).canceled += actionEvent.callbackAction;
                break;

            case EventType.PERFORMED:
                playerInput.actions.FindAction(actionEvent.name).performed += actionEvent.callbackAction;
                break;
        }
    }

    public void DisposeAllEvents()
    {
        foreach (var actionEvent in _disposeEventList)
        {
            switch (actionEvent.eventType)
            {
                case EventType.STARTED:
                    playerInput.actions.FindAction(actionEvent.name).started -= actionEvent.callbackAction;
                    break;

                case EventType.CANCELED:
                    playerInput.actions.FindAction(actionEvent.name).canceled -= actionEvent.callbackAction;
                    break;

                case EventType.PERFORMED:
                    playerInput.actions.FindAction(actionEvent.name).performed -= actionEvent.callbackAction;
                    break;
            }
        }
    }
}