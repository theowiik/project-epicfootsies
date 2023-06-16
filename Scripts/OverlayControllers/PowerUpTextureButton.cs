using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpTextureButton : TextureButton
{
  [GetNode("RichTextLabel")]
  private RichTextLabel _richTextLabel;
  public override void _Ready()
  {
    this.AutoWire();
    GrabFocus();
    ApplyShader();
  }

  private void ApplyShader()
  {
    var shader = GD.Load<Shader>("res://Assets/Shaders/shine.gdshader");
    Material = new ShaderMaterial { Shader = shader };
  }

  public void SetLabel(string text)
  { 
    _richTextLabel.Text = $"[center]{text}[/center]";
  }
}