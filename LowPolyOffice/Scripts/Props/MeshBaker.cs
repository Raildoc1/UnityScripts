using UnityEditor;
using UnityEngine;

namespace Office.Props {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshBaker : MonoBehaviour {

        public float mergeVerticesDistance = 0.0001f;

#if UNITY_EDITOR

        [ContextMenu("Combine")]
        private void Combine() {
            MeshFilter[] meshesToCombine = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combineInstances = new CombineInstance[meshesToCombine.Length];

            for (int i = 0; i < meshesToCombine.Length; i++) {
                combineInstances[i].mesh = meshesToCombine[i].sharedMesh;
                combineInstances[i].transform = meshesToCombine[i].transform.localToWorldMatrix;
                meshesToCombine[i].gameObject.SetActive(false);
            }

            Mesh newMesh = new Mesh();
            newMesh.CombineMeshes(combineInstances, true, true, true);



            Unwrapping.GenerateSecondaryUVSet(newMesh);

            Vector3[] vertices = newMesh.vertices;

            int mergedVerticesAmount = 0;

            for (int i = 0; i < vertices.Length; i++) {
                for (int j = i + 1; j < vertices.Length; j++) {
                    if (Vector3.Distance(vertices[i], vertices[j]) < mergeVerticesDistance) {
                        vertices[j] = vertices[i];
                        mergedVerticesAmount++;
                    }
                }
            }

            Debug.Log($"Merged {mergedVerticesAmount} vertices.");

            newMesh.vertices = vertices;

            transform.GetComponent<MeshFilter>().mesh = newMesh;

            transform.gameObject.SetActive(true);
        }

        [ContextMenu("Uncombine")]
        private void Uncombine() {
            MeshFilter[] meshesToCombine = GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i < meshesToCombine.Length; i++) {
                meshesToCombine[i].gameObject.SetActive(true);
            }

            transform.GetComponent<MeshFilter>().mesh = null;
        }

#endif
    }
}