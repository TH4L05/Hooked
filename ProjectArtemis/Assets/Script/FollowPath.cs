using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 5.0f;
    private int waypointIndex = 0;
    [SerializeField] private int waypointStartIndex = 1;
    [SerializeField] private GameObject character;
    [SerializeField] private bool lerpRotation;
    [SerializeField] private float lerpSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = waypointStartIndex;
        character.transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowThePath();
    }

    void FollowThePath()
    {
        if (lerpRotation)
        {
            LookAtNextPointSmooth();
        }
        else
        {
            LookAtNextPoint();
        }


        character.transform.position = Vector3.MoveTowards(character.transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
        if (character.transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }

        
    }

    void LookAtNextPointSmooth()
    {
        var direction = (waypoints[waypointIndex].transform.position - character.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        character.transform.rotation = Quaternion.Lerp(character.transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);

    }

    void LookAtNextPoint()
    {
        character.transform.LookAt(waypoints[waypointIndex].transform.position);
    }


}
