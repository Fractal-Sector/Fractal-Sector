using Content.Client._FS.VoiceBark;
using Content.Shared._FS.VoiceBark;
using Robust.Client.UserInterface.Controls;
using Range = Robust.Client.UserInterface.Controls.Range;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Content.Client.Lobby.UI;

public partial class HumanoidProfileEditor
{
    private List<VoiceBarkPrototype> _voiceBarkList = [];

    private VoiceBarkPrototype? SelectedVoiceBark =>
        VoiceBarkButton.SelectedId >= 0 &&
        VoiceBarkButton.SelectedId < _voiceBarkList.Count
            ? _voiceBarkList[VoiceBarkButton.SelectedId]
            : null;

    public void InitializeVoiceBark()
    {
        BarkPitchSlider.OnReleased += _ => OnBarkPitchSliderChanged();
        BarkPitchVarianceSlider.OnReleased += _ => OnBarkPitchVarianceSliderChanged();
        BarkPauseSlider.OnReleased += _ => OnBarkPauseSliderChanged();

        VoiceBarkButton.OnItemSelected += OnVoiceBarkButtonItemSelected;
        VoiceBarkPlayButton.OnPressed += OnVoiceBarkPlayButtonPressed;
    }

    public void UpdateVoiceBarkControls()
    {
        VoiceBarkButton.Clear();
        if (Profile is null)
            return;

        _voiceBarkList = _entManager.System<VoiceBarkSystem>().GetVoiceList();
        if (_voiceBarkList.Count == 0)
            return;

        var selectedId = -1;

        for (var i = 0; i < _voiceBarkList.Count; i++)
        {
            var voice = _voiceBarkList[i];
            if (voice.ID == Profile.BarkVoice)
                selectedId = i;

            var name = Loc.GetString($"voice-bark-{voice.ID.ToLower()}");
            VoiceBarkButton.AddItem(name, i);
        }

        if (selectedId == -1)
        {
            selectedId = 0;
            SetVoiceBark(_voiceBarkList[selectedId].ID, Profile.BarkSettings);
            return;
        }

        VoiceBarkButton.SelectId(selectedId);
        UpdateVoiceBarkSliderValues();
    }

    private void SetVoiceBark(string proto, VoiceBarkPercentageApplyData settings)
    {
        Profile = Profile?.WithBarkVoice(proto, settings);
        ReloadProfilePreview();
        PreviewVoiceBark();
    }

    private void PreviewVoiceBark()
    {
        if (Profile is null)
            return;

        _entManager.System<VoiceBarkPreviewSystem>()
            .PlayGlobal(Profile.BarkVoice, Loc.GetString("humanoid-profile-editor-bark-preview-text"), Profile.BarkSettings);
    }

    private void OnVoiceBarkPlayButtonPressed(BaseButton.ButtonEventArgs obj)
    {
        PreviewVoiceBark();
    }

    private void OnVoiceBarkButtonItemSelected(OptionButton.ItemSelectedEventArgs selected)
    {
        if (Profile is null || SelectedVoiceBark is null)
            return;

        VoiceBarkButton.SelectId(selected.Id);
        SetVoiceBark(SelectedVoiceBark.ID, Profile.BarkSettings);
    }

    private void UpdateVoiceBarkSliderValues()
    {
        if (Profile is null)
            return;

        BarkPauseSlider.Value = Profile.BarkPause;
        BarkPitchSlider.Value = Profile.BarkPitch;
        BarkPitchVarianceSlider.Value = Profile.BarkPitchVariance;
    }

    private void OnBarkPauseSliderChanged()
    {
        if (Profile is null)
            return;

        SetVoiceBark(Profile.BarkVoice, new()
        {
            Pause = (byte) BarkPauseSlider.Value,
            Pitch = Profile.BarkPitch,
            Volume = Profile.BarkVolume,
            PitchVariance = Profile.BarkPitchVariance,
        });
    }

    private void OnBarkPitchVarianceSliderChanged()
    {
        if (Profile is null)
            return;

        SetVoiceBark(Profile.BarkVoice, new()
        {
            Pause = Profile.BarkPause,
            Pitch = Profile.BarkPitch,
            Volume = Profile.BarkVolume,
            PitchVariance = (byte) BarkPitchVarianceSlider.Value,
        });
    }

    private void OnBarkPitchSliderChanged()
    {
        if (Profile is null)
            return;

        SetVoiceBark(Profile.BarkVoice, new()
        {
            Pause = Profile.BarkPause,
            Pitch = (byte) BarkPitchSlider.Value,
            Volume = Profile.BarkVolume,
            PitchVariance = Profile.BarkPitchVariance,
        });
    }
}
