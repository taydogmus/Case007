using UnityEngine;

namespace Tuna
{
    public class Broom : MonoBehaviour
    {

        [SerializeField] private Transform tipPoint;
        public float deltaX = 4f;
        public LayerMask _Mask;
        public bool clean;

        void Update()
        {
            
            transform.RotateAround(transform.position, Vector3.down, 3f);
            
            RaycastHit hit;
            Ray ray = new Ray(tipPoint.position, -transform.up);
            if (!Physics.Raycast(ray, out hit, 100f, _Mask))
                return;
          
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null)
                return;
            
            // Duplicate the original texture and assign to the material
            Texture2D texture = Instantiate(rend.material.GetTexture("mainTexture")) as Texture2D;
            rend.material.SetTexture("mainTexture", texture);
            
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= texture.width;
            pixelUV.y *= texture.height;

            //print(pixelUV);
            Color colorA = clean ? Color.black : Color.white;
            if (hit.transform.TryGetComponent<BloodVFX>(out BloodVFX vfx))
            {
                vfx.TakeHit();
                
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        texture.SetPixel((int)pixelUV.x + i, (int)pixelUV.y + j, colorA);        
                    }
                }
                texture.Apply();
            }
        }
    }
}
