using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Dither")]
public sealed class Dither : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter Spread = new ClampedFloatParameter(0f, 0f, 1f);
    public ClampedIntParameter RedColorCount = new ClampedIntParameter(4, 2, 16);
    public ClampedIntParameter GreenColorCount = new ClampedIntParameter(4, 2, 16);
    public ClampedIntParameter BlueColorCount = new ClampedIntParameter(4, 2, 16);
    public ClampedIntParameter BayerLevel = new ClampedIntParameter(1, 0, 2);

    public override CustomPostProcessInjectionPoint injectionPoint =>
        CustomPostProcessInjectionPoint.AfterPostProcess;

    private Material material;

    public bool IsActive()
    {
        return material != null && Spread.value <= 0f;
    }

    public override void Setup()
    {
        var shader = Shader.Find("Hidden/Custom/Dither");
        if (shader != null)
        {
            material = new Material(shader);
        }
        else
        {
            Debug.LogWarning("Dither shader not found.");
        }
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (material == null) return;

        material.SetFloat("_Spread", Spread.value);
        material.SetInt("_RedColorCount", RedColorCount.value);
        material.SetInt("_GreenColorCount", GreenColorCount.value);
        material.SetInt("_BlueColorCount", BlueColorCount.value);
        material.SetInt("_BayerLevel", BayerLevel.value);
        material.SetVector("_ScreenSize", new Vector4(Screen.width, Screen.height, 1.0f / Screen.width, 1.0f / Screen.height));

        cmd.Blit(source, destination, material);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(material);
    }
}