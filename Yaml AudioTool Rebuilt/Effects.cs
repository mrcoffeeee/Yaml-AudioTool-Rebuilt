using System;
using System.Windows.Forms;
using Vortice.XAudio2;

using reverbParameters = Vortice.XAudio2.Fx.ReverbI3DL2Parameters;

namespace Yaml_AudioTool_Rebuilt
{
    public class EffectsHelperFunctions
    {
        public static float DbToLinear(double db)
        {
            float linear = (float)Math.Pow(10, db / 20.0);

            // Clamp to EQ borders
            if (linear < 0.126f) linear = 0.126f;
            if (linear > 7.94f) linear = 7.94f;

            return linear;
        }

        public static double LinearToDb(float linear)
        {
            // Clamp to EQ borders
            if (linear < 0.126f) linear = 0.126f;
            if (linear > 7.94f) linear = 7.94f;

            return 20.0 * Math.Log10(linear);
        }
    }

    public class Effects
    {
        private static readonly Random rnd = new();

        public static float PitchRandomizer(float pitchValue, float pitchrandValue)
        {
            int min = Convert.ToInt32(pitchrandValue * -10);
            int max = -min;
            float rand = rnd.Next(min, max) / 10.0f;

            if (pitchValue + rand < 0.5)
                pitchValue = 0.5f;
            else if (pitchValue + rand > 2)
                pitchValue = 2;
            else
                pitchValue += rand;
            //MessageBox.Show(pitchValue.ToString());
            return pitchValue;
        }
    }

    public class EQCreationEffect
    {
        // Sets EQ-parameters (fixed frequencies: 40 / 160 / 650 / 2500 Hz)
        public static void SetEqualizer(IXAudio2SourceVoice sourceVoice)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            var eqParams = new Vortice.XAPO.EqualizerParameters
            {
                FrequencyCenter0 = 40f,
                Gain0 = EffectsHelperFunctions.DbToLinear(f1.EQGain1Pot.Value),
                Bandwidth0 = Convert.ToSingle(f1.Bandwidth1Pot.Value),

                FrequencyCenter1 = 160f,
                Gain1 = EffectsHelperFunctions.DbToLinear(f1.EQGain2Pot.Value),
                Bandwidth1 = Convert.ToSingle(f1.Bandwidth2Pot.Value),

                FrequencyCenter2 = 650f,
                Gain2 = EffectsHelperFunctions.DbToLinear(f1.EQGain3Pot.Value),
                Bandwidth2 = Convert.ToSingle(f1.Bandwidth3Pot.Value),

                FrequencyCenter3 = 2500f,
                Gain3 = EffectsHelperFunctions.DbToLinear(f1.EQGain4Pot.Value),
                Bandwidth3 = Convert.ToSingle(f1.Bandwidth4Pot.Value)
            };
            sourceVoice.SetEffectParameters(0, eqParams, 0);
        }

