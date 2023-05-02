using Godot;
using static World;

public partial class Overlay : Control
{
    private PackedScene _powerUpPicker = GD.Load<PackedScene>("res://Objects/UI/PowerUpPicker.tscn");

    [GetNode("VBox/RoundTimerLabel")]
    private Label _timerLabel;

    [GetNode("VBox/TotalScoreLabel")]
    private Label _totalScoreLabel;

    [GetNode("VBox/RoundScoreBar")]
    private TextureProgressBar _roundScoreBar;

    [GetNode("VBox/Logs")]
    private RichTextLabel _logs;

    [Signal]
    public delegate void PowerUpSelectedEventHandler();

    public string Time
    {
        set
        {
            _timerLabel.Text = value;
        }
    }

    public string TotalScore
    {
        set
        {
            _totalScoreLabel.Text = value;
        }
    }

    public Results RoundScore
    {
        set
        {
            _roundScoreBar.Value = (float)value.Dark / (value.Light + value.Dark);
        }
    }

    public override void _Ready()
    {
        this.AutoWire();
    }

    public void StartPowerUpSelection()
    {
        var instance = _powerUpPicker.Instantiate<PowerUpPicker>();
        AddChild(instance);
        Log("PowerUpPicker instantiated");
    }

    private void Log(string msg)
    {
        _logs.Text += msg + "\n";
    }
}
