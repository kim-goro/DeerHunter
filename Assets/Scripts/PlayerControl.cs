using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : GvrBasePointer
{
    public const float RETICLE_MIN_INNER_ANGLE = 0.0f;

    public const float RETICLE_MIN_OUTER_ANGLE = 0.5f;

    public const float RETICLE_GROWTH_ANGLE = 1.5f;

    public const float RETICLE_DISTANCE_MIN = 0.45f;

    public float maxReticleDistance = 250.0f;

    public int reticleSegments = 20;

    public float reticleGrowthSpeed = 8.0f;

    [Range(-32767, 32767)]
    public int reticleSortingOrder = 32767;

    public Material MaterialComp { private get; set; }

    public float ReticleInnerAngle { get; private set; }

    public float ReticleOuterAngle { get; private set; }

    public float ReticleDistanceInMeters { get; private set; }

    public float ReticleInnerDiameter { get; private set; }

    public float ReticleOuterDiameter { get; private set; }

    #region InGameControl
    MeshRenderer ReticleMesh;
    #endregion

    protected override void Start()
    {
        base.Start();

        Renderer rendererComponent = GetComponent<Renderer>();
        rendererComponent.sortingOrder = reticleSortingOrder;

        MaterialComp = rendererComponent.material;

        CreateReticleVertices();
        ///////////////////////////////////////////////////////
        ReticleMesh = this.gameObject.GetComponent<MeshRenderer>();
       

    }
    
    public override float MaxPointerDistance
    {
        get { return maxReticleDistance; }
    }

    public bool isEagle = false;
    public bool isSky = false;
    //ray에 object가 닿으면 그에 따른 mesh변경.
    public override void OnPointerEnter(RaycastResult raycastResultResult, bool isInteractive)
    {
        SetPointerTarget(raycastResultResult.worldPosition, isInteractive);
        Debug.Log(raycastResultResult.gameObject.tag);
        if (raycastResultResult.gameObject.tag == "Monster")
        {
            ReticleMesh.enabled = true;
            ReticleMesh.material.color = Color.red;
        }else if(raycastResultResult.gameObject.tag == "BasicObject")
        {
            ReticleMesh.enabled = true;
            ReticleMesh.material.color = Color.white;
        }
    }
    public override void OnPointerExit(GameObject previousObject)
    {
        ReticleDistanceInMeters = maxReticleDistance;
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        /////////////////////// 
        if(ReticleMesh.enabled == true)
        {
            ReticleMesh.material.color = Color.white;
            ReticleMesh.enabled = false;
        }

    }

    public override void OnPointerClickDown(GameObject target = null)//수정
    {
    }

    public override void OnPointerHover(RaycastResult raycastResultResult, bool isInteractive)
    {
        SetPointerTarget(raycastResultResult.worldPosition, isInteractive);
    }



 

    public override void OnPointerClickUp()
    {
    }

    public override void GetPointerRadius(out float enterRadius, out float exitRadius)
    {
        float min_inner_angle_radians = Mathf.Deg2Rad * RETICLE_MIN_INNER_ANGLE;

        float max_inner_angle_radians =
            Mathf.Deg2Rad * (RETICLE_MIN_INNER_ANGLE + RETICLE_GROWTH_ANGLE);

        enterRadius = 2.0f * Mathf.Tan(min_inner_angle_radians);
        exitRadius = 2.0f * Mathf.Tan(max_inner_angle_radians);
    }

    public void UpdateDiameters()
    {
        ReticleDistanceInMeters =
      Mathf.Clamp(ReticleDistanceInMeters, RETICLE_DISTANCE_MIN, maxReticleDistance);

        if (ReticleInnerAngle < RETICLE_MIN_INNER_ANGLE)
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        }

        if (ReticleOuterAngle < RETICLE_MIN_OUTER_ANGLE)
        {
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        }

        float inner_half_angle_radians = Mathf.Deg2Rad * ReticleInnerAngle * 0.5f;
        float outer_half_angle_radians = Mathf.Deg2Rad * ReticleOuterAngle * 0.5f;

        float inner_diameter = 2.0f * Mathf.Tan(inner_half_angle_radians);
        float outer_diameter = 2.0f * Mathf.Tan(outer_half_angle_radians);

        ReticleInnerDiameter =
      Mathf.Lerp(ReticleInnerDiameter, inner_diameter, Time.unscaledDeltaTime * reticleGrowthSpeed);
        ReticleOuterDiameter =
      Mathf.Lerp(ReticleOuterDiameter, outer_diameter, Time.unscaledDeltaTime * reticleGrowthSpeed);

        MaterialComp.SetFloat("_InnerDiameter", ReticleInnerDiameter * ReticleDistanceInMeters);
        MaterialComp.SetFloat("_OuterDiameter", ReticleOuterDiameter * ReticleDistanceInMeters);
        MaterialComp.SetFloat("_DistanceInMeters", ReticleDistanceInMeters);
    }


    private void Awake()
    {
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    private void Update()
    {
        UpdateDiameters();
    }

    private bool SetPointerTarget(Vector3 target, bool interactive)
    {
        if (PointerTransform == null)
        {
            Debug.LogWarning("Cannot operate on a null pointer transform");
            return false;
        }

        Vector3 targetLocalPosition = PointerTransform.InverseTransformPoint(target);

        ReticleDistanceInMeters = Mathf.Clamp(targetLocalPosition.z,
                                              RETICLE_DISTANCE_MIN,
                                              maxReticleDistance);
        if (interactive)
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE + RETICLE_GROWTH_ANGLE;
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE + RETICLE_GROWTH_ANGLE;
        }
        else
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        }

        return true;
    }

    private void CreateReticleVertices()
    {
        Mesh mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;

        int segments_count = reticleSegments;
        int vertex_count = (segments_count + 1) * 2;


        Vector3[] vertices = new Vector3[vertex_count];

        const float kTwoPi = Mathf.PI * 2.0f;
        int vi = 0;
        for (int si = 0; si <= segments_count; ++si)
        {
            float angle = (float)si / (float)segments_count * kTwoPi;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            vertices[vi++] = new Vector3(x, y, 0.0f); // Outer vertex.
            vertices[vi++] = new Vector3(x, y, 1.0f); // Inner vertex.
        }

        int indices_count = (segments_count + 1) * 3 * 2;
        int[] indices = new int[indices_count];

        int vert = 0;
        int idx = 0;
        for (int si = 0; si < segments_count; ++si)
        {
            indices[idx++] = vert + 1;
            indices[idx++] = vert;
            indices[idx++] = vert + 2;

            indices[idx++] = vert + 1;
            indices[idx++] = vert + 2;
            indices[idx++] = vert + 3;

            vert += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
#if !UNITY_5_5_OR_NEWER
        // Optimize() is deprecated as of Unity 5.5.0p1.
        mesh.Optimize();
#endif  // !UNITY_5_5_OR_NEWER
    }
}

