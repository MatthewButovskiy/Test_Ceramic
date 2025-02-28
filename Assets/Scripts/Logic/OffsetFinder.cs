using System.Collections.Generic;
using UnityEngine;
using MyMatrixAlignmentProject.Data;

namespace MyMatrixAlignmentProject.Logic
{
    public static class OffsetFinder
    {
        public static List<Matrix4x4> FindOffsets(
            List<Matrix4x4> modelMatrices,
            List<Matrix4x4> spaceMatrices)
        {
            var resultOffsets = new List<Matrix4x4>();

            if (modelMatrices.Count == 0 || spaceMatrices.Count == 0)
            {
                return resultOffsets;
            }
            
            var spaceSet = new HashSet<Matrix4x4>(spaceMatrices, new MatrixComparer());
            
            Matrix4x4 baseModelMatrix = modelMatrices[0];
            Matrix4x4 baseModelInverse = baseModelMatrix.inverse;
            
            foreach (var sMat in spaceMatrices)
            {
                Matrix4x4 offset = sMat * baseModelInverse;

                bool allFound = true;
                foreach (var mMat in modelMatrices)
                {
                    Matrix4x4 candidate = offset * mMat;
                    if (!spaceSet.Contains(candidate))
                    {
                        allFound = false;
                        break;
                    }
                }

                if (allFound)
                {
                    resultOffsets.Add(offset);
                }
            }

            return resultOffsets;
        }
    }
}