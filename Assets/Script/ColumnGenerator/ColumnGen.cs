using UnityEngine;
using System.Collections;

public class ColumnGen : MonoBehaviour {

    public int partWidth = 1;
    public int partHeight = 1;

    public GameObject emptyColumnPrefab;

    public GameObject[] baseParts;
    public GameObject[] centralParts;
    public GameObject[] subTopParts;
    public GameObject[] topParts;

    public int columnWitdh = 2;
    public int columnHeight = 3;



    public void GenerateColumn()
    {
        if(columnWitdh >= 2 && columnHeight >= 3)
        {
            int numberOfCentralPartsWidth = columnWitdh - 2;
            int numberOfCentralPartsHeight = columnHeight - 3;

            // Instantiating empty column
            GameObject theColumn = Instantiate(emptyColumnPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            // Base
            GameObject bottomLeft = Instantiate(baseParts[0], Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            bottomLeft.transform.parent = theColumn.transform;
            GameObject bottomRight = Instantiate(baseParts[2], new Vector3((columnWitdh-1) * partWidth,0,0), Quaternion.Euler(Vector3.zero)) as GameObject;
            bottomRight.transform.parent = theColumn.transform;
            for (int i = 1; i <= numberOfCentralPartsWidth; i++)
            {
                GameObject middlePart = Instantiate(baseParts[1], new Vector3(i * partWidth, 0, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
                middlePart.transform.parent = theColumn.transform;
            }

            // Central Parts
            for(int i=0; i < numberOfCentralPartsHeight; i++)
            {
                GameObject centerLeft = Instantiate(centralParts[0], new Vector3(0f, (float)(i + 1), 0f), Quaternion.Euler(Vector3.zero)) as GameObject;
                centerLeft.transform.parent = theColumn.transform;
                GameObject centerRight = Instantiate(centralParts[2], new Vector3((columnWitdh - 1) * partWidth, (float)(i + 1), 0), Quaternion.Euler(Vector3.zero)) as GameObject;
                centerRight.transform.parent = theColumn.transform;
                for (int j = 1; j <= numberOfCentralPartsWidth; j++)
                {
                    GameObject middlePart = Instantiate(centralParts[1], new Vector3(j * partWidth, (float)(i + 1), 0), Quaternion.Euler(Vector3.zero)) as GameObject;
                    middlePart.transform.parent = theColumn.transform;
                }
            }

            // SubTop Parts
            GameObject subTopLeft = Instantiate(subTopParts[0], new Vector3(0f, columnHeight - 2, 0f), Quaternion.Euler(Vector3.zero)) as GameObject;
            subTopLeft.transform.parent = theColumn.transform;
            GameObject subTopRight = Instantiate(subTopParts[2], new Vector3((columnWitdh - 1) * partWidth, columnHeight - 2, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
            subTopRight.transform.parent = theColumn.transform;
            for (int i = 1; i <= numberOfCentralPartsWidth; i++)
            {
                GameObject middlePart = Instantiate(subTopParts[1], new Vector3(i * partWidth, columnHeight - 2, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
                middlePart.transform.parent = theColumn.transform;
            }

            // Top Parts
            GameObject topLeft = Instantiate(topParts[0], new Vector3(0f, columnHeight - 1, 0f), Quaternion.Euler(Vector3.zero)) as GameObject;
            topLeft.transform.parent = theColumn.transform;
            GameObject topRight = Instantiate(topParts[2], new Vector3((columnWitdh - 1) * partWidth, columnHeight - 1, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
            topRight.transform.parent = theColumn.transform;
            for (int i = 1; i <= numberOfCentralPartsWidth; i++)
            {
                GameObject middlePart = Instantiate(topParts[1], new Vector3(i * partWidth, columnHeight - 1, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
                middlePart.transform.parent = theColumn.transform;
            }
        }
    }

}