        public static void UpdateEqualizerSettings(IXAudio2SourceVoice sourceVoice)
        {
            if (sourceVoice != null)
            {
                SetEqualizer(sourceVoice);
            }
        }
    }

    public class RoomCreationEffects
    {
        public static FilterParameters voiceFilter;
        public static reverbParameters[] ReverbPresets =
            [
                Vortice.XAudio2.Fx.Presets.Default,
                Vortice.XAudio2.Fx.Presets.Generic,
                Vortice.XAudio2.Fx.Presets.PaddedCell,
                Vortice.XAudio2.Fx.Presets.Room,
                Vortice.XAudio2.Fx.Presets.BathRoom,
                Vortice.XAudio2.Fx.Presets.LivingRoom,
                Vortice.XAudio2.Fx.Presets.StoneRoom,
                Vortice.XAudio2.Fx.Presets.Auditorium,
                Vortice.XAudio2.Fx.Presets.ConcertHall,
                Vortice.XAudio2.Fx.Presets.Cave,
                Vortice.XAudio2.Fx.Presets.Arena,
                Vortice.XAudio2.Fx.Presets.Hangar,
                Vortice.XAudio2.Fx.Presets.CarpetedHallway,
                Vortice.XAudio2.Fx.Presets.Hallway,
                Vortice.XAudio2.Fx.Presets.StoneCorridor,
                Vortice.XAudio2.Fx.Presets.Alley,
                Vortice.XAudio2.Fx.Presets.Forest,
                Vortice.XAudio2.Fx.Presets.City,
                Vortice.XAudio2.Fx.Presets.Mountains,
                Vortice.XAudio2.Fx.Presets.Quarry,
                Vortice.XAudio2.Fx.Presets.Plain,
                Vortice.XAudio2.Fx.Presets.ParkingLot,
                Vortice.XAudio2.Fx.Presets.SewerPipe,
                Vortice.XAudio2.Fx.Presets.UnderWater,
                Vortice.XAudio2.Fx.Presets.SmallRoom,
                Vortice.XAudio2.Fx.Presets.MediumRoom,
                Vortice.XAudio2.Fx.Presets.LargeRoom,
                Vortice.XAudio2.Fx.Presets.MediumHall,
                Vortice.XAudio2.Fx.Presets.LargeHall,
                Vortice.XAudio2.Fx.Presets.Plate
            ];

        public static IXAudio2SourceVoice SetRoomFilter(IXAudio2SourceVoice sourceVoice)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int roomIndex = Convert.ToInt32(f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.roomidHeader)].Text);
            voiceFilter.Frequency = Convert.ToSingle(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filterfrequencyHeader)].Text);
            voiceFilter.OneOverQ = Convert.ToSingle(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filteroneoverqHeader)].Text);
            voiceFilter.Type = (FilterType)Convert.ToInt32(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filtertypeHeader)].Text);
            sourceVoice.SetFilterParameters(voiceFilter, operationSet: 0);
            return sourceVoice;
        }

        public static void UpdateFilterSettings(int roomlistValue, IXAudio2SourceVoice sourceVoice)
        {
            if (sourceVoice != null &&
                roomlistValue == 1)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                int roomIndex = f1.RoomListView.SelectedItems[0].Index;
                voiceFilter.Frequency = Convert.ToSingle(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filterfrequencyHeader)].Text);
                voiceFilter.OneOverQ = Convert.ToSingle(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filteroneoverqHeader)].Text);
                voiceFilter.Type = (FilterType)Convert.ToInt32(f1.RoomListView.Items[roomIndex].SubItems[f1.RoomListView.Columns.IndexOf(f1.filtertypeHeader)].Text);
                sourceVoice.SetFilterParameters(voiceFilter, operationSet: 0);
            }
        }

        // Builds the effect chain: index 0 = EQ, index 1 = Reverb.
        public static (IDisposable eq, IDisposable reverb) SetEffectChain(IXAudio2SourceVoice sourceVoice)
        {
            //Form1 f1 = (Form1)Application.OpenForms["Form1"];

            // --- Create EQ (Index 0) ---
            Vortice.XAPO.XAPO.CreateFX(Vortice.XAPO.XAPO.CLSID_FXEQ, out var eqUnknown);

            // --- Create Reverb (Index 1) ---
            var reverb = Vortice.XAudio2.Fx.Fx.XAudio2CreateReverb();

            // --- Set chain ---
            uint channels = (uint)sourceVoice.VoiceDetails.InputChannels;
            var eqDescriptor = new EffectDescriptor(eqUnknown, channels);
            var reverbDescriptor = new EffectDescriptor(reverb, channels);
            sourceVoice.SetEffectChain(eqDescriptor, reverbDescriptor);

            return (eqUnknown, reverb);
        }

        public static void SetRoomReverb(IXAudio2SourceVoice sourceVoice)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            ReverbPresets[f1.ReverbpresetComboBox.SelectedIndex].WetDryMix = Convert.ToSingle(Math.Round(f1.ReverbwetdryPot.Value, 1));
            sourceVoice.SetEffectParameters(1, Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(ReverbPresets[f1.ReverbpresetComboBox.SelectedIndex]), 0);
        }

        public static void UpdateReverbSettings(int filelistValue, IXAudio2SourceVoice sourceVoice)
        {
            if (sourceVoice != null &&
                filelistValue == 1)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                ReverbPresets[f1.ReverbpresetComboBox.SelectedIndex].WetDryMix = Convert.ToSingle(Math.Round(f1.ReverbwetdryPot.Value, 1));
                sourceVoice.SetEffectParameters(1, Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(ReverbPresets[f1.ReverbpresetComboBox.SelectedIndex]), 0);
            }
        }
    }
}
