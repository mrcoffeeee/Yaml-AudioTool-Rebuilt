using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

//using CSCore;
//using CSCore.Streams.Effects;
using Vortice.XAudio2;

using reverbParameters = Vortice.XAudio2.Fx.ReverbI3DL2Parameters;
//using volumeMeterLevels = Vortice.XAudio2.Fx.VolumeMeterLevels;

namespace Yaml_AudioTool_Rebuilt
{
    public class Effects
    {
        //private ISampleSource sampleSource;

        public static FilterParameters voiceFilter;
        public static reverbParameters[] ReverbPresets =
            {
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
            };
        public static float[] peakLevel = null;
        public static float[] rmsLevel = null;

        /*public IWaveSource SetPitchshift(IWaveSource soundSource)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            float pitchValue = Convert.ToSingle(f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchHeader)].Text);

            float pitchrandValue = Convert.ToSingle(f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchrandHeader)].Text) * 100.00f;
            if (pitchrandValue != 0)
            {
                var rand = new Random();
                int randTemp = rand.Next(0, Convert.ToInt32(pitchrandValue));
                pitchValue += Convert.ToSingle(randTemp / 100.00f);

                if (pitchValue > 2)
                    pitchValue = 2;
                else if (pitchValue < 0.5f)
                    pitchValue = 0.5f;
            }

            sampleSource = soundSource.ToSampleSource();
            PitchShifter pitchshifter = new(sampleSource)
            {
                PitchShiftFactor = pitchValue
            };
            soundSource = pitchshifter.ToWaveSource(16);
            return soundSource;
        }*/

