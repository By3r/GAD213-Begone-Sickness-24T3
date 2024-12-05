using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dana.ConflictSystem
{


    public class TextWobbler : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_Text _textMesh;
        [SerializeField] private float xOffset = 9.5f;
        [SerializeField] private float yOffset = 5.5f;

        private Mesh _mesh;
        private Vector3[] _vertices;
        private Vector3[] _originalVertices;
        #endregion

        void Update()
        {
            _textMesh.ForceMeshUpdate();
            _mesh = _textMesh.mesh;
            _vertices = _mesh.vertices;

            TMP_TextInfo textInfo = _textMesh.textInfo;
            _originalVertices = _mesh.vertices;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 offset = Wobble(Time.time + i + j);
                    _vertices[vertexIndex + j] = _originalVertices[vertexIndex + j] + offset;
                }
            }

            _mesh.vertices = _vertices;
            _textMesh.canvasRenderer.SetMesh(_mesh);
        }

        Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time * 5.3f) * xOffset, Mathf.Cos(time * 3.5f) * yOffset);
        }
    }
}