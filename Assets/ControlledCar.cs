using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledCar : MonoBehaviour
{
    public GameObject car;
    Vector3[] originalCoordinates;
    float param;
    Vector3 pos;
    Matrix4x4 t;
    Matrix4x4 r; 
    float alpha; 
     Vector3 Interpolate(Vector3 A, Vector3 B, float t){ //EvaluateBezier
        return A + t * (B - A);
    }

     Vector3[] ApplyTransform(Matrix4x4 m, Vector3[] verts){
        int num = verts.Length;
        Vector3[] result = new Vector3[num];
        for(int i=0; i<num; i++){
            Vector3 v = verts[i];
            Vector4 temp = new Vector4(v.x,v.y,v.z,1);
            result[i] = m* temp;
        }

        return result;
    }
    // Start is called before the first frame update
    void Start()
    {
          alpha =0;
          param = 0;
          originalCoordinates = car.GetComponent<MeshFilter>().mesh.vertices;
          pos = Vector3.zero;
          t = Transformations.TranslateM(pos.x, pos.y+1.15f,pos.z);
          r = Transformations.RotateM(0, Transformations.AXIS.AX_Y);
    }

    // Update is called once per frame
    void Update()
    {      
        
         if(Input.GetKey(KeyCode.RightArrow)){
            pos.x = pos.x+1.0f;
            t = Transformations.TranslateM(pos.x, pos.y+1.15f,pos.z);
         }

         if(Input.GetKey(KeyCode.LeftArrow)){
            pos.x = pos.x-1.0f;
            t = Transformations.TranslateM(pos.x, pos.y+1.15f,pos.z);
         }

         if(Input.GetKey(KeyCode.UpArrow)){

            alpha = alpha+1.0f; 
            //  Vector3 du = (pos - prev).normalized;
            // float alpha = Mathf.Atan2(-du.z, du.x) * Mathf.Rad2Deg;
            // Matrix4x4 r = Transformations.RotateM(alpha, Transformations.AXIS.AX_Y);
       
            r = Transformations.RotateM(alpha, Transformations.AXIS.AX_Y);
         }
        //Vector3 prev = Interpolate(start, end, param - 0.0001f);
        // Vector3 prev = EvalBezier(param - 0.001f);
        
        // Vector3 du = (pos - prev).normalized;
        // float alpha = Mathf.Atan2(-du.z, du.x) * Mathf.Rad2Deg;
     
       
           car.GetComponent<MeshFilter>().mesh.vertices = ApplyTransform(t*r, originalCoordinates);
    } 
}

