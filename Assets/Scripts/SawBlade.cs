using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    [SerializeField] float AnglePerSecond = 400.0f;     // Rotations per second
    [SerializeField] bool Movement = false;             // Enabled positional movement
    [SerializeField] bool VerticalMovement = false;     // False = Horizontal Movement | True = Verical Movement
    [SerializeField] float MaxMoveUnits = 0.0f;
    [SerializeField] float MovementPerSecond = 0.1f;

    [SerializeField] Transform RespawnLocation;
    [SerializeField] string PlayerLayerString = "Player";
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip respawnSound;

    private bool reverseMovement = false;
    private Camera cam;
    private Vector3 originalPos;
    private float elapsedMovement = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = this.transform.position;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation
        this.transform.Rotate(Vector3.forward, AnglePerSecond * Time.deltaTime);

        elapsedMovement += MovementPerSecond * Time.deltaTime;

        if (Movement)
        {
            float negativeFloat = reverseMovement ? -1.0f : 1.0f;

            if (VerticalMovement)
            {
                this.transform.Translate(Vector3.up * MovementPerSecond * negativeFloat * Time.deltaTime,Space.World);
            }
            else
            {
                this.transform.Translate(Vector3.right * MovementPerSecond * negativeFloat * Time.deltaTime, Space.World);
            }

            if (elapsedMovement > MaxMoveUnits)
            {
                reverseMovement = !reverseMovement;
                elapsedMovement = 0.0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Movement) return;

        // Draw a box around our camera boundary
        Gizmos.color = Color.yellow;

        float xScale = this.transform.localScale.x / 2.0f;
        float yScale = this.transform.localScale.y / 2.0f;

        if (VerticalMovement) {
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y - yScale), new Vector2(this.transform.position.x - xScale, this.transform.position.y + yScale + MaxMoveUnits));
            Gizmos.DrawLine(new Vector2(this.transform.position.x + xScale, this.transform.position.y - yScale), new Vector2(this.transform.position.x + xScale, this.transform.position.y + yScale + MaxMoveUnits));
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y - yScale), new Vector2(this.transform.position.x + xScale, this.transform.position.y - yScale));
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y + yScale + MaxMoveUnits), new Vector2(this.transform.position.x + xScale, this.transform.position.y + yScale + MaxMoveUnits));
        }
        else
        {
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y + yScale), new Vector2(this.transform.position.x + xScale + MaxMoveUnits, this.transform.position.y + yScale));
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y - yScale), new Vector2(this.transform.position.x + xScale + MaxMoveUnits, this.transform.position.y - yScale));
            Gizmos.DrawLine(new Vector2(this.transform.position.x - xScale, this.transform.position.y + yScale), new Vector2(this.transform.position.x - xScale, this.transform.position.y - yScale));
            Gizmos.DrawLine(new Vector2(this.transform.position.x + xScale + MaxMoveUnits, this.transform.position.y + yScale), new Vector2(this.transform.position.x + xScale + MaxMoveUnits, this.transform.position.y - yScale));

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer(PlayerLayerString))
        {
            col.transform.position = RespawnLocation.position;
            cam.transform.position = RespawnLocation.position;
            audioSource.PlayOneShot(respawnSound);
        }
    }
}