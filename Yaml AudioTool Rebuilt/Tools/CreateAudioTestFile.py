import wave
import argparse
import numpy as np


def create_sine_wav_fast(
    filename: str,
    samplerate: int = 44100,
    bit_depth: int = 16,
    channels: int = 2,
    duration_minutes: float = 1.0,
    frequency: float = 1000.0,
    amplitude: float = 0.8,
    block_seconds: float = 10.0,
):
    if channels not in (1, 2):
        raise ValueError("channels muss 1 für Mono oder 2 für Stereo sein.")

    if bit_depth not in (8, 16, 24, 32):
        raise ValueError("bit_depth muss 8, 16, 24 oder 32 sein.")

    total_frames = int(duration_minutes * 60 * samplerate)
    block_frames = int(block_seconds * samplerate)

    sample_width = bit_depth // 8

    with wave.open(filename, "wb") as wav_file:
        wav_file.setnchannels(channels)
        wav_file.setsampwidth(sample_width)
        wav_file.setframerate(samplerate)

        written_frames = 0

        while written_frames < total_frames:
            frames_to_write = min(block_frames, total_frames - written_frames)

            sample_indices = np.arange(
                written_frames,
                written_frames + frames_to_write,
                dtype=np.float64
            )

            sine = np.sin(2 * np.pi * frequency * sample_indices / samplerate)
            sine *= amplitude

            if bit_depth == 8:
                audio = ((sine + 1.0) * 127.5).astype(np.uint8)

            elif bit_depth == 16:
                audio = (sine * 32767).astype("<i2")

            elif bit_depth == 24:
                audio_32 = (sine * 8388607).astype("<i4")
                audio = audio_32.view(np.uint8).reshape(-1, 4)[:, :3].flatten()

            elif bit_depth == 32:
                audio = (sine * 2147483647).astype("<i4")

            if channels == 2:
                if bit_depth == 24:
                    audio = np.repeat(audio.reshape(-1, 3), 2, axis=0).flatten()
                else:
                    audio = np.repeat(audio[:, np.newaxis], 2, axis=1).flatten()

            wav_file.writeframes(audio.tobytes())
            written_frames += frames_to_write

            percent = written_frames / total_frames * 100
            print(f"\rFortschritt: {percent:6.2f} %", end="")

    print("\nFertig.")
    print(f"Datei: {filename}")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Erstellt eine schnelle 1-kHz-Sinus-WAV-Datei."
    )

    parser.add_argument("-o", "--output", default="sinus_1khz.wav")
    parser.add_argument("-sr", "--samplerate", type=int, default=44100)
    parser.add_argument("-b", "--bit-depth", type=int, default=16, choices=[8, 16, 24, 32])
    parser.add_argument("-c", "--channels", type=int, default=2, choices=[1, 2])
    parser.add_argument("-m", "--minutes", type=float, default=1.0)
    parser.add_argument("-f", "--frequency", type=float, default=1000.0)
    parser.add_argument("-a", "--amplitude", type=float, default=0.8)

    args = parser.parse_args()

    create_sine_wav_fast(
        filename=args.output,
        samplerate=args.samplerate,
        bit_depth=args.bit_depth,
        channels=args.channels,
        duration_minutes=args.minutes,
        frequency=args.frequency,
        amplitude=args.amplitude,
    )