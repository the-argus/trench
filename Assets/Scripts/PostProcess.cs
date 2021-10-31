using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{
    public Material PostProcessMat;
    public int PixelateFactor = 1;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // create new rendertextures
        RenderTexture postprocess = new RenderTexture(source.width, source.height, source.depth);
        RenderTexture pixelate = new RenderTexture(source.width / PixelateFactor, source.height / PixelateFactor, source.depth, source.format);
        pixelate.Create();
        postprocess.Create();

        // make them scale without aliasing
        pixelate.filterMode = FilterMode.Point;
        source.filterMode = FilterMode.Point;

        // apply pixelation by scaling render texture down to smaller tex
        Graphics.Blit(source, pixelate);

        // apply postprocessing
        Graphics.Blit(pixelate, postprocess, PostProcessMat);
        Graphics.Blit(postprocess, destination);
    }
}
