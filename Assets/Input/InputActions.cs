//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/InputActions.inputactions
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

public partial class @InputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""edf38c92-59f5-483f-b7e5-cccbbfabe706"",
            ""actions"": [
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""81647f2b-7782-4a7b-9469-bd22677d7831"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PointerPress"",
                    ""type"": ""Button"",
                    ""id"": ""749c39c8-a2d8-49e6-b18a-59732072c4a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""d77ee8ec-d074-41bd-92b4-03811406c4d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e135a82d-df23-4df0-9921-88515636aae5"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19cc95a3-6696-4b13-b615-6521a936896d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1bdf946-5698-4c23-a542-c5528019d25a"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2673676f-6738-4456-8cfc-34b5ff3878a4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Global"",
            ""id"": ""58329a6b-fe40-4961-bbe5-a5c8ef73dcf8"",
            ""actions"": [
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""2f2163fc-3d87-4bea-b7d3-7d24eede41e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""68078e18-eeda-4f6b-b1d7-6c38448e6186"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShowHelp"",
                    ""type"": ""Button"",
                    ""id"": ""2dad751f-f459-41a7-9da3-6fce530e4e20"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HideHelp"",
                    ""type"": ""Button"",
                    ""id"": ""d549bbfe-129a-492b-8392-ba1de5d30258"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""330dee7d-1b05-4ee6-9ef3-c2213d141bf7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d34a9bd8-8161-4dd5-b809-dc3c4f95b577"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""465c1f4a-312c-4531-bfb9-f42016d4f457"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowHelp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fef9007d-d9ef-4a15-9272-88a6a2baba97"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HideHelp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""651adf3b-c9a1-4928-90c2-37bf956e122f"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HideHelp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_PointerPosition = m_Player.FindAction("PointerPosition", throwIfNotFound: true);
        m_Player_PointerPress = m_Player.FindAction("PointerPress", throwIfNotFound: true);
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_Quit = m_Global.FindAction("Quit", throwIfNotFound: true);
        m_Global_Restart = m_Global.FindAction("Restart", throwIfNotFound: true);
        m_Global_ShowHelp = m_Global.FindAction("ShowHelp", throwIfNotFound: true);
        m_Global_HideHelp = m_Global.FindAction("HideHelp", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_PointerPosition;
    private readonly InputAction m_Player_PointerPress;
    private readonly InputAction m_Player_Cancel;
    public struct PlayerActions
    {
        private @InputActions m_Wrapper;
        public PlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PointerPosition => m_Wrapper.m_Player_PointerPosition;
        public InputAction @PointerPress => m_Wrapper.m_Player_PointerPress;
        public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @PointerPosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
                @PointerPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPress;
                @PointerPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPress;
                @PointerPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPress;
                @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @PointerPress.started += instance.OnPointerPress;
                @PointerPress.performed += instance.OnPointerPress;
                @PointerPress.canceled += instance.OnPointerPress;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Global
    private readonly InputActionMap m_Global;
    private IGlobalActions m_GlobalActionsCallbackInterface;
    private readonly InputAction m_Global_Quit;
    private readonly InputAction m_Global_Restart;
    private readonly InputAction m_Global_ShowHelp;
    private readonly InputAction m_Global_HideHelp;
    public struct GlobalActions
    {
        private @InputActions m_Wrapper;
        public GlobalActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Quit => m_Wrapper.m_Global_Quit;
        public InputAction @Restart => m_Wrapper.m_Global_Restart;
        public InputAction @ShowHelp => m_Wrapper.m_Global_ShowHelp;
        public InputAction @HideHelp => m_Wrapper.m_Global_HideHelp;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
            {
                @Quit.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                @Quit.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                @Quit.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                @Restart.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnRestart;
                @ShowHelp.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnShowHelp;
                @ShowHelp.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnShowHelp;
                @ShowHelp.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnShowHelp;
                @HideHelp.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnHideHelp;
                @HideHelp.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnHideHelp;
                @HideHelp.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnHideHelp;
            }
            m_Wrapper.m_GlobalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Quit.started += instance.OnQuit;
                @Quit.performed += instance.OnQuit;
                @Quit.canceled += instance.OnQuit;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
                @ShowHelp.started += instance.OnShowHelp;
                @ShowHelp.performed += instance.OnShowHelp;
                @ShowHelp.canceled += instance.OnShowHelp;
                @HideHelp.started += instance.OnHideHelp;
                @HideHelp.performed += instance.OnHideHelp;
                @HideHelp.canceled += instance.OnHideHelp;
            }
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    public interface IPlayerActions
    {
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnPointerPress(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IGlobalActions
    {
        void OnQuit(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnShowHelp(InputAction.CallbackContext context);
        void OnHideHelp(InputAction.CallbackContext context);
    }
}
