using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Exponential_Fog")]
public sealed class Exponential_Fog : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the distance from the camera at which the fog begins.")]
    public FloatParameter _fogBegin = new FloatParameter(1f);

    [Tooltip("Controls the size of the fog before it becomes completely opaque.")]
    public FloatParameter _fogSize = new FloatParameter(1f);

    Material m_Material;

    public bool IsActive() => m_Material != null && _fogSize.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.BeforePostProcess;

    const string kShaderName = "Hidden/Shader/Exponential_Fog";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Exponential_Fog is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_FogBegin", _fogBegin.value);
        m_Material.SetFloat("_FogSize", _fogSize.value);
        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
