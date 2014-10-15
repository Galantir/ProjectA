using UnityEngine;
using System.Collections;

public class PlayerIO : MonoBehaviour {

    public static PlayerIO Current { get; set; }
    public float InteractionRange = 8;
    public Transform AddReticule;
    public Transform DeleteReticule;

    // Update is called once per frame
    void Update() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionRange)) {
            Vector3 pos = hit.point - hit.normal / 4;
            DeleteReticule.position = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
            Vector3 posb = hit.point + hit.normal / 4;
            AddReticule.position = new Vector3(Mathf.FloorToInt(posb.x), Mathf.FloorToInt(posb.y), Mathf.FloorToInt(posb.z));
        } else {
            AddReticule.position = new Vector3(0, -100, 0);
            DeleteReticule.position = new Vector3(0, -100, 0);
        }

        Control();
    }

    internal void Control() {
        if (Input.GetMouseButtonDown(0)) {
            Chunk chunk = World.FindChunk(DeleteReticule.transform.position);
            if (chunk != null) {
                chunk.SetBlock(new Block(0), DeleteReticule.transform.position);
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            Chunk chunk = World.FindChunk(AddReticule.transform.position);
            if (chunk != null) {
                chunk.SetBlock(new Block(2), AddReticule.transform.position);
            }
        }
    }
}
