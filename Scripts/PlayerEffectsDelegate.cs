﻿using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class PlayerEffectsDelegate : Node2D
{
    public delegate void PlayerEffectPerformed(Node2D effect);

    private const string JumpAnimation = "stretch_jump";
    private const string LandAnimation = "squish_land";
    private const string RunLeftAnimation = "running_left";
    private const string RunRightAnimation = "running_right";
    private const string SpawnAnimation = "spawn";
    private const string Wall = "squish_wall";
    private readonly Color _hurtColor = new(0.8f, 0, 0);

    [GetNode("AnimationPlayer")]
    private AnimationPlayer _animationPlayer;

    private PackedScene _deathParticlesScene =
        ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/DeathParticles.tscn");

    [GetNode("Sfx/DeathPlayer")]
    private AudioStreamPlayer2D _deathPlayer;

    [GetNode("Sfx/FallDeathPlayer")]
    private AudioStreamPlayer2D _fallDeathPlayer;

    private PackedScene _hurtParticlesScene =
        ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/HurtParticles.tscn");

    [GetNode("Sfx/HurtPlayer")]
    private AudioStreamPlayer2D _hurtPlayer;

    [GetNode("HurtTimer")]
    private Timer _hurtTimer;

    // TODO: Create a object pool for particles
    private PackedScene _jumpParticlesScene =
        ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/JumpParticles.tscn");

    [GetNode("Sfx/JumpPlayer")]
    private AudioStreamPlayer2D _jumpPlayer;

    public PlayerEffectPerformed PlayerEffectAddedListeners;
    public Sprite2D PlayerSprite { get; set; }

    public override void _Ready()
    {
        this.AutoWire();
        _hurtTimer.Timeout += HurtTimerOnTimeout;
    }

    private void HurtTimerOnTimeout()
    {
        PlayerSprite.Modulate = Colors.White;
    }

    public void EmitDeathParticles()
    {
        var instance = GenerateParticles(_deathParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    public void EmitJumpParticles()
    {
        var instance = GenerateParticles(_jumpParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    public void EmitHurtParticles()
    {
        var instance = GenerateParticles(_hurtParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    private static Node2D GenerateParticles(PackedScene particlesScene)
    {
        var instance = particlesScene.Instantiate<CpuParticles2D>();
        var timer = TimerFactory.OneShotStartedTimer(instance.Lifetime);

        instance.Emitting = true;
        timer.Timeout += () => instance.QueueFree();
        instance.AddChild(timer);

        return instance;
    }

    public void PlayDeathSound()
    {
        _deathPlayer.Play();
    }

    public void PlayHurtSound()
    {
        _hurtPlayer.Play();
    }

    public void PlayFallDeathSound()
    {
        _fallDeathPlayer.Play();
    }

    public void PlayJumpSound()
    {
        _jumpPlayer.Play();
    }

    public void AnimationPlayJump()
    {
        _animationPlayer.Play(JumpAnimation);
    }

    public void AnimationPlayLand()
    {
        _animationPlayer.Play(LandAnimation);
    }

    public void AnimationPlayRunLeft()
    {
        _animationPlayer.Play(RunLeftAnimation);
    }

    public void AnimationPlayRunRight()
    {
        _animationPlayer.Play(RunRightAnimation);
    }

    public void AnimationPlayWall()
    {
        _animationPlayer.Play(Wall);
    }

    public void AnimationPlayHurt()
    {
        PlayerSprite.Modulate = _hurtColor;
        _hurtTimer.Start();
    }

    public void AnimationPlaySpawn()
    {
        _animationPlayer.Play(SpawnAnimation);
    }
}