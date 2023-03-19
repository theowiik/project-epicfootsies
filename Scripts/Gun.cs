using Godot;
using System;

public partial class Gun : Node2D
{
    private AudioStreamPlayer2D _shootPlayer;
    public Light.LightMode LightMode { get; set; }
    public string ShootActionName { get; set; }
    private double BulletSpeed { get; set; } = 750.0f;
    private double FireRate { get; set; } = 5f;
    private float BulletSizeFactor { get; set; } = 1.0f;
    private int BulletCount { get; set; } = 10;
    private float BulletSpread { get; set; } = 0.2f;
    private PackedScene _bulletScene;
    private Timer _shootTimer;
    private bool _loading = true;
    public bool Freeze { get; set; }

    public override void _Ready()
    {
        _shootPlayer = GetNode<AudioStreamPlayer2D>("ShootPlayer");
        _bulletScene = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
        _shootTimer = GetNode<Timer>("Timer");
        _shootTimer.Timeout += () => _loading = !_loading;
        LightMode = Light.LightMode.Light;
        _shootTimer.WaitTime = 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Freeze)
            return;

        if (Input.IsActionPressed(ShootActionName) && _loading)
            Shoot();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_right"))
        {
            if (LightMode == Light.LightMode.Light)
                LightMode = Light.LightMode.Dark;
            else
                LightMode = Light.LightMode.Light;
        }
    }

    [Signal]
    public delegate void ShootDelegateEventHandler(Node2D bullet);

    private void Shoot()
    {
        _shootPlayer.PitchScale = GetLightPitch();
        _shootPlayer.Play();

        for (int i = 0; i < BulletCount; i++)
        {
            var bullet = _bulletScene.Instantiate<Bullet>();
            var shotSpread = BulletCount == 1 ? 0 : (float)GD.RandRange(-BulletSpread, BulletSpread);

            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = GetParent<Marker2D>().Rotation + shotSpread;
            bullet.Speed = GetRandomBetweenRange((float)BulletSpeed * 0.9f, (float)BulletSpeed * 1.1f);
            bullet.Scale *= BulletSizeFactor;
            bullet.LightMode = LightMode;

            EmitSignal(SignalName.ShootDelegate, bullet);
        }

        _loading = false;
        _shootTimer.Start();
    }

    private float GetRandomBetweenRange(float min, float max)
    {
        return (float)GD.RandRange(min, max);
    }

    private float GetLightPitch()
    {
        if (LightMode == Light.LightMode.Light)
            return (float)GD.RandRange(1.0f, 1.3f);
        else
            return (float)GD.RandRange(0.7f, 1.0f);
    }
}
