using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorCenter : MonoBehaviour
{
    [SerializeField]
    private Sprite fullGenerator;
    [SerializeField]
    private Sprite emptyGenerator;
    [SerializeField]
    private GameObject generatorImagePrefab;
    [SerializeField]
    private Transform generatorUIContainer;
    [SerializeField]
    private Exit exit;

    private List<Generator> collectedGenerators = new List<Generator>();
    private List<Image> generatorCountImages = new List<Image>();

    private bool isCompleteGenerator = false;
    // Start is called before the first frame update
    private void Start()
    {
        Generator[] foundGenerators = FindObjectsOfType<Generator>();
        collectedGenerators.AddRange(foundGenerators);
        Debug.Log(collectedGenerators.Count);
        CreateGeneratorUI();
    }

    private void CreateGeneratorUI()
    {
        for (int i = 0; i < collectedGenerators.Count; i++)
        {
            GameObject generatorImage = Instantiate(generatorImagePrefab, generatorUIContainer.transform);
            generatorCountImages.Add(generatorImage.GetComponent<Image>());
        }

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        UpdateGeneratorUI();

    }
    private void UpdateGeneratorUI()
    {
        if (collectedGenerators.Count == 0 || isCompleteGenerator)
            return;
        int generator_Index = 0;
        for (int i = 0; i < collectedGenerators.Count; i++)
        {
            //Debug.Log(collectedGenerators[i].IsComplete());
            if (collectedGenerators[i].IsComplete())
            {
                generatorCountImages[generator_Index].sprite = fullGenerator;
                generator_Index++;
            }
        }
        if(generator_Index == collectedGenerators.Count)
        {
            exit.gameObject.SetActive(true);
            isCompleteGenerator = true;
        }
    }
}
