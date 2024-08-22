using Microsoft.Xna.Framework.Audio;

namespace Platformer2D;

interface ISoundEffect
{
    SoundEffect SoundEffect { get; }
    bool Play();
}

public class SoundEffectWrapper : ISoundEffect
{
    private SoundEffect _soundEffect;
    public SoundEffect SoundEffect
    {
        get { return _soundEffect;  }
    }

    public SoundEffectWrapper(SoundEffect effect)
    {
        this._soundEffect = effect;
    }

    public bool Play()
    {
        return _soundEffect != null && _soundEffect.Play();
    }
}