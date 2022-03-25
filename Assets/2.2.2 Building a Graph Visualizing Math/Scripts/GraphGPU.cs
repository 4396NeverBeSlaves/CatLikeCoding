using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphGPU : MonoBehaviour
{
    public enum TransitionMode { Cycle, Random}
    const int maxResolution = 1000;

    [SerializeField] ComputeShader computeShader;
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;
    [SerializeField,Range(10, maxResolution)] int resolution = 400;
    [SerializeField] FunctionLibrary.FunctionName functionName;
    [SerializeField, Range(0, 1)] float alpha = 0.7f;
    [SerializeField, Range(0, 32)] float beta = 4;
    [SerializeField, Range(0, 16)] float gamma = 2;
    [SerializeField] Text textAlpha, textBeta, textGamma, textResolution;
    [SerializeField] Dropdown dropdown;
    [SerializeField,Min(0f)] float functionDurationThreshold = 1f, transitionDurationThreshold=1f;
    [SerializeField] TransitionMode transitionMode;

    FunctionLibrary.Function f;
    FunctionLibrary.FunctionName oldFunctionName;
    float durationCounter = 0;
    bool transiting = false;
    ComputeBuffer positionsBuffer;

    static readonly int PositionsBufferID = Shader.PropertyToID("_Positions"),
                        TimeID = Shader.PropertyToID("_Time"),
                        ResolutionID = Shader.PropertyToID("_Resolution"),
                        StepID = Shader.PropertyToID("_Step"),
                        AlphaID = Shader.PropertyToID("alpha"),
                        BetaID = Shader.PropertyToID("beta"),
                        GammaID = Shader.PropertyToID("gamma"),
                        TransitionDurationID = Shader.PropertyToID("_TransitionDuration");

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

    public void sliderResolutionChange(Slider s)
    {
        resolution = (int)s.value;
        Text t = s.GetComponentInChildren<Text>();
        t.text = "Resolution(10-1000):" + resolution;
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
        resolution = (int)textResolution.GetComponentInParent<Slider>().value;
        textAlpha.text = "Alpha(0-1): " + alpha;
        textBeta.text = "Beta(0-16): " + beta;
        textGamma.text = "Gamma(0-16): " + gamma;
        textResolution.text = "Resolution(10 - 1000): " + resolution;
        UpdateDropdown();
    }
    // Start is called before the first frame update
    void Awake() {
        GenerateUI();
    }
    private void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }
    void Start()
    {
        dropdown.onValueChanged.AddListener(dropdownChange);
    }

    void GetNextFunction()
    {

        functionName = transitionMode == TransitionMode.Cycle ? 
            FunctionLibrary.GetNextFunctionName(functionName) : 
            FunctionLibrary.GetDifferentRandomFunctionName(functionName); 
    }

    void UpdateOnGPU()
    {
        int kernelIndex = (int)functionName + (transiting ? (int)oldFunctionName: (int)functionName) * (int)FunctionLibrary.FunctionName.End;
        //Debug.Log(kernelIndex);
        computeShader.SetFloat(TimeID, Time.time);
        computeShader.SetFloat(StepID, 1f / resolution);
        computeShader.SetFloat(ResolutionID, resolution);
        computeShader.SetFloat(AlphaID, alpha);
        computeShader.SetFloat(BetaID, beta);
        computeShader.SetFloat(GammaID, gamma);
        if (transiting) { 
            computeShader.SetFloat(TransitionDurationID, Mathf.SmoothStep(0f,1f, durationCounter / transitionDurationThreshold));
        }
        computeShader.SetBuffer(kernelIndex, PositionsBufferID, positionsBuffer);
        material.SetFloat(StepID, 1f / resolution);
        material.SetBuffer(PositionsBufferID, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);


        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0,material, bounds, resolution* resolution);
    }

    // Update is called once per frame
    void Update()
    {
        durationCounter += Time.unscaledDeltaTime;
        
        if (transiting)
        {
            if (durationCounter >= transitionDurationThreshold)
            {
                durationCounter = 0;
                transiting = false;
            }
        }
        else if(durationCounter>= functionDurationThreshold) {
            transiting = true;
            durationCounter = 0f;
            oldFunctionName = functionName;
            GetNextFunction();
        }
        UpdateOnGPU();
    }
}
