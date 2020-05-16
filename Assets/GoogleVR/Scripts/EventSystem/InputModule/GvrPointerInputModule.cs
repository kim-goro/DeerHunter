
using System.Collections.Generic;
using Gvr.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("GoogleVR/GvrPointerInputModule")]
[HelpURL("https://developers.google.com/vr/unity/reference/class/GvrPointerInputModule")]
public class GvrPointerInputModule : BaseInputModule, IGvrInputModuleController
{

    [Tooltip("Whether Pointer input is active in VR Mode only (true), or all the time (false).")]
    public bool vrModeOnly = false;


    [Tooltip("Manages scroll events for the input module.")]
    public GvrPointerScrollInput scrollInput = new GvrPointerScrollInput();


    public static GvrBasePointer Pointer
    {
        get
        {
            GvrPointerInputModule module = FindInputModule();
            if (module == null || module.Impl == null)
            {
                return null;
            }

            return module.Impl.Pointer;
        }

        set
        {
            GvrPointerInputModule module = FindInputModule();
            if (module == null || module.Impl == null)
            {
                return;
            }

            module.Impl.Pointer = value;
        }
    }

    public static RaycastResult CurrentRaycastResult
    {
        get
        {
            GvrPointerInputModule inputModule = GvrPointerInputModule.FindInputModule();
            if (inputModule == null)
            {
                return new RaycastResult();
            }

            if (inputModule.Impl == null)
            {
                return new RaycastResult();
            }

            if (inputModule.Impl.CurrentEventData == null)
            {
                return new RaycastResult();
            }

            return inputModule.Impl.CurrentEventData.pointerCurrentRaycast;
        }
    }

    public GvrPointerInputModuleImpl Impl { get; private set; }

    public GvrEventExecutor EventExecutor { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "UnityRules.LegacyGvrStyleRules",
        "VR1001:AccessibleNonConstantPropertiesMustBeUpperCamelCase",
        Justification = "Legacy Public API.")]
    public new EventSystem eventSystem
    {
        get
        {
            return base.eventSystem;
        }
    }


    public List<RaycastResult> RaycastResultCache
    {
        get
        {
            return m_RaycastResultCache;
        }
    }

    public static void OnPointerCreated(GvrBasePointer createdPointer)
    {
        GvrPointerInputModule module = FindInputModule();
        if (module == null || module.Impl == null)
        {
            return;
        }

        if (module.Impl.Pointer == null)
        {
            module.Impl.Pointer = createdPointer;
        }
        Debug.Log("on potinter create");
    }

    public static GvrEventExecutor FindEventExecutor()
    {
        GvrPointerInputModule gvrInputModule = FindInputModule();
        if (gvrInputModule == null)
        {
            return null;
        }
        Debug.Log("find event executor");
        return gvrInputModule.EventExecutor;
    }

    public static GvrPointerInputModule FindInputModule()
    {
        if (EventSystem.current == null)
        {
            return null;
        }

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            return null;
        }

        GvrPointerInputModule gvrInputModule =
            eventSystem.GetComponent<GvrPointerInputModule>();


        return gvrInputModule;
    }

    /// <inheritdoc/>
    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public override bool ShouldActivateModule()
    {

        return Impl.ShouldActivateModule();
    }

    /// <inheritdoc/>
    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public override void DeactivateModule()
    {
        Impl.DeactivateModule();
    }

    /// <inheritdoc/>
    public override bool IsPointerOverGameObject(int pointerId)
    {
        Debug.Log("is pointer over gameobject");
        return Impl.IsPointerOverGameObject(pointerId);
    }

    /// <inheritdoc/>
    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public override void Process()
    {
        UpdateImplProperties();
        Impl.Process();
    }

    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public bool ShouldActivate()
    {
        return base.ShouldActivateModule();
    }

    /// <summary>Deactivate this instance.</summary>
    public void Deactivate()
    {
        base.DeactivateModule();
    }

    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public new GameObject FindCommonRoot(GameObject g1, GameObject g2)
    {
        return BaseInputModule.FindCommonRoot(g1, g2);
    }


    [SuppressMemoryAllocationError(IsWarning = true, Reason = "Pending documentation.")]
    public new BaseEventData GetBaseEventData()
    {

        return base.GetBaseEventData();
    }


    public new RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
    {

        return BaseInputModule.FindFirstRaycast(candidates);
    }

    /// @cond
    /// <inheritdoc/>
    protected override void Awake()
    {
        base.Awake();
        Impl = new GvrPointerInputModuleImpl();
        EventExecutor = new GvrEventExecutor();
        UpdateImplProperties();
    }

    /// @endcond

    private void UpdateImplProperties()
    {
        if (Impl == null)
        {
            return;
        }

        Impl.ScrollInput = scrollInput;
        Impl.VrModeOnly = vrModeOnly;
        Impl.ModuleController = this;
        Impl.EventExecutor = EventExecutor;
    }
}
