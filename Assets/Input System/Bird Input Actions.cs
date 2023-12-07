//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Input System/Bird Input Actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @BirdInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BirdInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Bird Input Actions"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""0b4e59e4-b1b0-43ff-b63d-67d22ed463d7"",
            ""actions"": [
                {
                    ""name"": ""Pitch"",
                    ""type"": ""Value"",
                    ""id"": ""a99277a8-8fa2-4f9d-a687-f24eb34f4f26"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""a5db1687-c3b8-4e19-bd96-cc5c6019184e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Flap"",
                    ""type"": ""Value"",
                    ""id"": ""709a40c2-bf8e-4e54-a62e-4b14356cb6f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32250351-52cc-4c4d-b0d4-ee0880f13f87"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19664bfb-90f2-4839-9ffa-9745096f86f1"",
                    ""path"": ""<Joystick>/stick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51265b46-59c1-4a49-a7f3-b797d6df6543"",
                    ""path"": ""<HID::Logitech Logitech Extreme 3D>/stick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb508a0d-cd21-4701-a6e4-421b93707d30"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c870c2c-af7f-4335-a076-9ef28d9cffcf"",
                    ""path"": ""<Joystick>/stick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15ebd65d-5c8f-4118-a623-02aefc2ec13f"",
                    ""path"": ""<HID::Logitech Logitech Extreme 3D>/stick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04ef9c43-a22c-49ff-bc36-63886bab64a7"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79834513-31ba-4223-824d-ac1f87f1d9e3"",
                    ""path"": ""<HID::Logitech Logitech Extreme 3D>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_Pitch = m_gameplay.FindAction("Pitch", throwIfNotFound: true);
        m_gameplay_Roll = m_gameplay.FindAction("Roll", throwIfNotFound: true);
        m_gameplay_Flap = m_gameplay.FindAction("Flap", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // gameplay
    private readonly InputActionMap m_gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_gameplay_Pitch;
    private readonly InputAction m_gameplay_Roll;
    private readonly InputAction m_gameplay_Flap;
    public struct GameplayActions
    {
        private @BirdInputActions m_Wrapper;
        public GameplayActions(@BirdInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pitch => m_Wrapper.m_gameplay_Pitch;
        public InputAction @Roll => m_Wrapper.m_gameplay_Roll;
        public InputAction @Flap => m_Wrapper.m_gameplay_Flap;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @Pitch.started += instance.OnPitch;
            @Pitch.performed += instance.OnPitch;
            @Pitch.canceled += instance.OnPitch;
            @Roll.started += instance.OnRoll;
            @Roll.performed += instance.OnRoll;
            @Roll.canceled += instance.OnRoll;
            @Flap.started += instance.OnFlap;
            @Flap.performed += instance.OnFlap;
            @Flap.canceled += instance.OnFlap;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @Pitch.started -= instance.OnPitch;
            @Pitch.performed -= instance.OnPitch;
            @Pitch.canceled -= instance.OnPitch;
            @Roll.started -= instance.OnRoll;
            @Roll.performed -= instance.OnRoll;
            @Roll.canceled -= instance.OnRoll;
            @Flap.started -= instance.OnFlap;
            @Flap.performed -= instance.OnFlap;
            @Flap.canceled -= instance.OnFlap;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnPitch(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnFlap(InputAction.CallbackContext context);
    }
}
