using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField,Range(10,100)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName functionName= FunctionLibrary.FunctionName.Torus;
    [SerializeField, Range(0, 1)] float alpha = 0.7f;
    [SerializeField, Range(0, 32)] float beta = 4;
    [SerializeField, Range(0, 16)] float gamma = 2;
    [SerializeField] Text textAlpha, textBeta, textGamma;
    [SerializeField] Dropdown dropdown;

    FunctionLibrary.Function f;

    public void sliderAlphaChange(Slider s)
    {
        alpha = s.value;
        Text t = s.GetComponentInChildren<Text>();
        t.text = "Alpha(0-1): "+alpha;
    }    
    public void sliderBetaChange(Slider s)
    {
        beta = s.value;
        Text t = s.GetComponentInChildren<Text>();
        t.text = "Beta(0-16): " + beta;
    }    
    public void sliderGammaChange(Slider s)
    {
        gamma = s.value;
        Text t = s.GetComponentInChildren<Text>();
        t.text = "Gamma(0-16): " + gamma;
    }

    public void dropdownChange(int funcIndex)
    {
        
        //f = FunctionLibrary.GetFunction(funcIndex);
        functionName = (FunctionLibrary.FunctionName)funcIndex;
        Debug.Log(functionName.ToString());
    }

    void UpdateDropdown()
    {
        dropdown.ClearOptions();
        Dropdown.OptionData optionData;

        for(int i = 0; i < (int)FunctionLibrary.FunctionName.End; i++)
        {
            optionData = new Dropdown.OptionData();
            optionData.text = ((FunctionLibrary.FunctionName)i).ToString();
            dropdown.options.Add(optionData);
        }
        dropdown.value = (int)FunctionLibrary.FunctionName.Torus;
    }

    void GenerateUI()
    {
        alpha = textAlpha.GetComponentInParent<Slider>().value;
        beta = textBeta.GetComponentInParent<Slider>().value;
        gamma = textGamma.GetComponentInParent<Slider>().value;
        textAlpha.text = "Alpha(0-1): " + alpha;
        textBeta.text = "Beta(0-16): " + beta;
        textGamma.text = "Gamma(0-16): " + gamma;
        UpdateDropdown();
    }
    Transform[] points;
    // Start is called before the first frame update
    void Awake() {
        points = new Transform[resolution*resolution];
        Transform point;
        float step = 2f / resolution;
        var scale= Vector3.one * step;

        for (int i = 0; i < points.Length; i++)
        {
                points[i] = point = Instantiate(pointPrefab);
                point.SetParent(this.transform, false);
                point.localScale = scale;
        }
        GenerateUI();
    }
    private void Start()
    {
        dropdown.onValueChanged.AddListener(dropdownChange);
    }
    // Update is called once per frame
    void Update()
    {
        f = FunctionLibrary.GetFunction(functionName);
        FunctionLibrary.alpha = alpha;
        FunctionLibrary.beta = beta;
        FunctionLibrary.gamma = gamma;

        var time = Time.time;
        float step = 2f / resolution;

        for (int v = 0; v < resolution; v++)
        {
            for (int u = 0; u < resolution; u++)
            {
                float uOffset= (u + 0.5f) * step - 1f;
                float vOffset= (v + 0.5f) * step - 1f;

                points[v * resolution + u].position = f(uOffset, vOffset, time);
            }
        }
    }
}
