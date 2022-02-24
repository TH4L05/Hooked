using UnityEngine;

public class DestructableItem :Item
{
    #region Fields

    private bool destroyed = false;
    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject broken;
    [SerializeField] private MeshCollider mCollider;

    [SerializeField] private GameObject pointsFloatTextTemplate;
    [SerializeField] private float positionOffset = 1f;

    #endregion

    #region UnityFunctions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged")) return;
        if (destroyed) return;
       

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER HITS VASE");
            var player =other.GetComponent<CharacterController>();
            var velocity = player.velocity.magnitude;
            Debug.Log(velocity);

            if (velocity < 1f) return;
        }

        //destroyed = true;
        //Debug.Log("fgdfgdf");       
        //UpdatePoints();
        Destroy();       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    #endregion

    public void Destroy()
    {
        if (destroyed) return;
        
        destroyed = true;
        normal.SetActive(false);
        mCollider.enabled = false;

        var collider = GetComponent<MeshCollider>();
        collider.enabled = false;
        broken.SetActive(true);

        float scale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;

        Debug.Log("Pot_Scale: " + scale);
        Level.instance.audioEvents.SetAudioParameter("Pot_Scale", scale);
        Level.instance.audioEvents.PlayAudioEvent("PotBreak", gameObject);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

        var obj = Instantiate(pointsFloatTextTemplate, transform.position, Quaternion.identity);
        obj.transform.Translate(0, positionOffset, 0);
        obj.transform.LookAt(Level.instance.player.transform);       
        obj.transform.Rotate(Vector3.up, 180);
        //pointsFloatTextTemplate.SetActive(true);
        //pointsFloatTextTemplate.transform.LookAt(Level.instance.player.transform);
        //pointsFloatTextTemplate.transform.Rotate(Vector3.up, 180);
        UpdatePoints();
    }

    public void UpdatePoints()
    {
        //if (destroyed) return;
        Level.instance.UpdateDestructionPoints((int)data.health);
    }
}
