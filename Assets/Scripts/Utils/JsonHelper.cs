using UnityEngine;
using MyMatrixAlignmentProject.Data;

namespace MyMatrixAlignmentProject.Utils
{
    public static class JsonHelper
    {
        public static MatrixDataArray FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Пустой JSON");
                return null;
            }
            
            string trimmed = json.TrimStart();
            if (trimmed.StartsWith("["))
            {
                json = "{\"items\":" + json + "}";
            }
            
            MatrixDataArray dataArray = JsonUtility.FromJson<MatrixDataArray>(json);
            return dataArray;
        }

        public static string ToJson(MatrixDataArray dataArray, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(dataArray, prettyPrint);
        }
    }
}