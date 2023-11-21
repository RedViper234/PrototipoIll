using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeshMaskUI : MaskableGraphic
{
    public Image imageToMask;
    private float imageWidth;
    private float imageHeight;
    public float variableHeight;
    public float variableWidth;
    public float percentage;
    public bool vertical = true;
    public bool inverseDirection = false;
    protected override void Awake()
    {
        imageWidth = imageToMask.GetComponent<RectTransform>().rect.width;
        imageHeight = imageToMask.GetComponent<RectTransform>().rect.height;
        SetVerticesDirty();
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Vector3 vec_00 = new Vector3();
        Vector3 vec_01 = new Vector3();
        Vector3 vec_11 = new Vector3();
        Vector3 vec_10 = new Vector3();

        if (vertical && !inverseDirection)
        {
            vec_00 = new Vector3(-(imageWidth / 2), -imageHeight / 2);
            vec_01 = new Vector3(-(imageWidth / 2), Mathf.Min(variableHeight, imageHeight / 2));
            vec_11 = new Vector3(imageWidth / 2, Mathf.Min(variableHeight, imageHeight / 2));
            vec_10 = new Vector3(imageWidth / 2, -imageHeight / 2);
        }
        else if (vertical && inverseDirection)
        {
            vec_00 = new Vector3(-(imageWidth / 2), Mathf.Max(variableHeight, -imageHeight / 2));
            vec_01 = new Vector3(-(imageWidth / 2), imageHeight / 2);
            vec_11 = new Vector3(imageWidth / 2, imageHeight / 2);
            vec_10 = new Vector3(imageWidth / 2, Mathf.Max(variableHeight, -imageHeight / 2));
        }
        else if (!vertical && !inverseDirection)
        {
            vec_00 = new Vector3(-(imageWidth)/2, -imageHeight / 2);
            vec_01 = new Vector3(-(imageWidth)/2, imageHeight/2);
            vec_11 = new Vector3(Mathf.Min(variableWidth, imageWidth / 2), imageHeight/2);
            vec_10 = new Vector3(Mathf.Min(variableWidth, imageWidth / 2), -imageHeight / 2);
        }
        else if (!vertical && inverseDirection)
        {
            vec_00 = new Vector3(Mathf.Max(variableWidth, -imageWidth / 2), -imageHeight / 2);
            vec_01 = new Vector3(Mathf.Max(variableWidth, -imageWidth / 2), imageHeight / 2);
            vec_11 = new Vector3((imageWidth / 2), imageHeight / 2);
            vec_10 = new Vector3((imageWidth / 2), -imageHeight / 2);
        }


        vh.AddUIVertexQuad(new UIVertex[]
        {
            new UIVertex { position = vec_00, color = Color.green},
            new UIVertex { position = vec_01, color = Color.green},
            new UIVertex { position = vec_11, color = Color.green},
            new UIVertex { position = vec_10, color = Color.green}
        });
    }

    public void setPercentage(float perc)
    {
        percentage = Mathf.Clamp(perc, 1, 100);
        float temp = 0;
        if (vertical)
        {
           temp  = (imageHeight / 100) * percentage;
           variableHeight = temp - (imageHeight / 2);
        }
        else
        {
            temp = (imageWidth / 100) * percentage;
            variableWidth = temp - (imageWidth / 2);
        }
        SetVerticesDirty();
    }
}
