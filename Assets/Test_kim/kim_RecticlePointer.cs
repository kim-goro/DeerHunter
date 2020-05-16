using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class kim_RecticlePointer : GvrBasePointer
{
    public const float RETICLE_MIN_INNER_ANGLE = 0.0f;
    public const float RETICLE_MIN_OUTER_ANGLE = 0.5f;
    public const float RETICLE_GROWTH_ANGLE = 1.5f;
    public const float RETICLE_DISTANCE_MIN = 0.45f;
    public float maxReticleDistance = 20.0f;
    public int reticleSegments = 20;
    public float reticleGrowthSpeed = 8.0f;

    [Range(-32767, 32767)] public int reticleSortingOrder = 32767;
    public Material MaterialComp { private get; set; }
    public float ReticleInnerAngle { get; private set; }
    public float ReticleOuterAngle { get; private set; }
    public float ReticleDistanceInMeters { get; private set; }
    public float ReticleInnerDiameter { get; private set; }
    public float ReticleOuterDiameter { get; private set; }

    float showCrosshairTime = 0;
    public GameObject m_crosshair;
    public Material m_bulletTracing;

    public override float MaxPointerDistance
    {
        get { return maxReticleDistance; }
    }

    public override void OnPointerEnter(RaycastResult raycastResultResult, bool isInteractive)
    {
        SetPointerTarget(raycastResultResult.worldPosition, isInteractive);
        //Debug.Log(raycastResultResult.gameObject.tag);
        if (raycastResultResult.gameObject.tag == "Monster")
        {
        }
    }

    public override void OnPointerHover(RaycastResult raycastResultResult, bool isInteractive)
    {
        SetPointerTarget(raycastResultResult.worldPosition, isInteractive);
    }

    public override void OnPointerExit(GameObject previousObject)
    {
        ReticleDistanceInMeters = maxReticleDistance;
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    public override void OnPointerClickDown(GameObject Target = null) //화면 터치 시
    {
        bool isBulletAvailable = transform.root.GetComponentInChildren<OBJ_Shotgun>().Shot();
        if (!isBulletAvailable) return;
        m_crosshair.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        List<Eagle_Prefab> hitedEagle = new List<Eagle_Prefab>();
        for (var i = 0; i < 30; i++)
        {
            var v3Offset = Camera.main.transform.up * Random.Range(10f, 60.0f);
            v3Offset = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Camera.main.transform.forward) * v3Offset;
            var v3Hit = Camera.main.transform.forward * 1000 + v3Offset;
            DrawLine(transform.root.GetComponentInChildren<OBJ_Shotgun>().FireOut.position + v3Hit.normalized * 5, v3Hit, Color.yellow);
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, v3Hit);
            if (Physics.Raycast(ray, out hit, 1000))
                if (hit.collider != null)
                    if (hit.collider.gameObject.CompareTag("Monster"))
                    {
                        if (hit.collider.gameObject.GetComponent<Eagle_Prefab>().isAlive)
                        {
                            hit.collider.gameObject.GetComponent<Eagle_Prefab>().totalDamage += 30;
                            hitedEagle.Add(hit.collider.gameObject.GetComponent<Eagle_Prefab>());
                            showCrosshairTime = 0.3f;
                        }
                    }
        }
        foreach (var item in hitedEagle)
        {
            if (item.totalDamage >= 40)
            {
                m_crosshair.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            }
            int totalDamage = item.totalDamage;
            item.GetShot(totalDamage);
        }
        if (Target != null)
        {
            if (Target.CompareTag("Monster"))
            {
                if (Target.GetComponent<Eagle_Prefab>().isAlive)
                {
                    Target.GetComponent<Eagle_Prefab>().GetShot(100);
                    showCrosshairTime = 0.3f;
                    m_crosshair.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                }
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
         {
             GameObject myLine = new GameObject();
             myLine.transform.position = start;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();
             lr.material = m_bulletTracing;
             lr.material.color = lr.startColor = lr.endColor = color;
             lr.startWidth = lr.endWidth = 0.01f;
             lr.SetPosition(0, start);
             lr.SetPosition(1, end);
             GameObject.Destroy(myLine, duration);
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

    protected override void Start()
    {
        base.Start();

        Renderer rendererComponent = GetComponent<Renderer>();
        rendererComponent.sortingOrder = reticleSortingOrder;

        MaterialComp = rendererComponent.material;

        CreateReticleVertices();
    }

    private void Awake()
    {
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    private void Update()
    {
        UpdateDiameters();
        if (showCrosshairTime > 0) //수정
        {
            if (GetComponent<MeshRenderer>().enabled) { GetComponent<MeshRenderer>().enabled = false; }
            if (!m_crosshair.activeSelf) { m_crosshair.SetActive(true); }
            showCrosshairTime -= Time.deltaTime;
            if (showCrosshairTime <= 0)
                showCrosshairTime = 0;
        }
        else
        {
            if (!GetComponent<MeshRenderer>().enabled) { GetComponent<MeshRenderer>().enabled = true; }
            if (m_crosshair.activeSelf) { m_crosshair.SetActive(false); }
        }
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

            vertices[vi++] = new Vector3(x, y, 0.5f); // Outer vertex.
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
