using UnityEngine;

namespace MyMatrixAlignmentProject.Data
{
    [System.Serializable]
    public class MatrixData
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;
        
        public Matrix4x4 ToMatrix4x4()
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.m00 = m00; mat.m01 = m01; mat.m02 = m02; mat.m03 = m03;
            mat.m10 = m10; mat.m11 = m11; mat.m12 = m12; mat.m13 = m13;
            mat.m20 = m20; mat.m21 = m21; mat.m22 = m22; mat.m23 = m23;
            mat.m30 = m30; mat.m31 = m31; mat.m32 = m32; mat.m33 = m33;
            return mat;
        }
    }
}