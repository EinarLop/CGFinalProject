using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject car;
    Vector3[] originalCoordinates;
    public Vector3 start;
    public Vector3 end;
    public Vector3 pos;
    float param;
     List<Vector3> ctrl;
    float t;
    int n;


     float Combination(int n, int i){
        return (float)Factorial(n) / (Factorial(i) * Factorial(n-i));
    }

    int Factorial(int n){
        if(n==0) return 1;
        else return n * Factorial(n-1);
    }

     Vector3 EvalBezier(float t){
     Vector3 bez = Vector3.zero;
     for(int i=0; i<n ; i++){
         float u = Combination(n-1,i) * Mathf.Pow(1.0f - t, n -1 - i) * Mathf.Pow(t, i);
         bez += u*ctrl[i];
     }
     return bez;
    }

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
    ctrl = new List<Vector3>();
    ctrl.Add(new Vector3(0,0,0));
    ctrl.Add(new Vector3(20f,0,0));
    ctrl.Add(new Vector3(40f,0,0));
    ctrl.Add(new Vector3(60f,0,0));
    ctrl.Add(new Vector3(80f,0,0));
    ctrl.Add(new Vector3(103f,0,-25f));
        ctrl.Add(new Vector3(0,0,0));

    // ctrl.Add(new Vector3(20,0,0));
    // ctrl.Add(new Vector3(25,0,0));
    n = ctrl.Count;
        param = 0;
        originalCoordinates = car.GetComponent<MeshFilter>().mesh.vertices;
        
    }

    // Update is called once per frame
    void Update()
    {   //Para controlar velocidad aumentar o reducir param
        // (Pos - prev)/ parm = velocicdad
        // Formula colision 36:11
        param += 0.001f;
        //pos = Interpolate(start, end, param); //EvalBezier
        pos = EvalBezier(param);
        Matrix4x4 t = Transformations.TranslateM(pos.x, pos.y+1.15f,pos.z);
        //Vector3 prev = Interpolate(start, end, param - 0.0001f);
        Vector3 prev = EvalBezier(param - 0.001f);
        
        Vector3 du = (pos - prev).normalized;
        float alpha = Mathf.Atan2(-du.z, du.x) * Mathf.Rad2Deg;
        Matrix4x4 r = Transformations.RotateM(alpha, Transformations.AXIS.AX_Y);
        // r * t sistema solar 
        car.GetComponent<MeshFilter>().mesh.vertices = ApplyTransform(t*r, originalCoordinates);
        car.GetComponent<MeshFilter>().mesh.RecalculateBounds();
    }
}
