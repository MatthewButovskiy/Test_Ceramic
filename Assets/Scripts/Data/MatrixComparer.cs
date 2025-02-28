using UnityEngine;
using System.Collections.Generic;

namespace MyMatrixAlignmentProject.Data
{
    public class MatrixComparer : IEqualityComparer<Matrix4x4>
    {
        private const float Epsilon = 1e-2f;

        public bool Equals(Matrix4x4 a, Matrix4x4 b)
        {
            for (int i = 0; i < 16; i++)
            {
                if (Mathf.Abs(a[i] - b[i]) > Epsilon)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(Matrix4x4 obj)
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < 16; i++)
                {
                    hash = hash * 31 + Mathf.RoundToInt(obj[i] / Epsilon);
                }
                return hash;
            }
        }
    }
}