        public static IXAudio2SourceVoice SetRoomFilter(IXAudio2SourceVoice sourceVoice)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];            
            int roomIndex = Convert.ToInt32(f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.roomidHeader)].Text);
            voiceFilter.Frequency = Convert.ToSingle(f1.roomlistView.Items[roomIndex].SubItems[f1.roomlistView.Columns.IndexOf(f1.filterfrequencyHeader)].Text);
            voiceFilter.OneOverQ = Convert.ToSingle(f1.roomlistView.Items[roomIndex].SubItems[f1.roomlistView.Columns.IndexOf(f1.filteroneoverqHeader)].Text);
            voiceFilter.Type = (FilterType)Convert.ToInt32(f1.roomlistView.Items[roomIndex].SubItems[f1.roomlistView.Columns.IndexOf(f1.filtertypeHeader)].Text);
            sourceVoice.SetFilterParameters(voiceFilter, operationSet: 0);
            return sourceVoice;
        }

        public static void UpdateFilterSettings(int filelistValue, IXAudio2SourceVoice sourceVoice)
        {
            if (sourceVoice != null &&
                filelistValue == 1)
            {
                SetRoomFilter(sourceVoice);
            }
        }
        
        public static IXAudio2SourceVoice SetRoomReverb(IXAudio2SourceVoice sourceVoice)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            var reverb = Vortice.XAudio2.Fx.Fx.CreateAudioReverb();

            var effectDescriptor = new EffectDescriptor(reverb, sourceVoice.VoiceDetails.InputChannelCount);
            sourceVoice.SetEffectChain(effectDescriptor);
            sourceVoice.SetEffectParameters(0, Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(ReverbPresets[f1.reverbpresetcomboBox.SelectedIndex], false));
            ReverbPresets[f1.reverbpresetcomboBox.SelectedIndex].WetDryMix = Convert.ToSingle(Math.Round(f1.reverbwetdryPot.Value, 1));
            sourceVoice.EnableEffect(0);
            return sourceVoice;
        }

        public static void UpdateReverbSettings(int filelistValue, IXAudio2SourceVoice sourceVoice)
        {
            if (sourceVoice != null &&
                filelistValue == 1)
            {
                SetRoomReverb(sourceVoice);
            }
        }

        public static IXAudio2SourceVoice SetVolumeMeter(IXAudio2SourceVoice sourceVoice)
        {
            //Form1 f1 = (Form1)Application.OpenForms["Form1"];
            var volumeMeter = Vortice.XAudio2.Fx.Fx.CreateAudioVolumeMeter();
            var effectDescriptor = new EffectDescriptor(volumeMeter, sourceVoice.VoiceDetails.InputChannelCount);
            sourceVoice.SetEffectChain(effectDescriptor);
            //volumeMeterLevels volumeLevel = new(peakLevel, rmsLevel, 2);
            //sourceVoice.SetEffectParameters(0, volumeLevel);
            sourceVoice.EnableEffect(0);
            return sourceVoice;
        }
    }

    public class PitchShifter
    {
        /****************************************************************************
        *
        * NAME: PitchShift.cs
        * VERSION: 1.2
        * HOME URL: http://www.dspdimension.com
        * KNOWN BUGS: none
        *
        * SYNOPSIS: Routine for doing pitch shifting while maintaining
        * duration using the Short Time Fourier Transform.
        *
        * DESCRIPTION: The routine takes a pitchShift factor value which is between 0.5
        * (one octave down) and 2. (one octave up). A value of exactly 1 does not change
        * the pitch. numSampsToProcess tells the routine how many samples in indata[0...
        * numSampsToProcess-1] should be pitch shifted and moved to outdata[0 ...
        * numSampsToProcess-1]. The two buffers can be identical (ie. it can process the
        * data in-place). fftFrameSize defines the FFT frame size used for the
        * processing. Typical values are 1024, 2048 and 4096. It may be any value <=
        * MAX_FRAME_LENGTH but it MUST be a power of 2. osamp is the STFT
        * oversampling factor which also determines the overlap between adjacent STFT
        * frames. It should at least be 4 for moderate scaling ratios. A value of 32 is
        * recommended for best quality. sampleRate takes the sample rate for the signal 
        * in unit Hz, ie. 44100 for 44.1 kHz audio. The data passed to the routine in 
        * indata[] should be in the range [-1.0, 1.0), which is also the output range 
        * for the data, make sure you scale the data accordingly (for 16bit signed integers
        * you would have to divide (and multiply) by 32768). 
        *
        * COPYRIGHT 1999-2006 Stephan M. Bernsee <smb [AT] dspdimension [DOT] com>
        *
        * 						The Wide Open License (WOL)
        *
        * Permission to use, copy, modify, distribute and sell this software and its
        * documentation for any purpose is hereby granted without fee, provided that
        * the above copyright notice and this license appear in all source copies. 
        * THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT EXPRESS OR IMPLIED WARRANTY OF
        * ANY KIND. See http://www.dspguru.com/wol.htm for more information.
        *
        *****************************************************************************/

        /****************************************************************************
        *
        * This code was converted to C# by Michael Knight
        * madmik3 at gmail dot com. 
        * http://sites.google.com/site/mikescoderama/ 
        * 
        *****************************************************************************/

        #region Private Static Memebers
        private static int MAX_FRAME_LENGTH = 16000;
        private static float[] gInFIFO = new float[MAX_FRAME_LENGTH];
        private static float[] gOutFIFO = new float[MAX_FRAME_LENGTH];
        private static float[] gFFTworksp = new float[2 * MAX_FRAME_LENGTH];
        private static float[] gLastPhase = new float[MAX_FRAME_LENGTH / 2 + 1];
        private static float[] gSumPhase = new float[MAX_FRAME_LENGTH / 2 + 1];
        private static float[] gOutputAccum = new float[2 * MAX_FRAME_LENGTH];
        private static float[] gAnaFreq = new float[MAX_FRAME_LENGTH];
        private static float[] gAnaMagn = new float[MAX_FRAME_LENGTH];
        private static float[] gSynFreq = new float[MAX_FRAME_LENGTH];
        private static float[] gSynMagn = new float[MAX_FRAME_LENGTH];
        private static long gRover, gInit;
        #endregion

        #region Public Static  Methods
        public static void PitchShift(float pitchShiftFactor, long numSampsToProcess,
           float sampleRate, float[] indata)
        {
            PitchShift(pitchShiftFactor, numSampsToProcess, (long)2048, (long)10, sampleRate, indata);
        }
        public static void PitchShift(float pitchShift, long numSampsToProcess, long fftFrameSize,
            long osamp, float sampleRate, float[] indata)
        {
            double magn, phase, tmp, window, real, imag;
            double freqPerBin, expct;
            long i, k, qpd, index, inFifoLatency, stepSize, fftFrameSize2;


            float[] outdata = indata;
            /* set up some handy variables */
            fftFrameSize2 = fftFrameSize / 2;
            stepSize = fftFrameSize / osamp;
            freqPerBin = sampleRate / (double)fftFrameSize;
            expct = 2.0 * Math.PI * (double)stepSize / (double)fftFrameSize;
            inFifoLatency = fftFrameSize - stepSize;
            if (gRover == 0) gRover = inFifoLatency;


            /* main processing loop */
            for (i = 0; i < numSampsToProcess; i++)
            {

                /* As long as we have not yet collected enough data just read in */
                gInFIFO[gRover] = indata[i];
                outdata[i] = gOutFIFO[gRover - inFifoLatency];
                gRover++;

                /* now we have enough data for processing */
                if (gRover >= fftFrameSize)
                {
                    gRover = inFifoLatency;

                    /* do windowing and re,im interleave */
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        window = -.5 * Math.Cos(2.0 * Math.PI * (double)k / (double)fftFrameSize) + .5;
                        gFFTworksp[2 * k] = (float)(gInFIFO[k] * window);
                        gFFTworksp[2 * k + 1] = 0.0F;
                    }


                    /* ***************** ANALYSIS ******************* */
                    /* do transform */
                    ShortTimeFourierTransform(gFFTworksp, fftFrameSize, -1);

                    /* this is the analysis step */
                    for (k = 0; k <= fftFrameSize2; k++)
                    {

                        /* de-interlace FFT buffer */
                        real = gFFTworksp[2 * k];
                        imag = gFFTworksp[2 * k + 1];

                        /* compute magnitude and phase */
                        magn = 2.0 * Math.Sqrt(real * real + imag * imag);
                        phase = Math.Atan2(imag, real);

                        /* compute phase difference */
                        tmp = phase - gLastPhase[k];
                        gLastPhase[k] = (float)phase;

                        /* subtract expected phase difference */
                        tmp -= (double)k * expct;

                        /* map delta phase into +/- Pi interval */
                        qpd = (long)(tmp / Math.PI);
                        if (qpd >= 0) qpd += qpd & 1;
                        else qpd -= qpd & 1;
                        tmp -= Math.PI * (double)qpd;

                        /* get deviation from bin frequency from the +/- Pi interval */
                        tmp = osamp * tmp / (2.0 * Math.PI);

                        /* compute the k-th partials' true frequency */
                        tmp = (double)k * freqPerBin + tmp * freqPerBin;

                        /* store magnitude and true frequency in analysis arrays */
                        gAnaMagn[k] = (float)magn;
                        gAnaFreq[k] = (float)tmp;

                    }

                    /* ***************** PROCESSING ******************* */
                    /* this does the actual pitch shifting */
                    for (int zero = 0; zero < fftFrameSize; zero++)
                    {
                        gSynMagn[zero] = 0;
                        gSynFreq[zero] = 0;
                    }

                    for (k = 0; k <= fftFrameSize2; k++)
                    {
                        index = (long)(k * pitchShift);
                        if (index <= fftFrameSize2)
                        {
                            gSynMagn[index] += gAnaMagn[k];
                            gSynFreq[index] = gAnaFreq[k] * pitchShift;
                        }
                    }

                    /* ***************** SYNTHESIS ******************* */
                    /* this is the synthesis step */
                    for (k = 0; k <= fftFrameSize2; k++)
                    {

                        /* get magnitude and true frequency from synthesis arrays */
                        magn = gSynMagn[k];
                        tmp = gSynFreq[k];

                        /* subtract bin mid frequency */
                        tmp -= (double)k * freqPerBin;

                        /* get bin deviation from freq deviation */
                        tmp /= freqPerBin;

                        /* take osamp into account */
                        tmp = 2.0 * Math.PI * tmp / osamp;

                        /* add the overlap phase advance back in */
                        tmp += (double)k * expct;

                        /* accumulate delta phase to get bin phase */
                        gSumPhase[k] += (float)tmp;
                        phase = gSumPhase[k];

                        /* get real and imag part and re-interleave */
                        gFFTworksp[2 * k] = (float)(magn * Math.Cos(phase));
                        gFFTworksp[2 * k + 1] = (float)(magn * Math.Sin(phase));
                    }

                    /* zero negative frequencies */
                    for (k = fftFrameSize + 2; k < 2 * fftFrameSize; k++) gFFTworksp[k] = 0.0F;

                    /* do inverse transform */
                    ShortTimeFourierTransform(gFFTworksp, fftFrameSize, 1);

                    /* do windowing and add to output accumulator */
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        window = -.5 * Math.Cos(2.0 * Math.PI * (double)k / (double)fftFrameSize) + .5;
                        gOutputAccum[k] += (float)(2.0 * window * gFFTworksp[2 * k] / (fftFrameSize2 * osamp));
                    }
                    for (k = 0; k < stepSize; k++) gOutFIFO[k] = gOutputAccum[k];

                    /* shift accumulator */
                    //memmove(gOutputAccum, gOutputAccum + stepSize, fftFrameSize * sizeof(float));
                    for (k = 0; k < fftFrameSize; k++)
                    {
                        gOutputAccum[k] = gOutputAccum[k + stepSize];
                    }

                    /* move input FIFO */
                    for (k = 0; k < inFifoLatency; k++) gInFIFO[k] = gInFIFO[k + stepSize];
                }
            }
        }
        #endregion

        #region Private Static Methods
        public static void ShortTimeFourierTransform(float[] fftBuffer, long fftFrameSize, long sign)
        {
            float wr, wi, arg, temp;
            float tr, ti, ur, ui;
            long i, bitm, j, le, le2, k;

            for (i = 2; i < 2 * fftFrameSize - 2; i += 2)
            {
                for (bitm = 2, j = 0; bitm < 2 * fftFrameSize; bitm <<= 1)
                {
                    if ((i & bitm) != 0) j++;
                    j <<= 1;
                }
                if (i < j)
                {
                    temp = fftBuffer[i];
                    fftBuffer[i] = fftBuffer[j];
                    fftBuffer[j] = temp;
                    temp = fftBuffer[i + 1];
                    fftBuffer[i + 1] = fftBuffer[j + 1];
                    fftBuffer[j + 1] = temp;
                }
            }
            long max = (long)(Math.Log(fftFrameSize) / Math.Log(2.0) + .5);
            for (k = 0, le = 2; k < max; k++)
            {
                le <<= 1;
                le2 = le >> 1;
                ur = 1.0F;
                ui = 0.0F;
                arg = (float)Math.PI / (le2 >> 1);
                wr = (float)Math.Cos(arg);
                wi = (float)(sign * Math.Sin(arg));
                for (j = 0; j < le2; j += 2)
                {

                    for (i = j; i < 2 * fftFrameSize; i += le)
                    {
                        tr = fftBuffer[i + le2] * ur - fftBuffer[i + le2 + 1] * ui;
                        ti = fftBuffer[i + le2] * ui + fftBuffer[i + le2 + 1] * ur;
                        fftBuffer[i + le2] = fftBuffer[i] - tr;
                        fftBuffer[i + le2 + 1] = fftBuffer[i + 1] - ti;
                        fftBuffer[i] += tr;
                        fftBuffer[i + 1] += ti;

                    }
                    tr = ur * wr - ui * wi;
                    ui = ur * wi + ui * wr;
                    ur = tr;
                }
            }
        }
        #endregion
    }
}
