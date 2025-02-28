using System.Collections.Generic;
using UnityEngine;
using MyMatrixAlignmentProject.Data;
using MyMatrixAlignmentProject.Utils;

namespace MyMatrixAlignmentProject.Logic
{
    public class MainController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _cubeSpacePrefab;
        [SerializeField] private GameObject _cubeModelPrefab;

        [Header("Data Files (Resources)")]
        [SerializeField] private string _modelFileName = "model";
        [SerializeField] private string _spaceFileName = "space";

        [Header("Visualization Settings")]
        [SerializeField] private int _maxSpaceObjects = 200;
        [SerializeField] private int _maxModelObjects = 100;

        private List<Matrix4x4> _modelMatrices;
        private List<Matrix4x4> _spaceMatrices;
        private List<Matrix4x4> _foundOffsets = new List<Matrix4x4>();

        private void Start()
        {
            LoadMatrices();
            VisualizeSpace();
            VisualizeModel();
            FindAllOffsets();
            ExportOffsetsToJson();
        }
        
        private void LoadMatrices()
        {
            TextAsset modelText = Resources.Load<TextAsset>(_modelFileName);
            TextAsset spaceText = Resources.Load<TextAsset>(_spaceFileName);

            if (modelText == null || spaceText == null)
            {
                Debug.LogError("Не удалось загрузить JSON-файлы из папки Resources!");
                return;
            }
            
            MatrixDataArray modelArray = JsonHelper.FromJson(modelText.text);
            MatrixDataArray spaceArray = JsonHelper.FromJson(spaceText.text);

            if (modelArray == null || spaceArray == null)
            {
                Debug.LogError("Ошибка при парсинге JSON!");
                return;
            }
            
            _modelMatrices = ConvertToMatrix4x4List(modelArray.items);
            _spaceMatrices = ConvertToMatrix4x4List(spaceArray.items);

            Debug.Log($"Загружено {_modelMatrices.Count} матриц модели и {_spaceMatrices.Count} матриц пространства.");
        }
        
        private List<Matrix4x4> ConvertToMatrix4x4List(MatrixData[] dataArr)
        {
            var result = new List<Matrix4x4>();
            if (dataArr == null) return result;

            foreach (var data in dataArr)
            {
                result.Add(data.ToMatrix4x4());
            }
            return result;
        }

        
        private void VisualizeSpace()
        {
            if (_spaceMatrices == null || _spaceMatrices.Count == 0) return;

            int count = Mathf.Min(_spaceMatrices.Count, _maxSpaceObjects);
            for (int i = 0; i < count; i++)
            {
                Matrix4x4 mat = _spaceMatrices[i];
                InstantiateFromMatrix(mat, _cubeSpacePrefab);
            }
        }
        
        private void VisualizeModel()
        {
            if (_modelMatrices == null || _modelMatrices.Count == 0) return;

            int count = Mathf.Min(_modelMatrices.Count, _maxModelObjects);
            for (int i = 0; i < count; i++)
            {
                Matrix4x4 mat = _modelMatrices[i];
                InstantiateFromMatrix(mat, _cubeModelPrefab);
            }
        }

        
        private void InstantiateFromMatrix(Matrix4x4 mat, GameObject prefab)
        {
            if (prefab == null) return;

            Vector3 position = mat.GetColumn(3);
            Quaternion rotation = Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
            Vector3 scale = new Vector3(
                mat.GetColumn(0).magnitude,
                mat.GetColumn(1).magnitude,
                mat.GetColumn(2).magnitude
            );

            var obj = Instantiate(prefab, position, rotation);
            obj.transform.localScale = scale;
        }

       
        private void FindAllOffsets()
        {
            if (_modelMatrices == null || _spaceMatrices == null) return;

            _foundOffsets = OffsetFinder.FindOffsets(_modelMatrices, _spaceMatrices);
            Debug.Log($"Найдено смещений: {_foundOffsets.Count}");
        }

        
        private void ExportOffsetsToJson()
        {
            if (_foundOffsets == null || _foundOffsets.Count == 0) return;

            var dataList = new List<MatrixData>();
            foreach (var offsetMat in _foundOffsets)
            {
                var md = new MatrixData
                {
                    m00 = offsetMat.m00,
                    m01 = offsetMat.m01,
                    m02 = offsetMat.m02,
                    m03 = offsetMat.m03,

                    m10 = offsetMat.m10,
                    m11 = offsetMat.m11,
                    m12 = offsetMat.m12,
                    m13 = offsetMat.m13,

                    m20 = offsetMat.m20,
                    m21 = offsetMat.m21,
                    m22 = offsetMat.m22,
                    m23 = offsetMat.m23,

                    m30 = offsetMat.m30,
                    m31 = offsetMat.m31,
                    m32 = offsetMat.m32,
                    m33 = offsetMat.m33,
                };
                dataList.Add(md);
            }

            var arrayWrapper = new MatrixDataArray
            {
                items = dataList.ToArray()
            };

            string jsonResult = JsonHelper.ToJson(arrayWrapper, true);
            Debug.Log("Экспорт смещений в JSON:\n" + jsonResult);
            
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, "offsets.json");
            System.IO.File.WriteAllText(path, jsonResult);
            Debug.Log("Файл экспорта сохранён по пути: " + path);
        }

    }
}
