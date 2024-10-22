using UnityEngine;
using TMPro;

public class TitleAnimation : MonoBehaviour
{
    private TMP_Text titleText;
    private float[] randomOffsets;

    public float amplitude = 5.0f; // Амплитуда вибрации
    public float speed = 3.0f; // Скорость вибрации

    void Start()
    {
        // Получаем компонент TMP_Text
        titleText = GetComponent<TMP_Text>();

        // Количество символов в тексте "Game of Life"
        int characterCount = titleText.text.Length;

        // Массив случайных смещений для каждой буквы
        randomOffsets = new float[characterCount];

        // Генерация случайных смещений для каждого символа
        for (int i = 0; i < characterCount; i++)
        {
            randomOffsets[i] = Random.Range(0f, 2f * Mathf.PI);
        }
    }

    void Update()
    {
        // Обновляем каждую букву
        titleText.ForceMeshUpdate();
        var textInfo = titleText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            // Позиция каждой буквы
            var verts = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

            // Смещение по оси Y на основе синусоиды (вибрация)
            float offsetY = Mathf.Sin(Time.time * speed + randomOffsets[i]) * amplitude;

            // Применяем вибрацию к вершинам символа
            for (int j = 0; j < 4; j++)
            {
                verts[textInfo.characterInfo[i].vertexIndex + j].y += offsetY;
            }
        }

        // Обновляем mesh
        for (int i = 0; i < titleText.textInfo.meshInfo.Length; i++)
        {
            titleText.textInfo.meshInfo[i].mesh.vertices = titleText.textInfo.meshInfo[i].vertices;
            titleText.UpdateGeometry(titleText.textInfo.meshInfo[i].mesh, i);
        }
    }
}