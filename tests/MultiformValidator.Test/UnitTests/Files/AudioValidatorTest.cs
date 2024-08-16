using MultiformValidator.Files;

namespace MultiformValidator.Test.UnitTests.Files;

public class AudioValidatorTests
{
    private readonly string _basePath =  Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

    [Fact]
    public void IsValidAudio_FileIsNull_ThrowsInvalidOperationException()
    {
        FileInfo? file = null;
        var exception = Assert.Throws<InvalidOperationException>(() => AudioValidator.IsValidAudio(file!));
        Assert.Equal("The input value cannot be null.", exception.Message);
    }

    [Fact]
    public void IsValidAudio_FileReadThrowsIOException_LogsErrorAndReturnsFalse()
    {
        bool result = AudioValidator.IsValidAudio(new FileInfo("not/valid/path.mp3"));
        Assert.False(result);
    }

    [Fact]
    public void IsValidAudio_ValidMp3File_ReturnsTrue()
    {
        var mp3File = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/Valid/valid.mp3"));
        bool result = AudioValidator.IsValidAudio(mp3File);

        Assert.True(result);
    }

    [Fact]
    public void IsValidAudio_ValidWavFile_ReturnsTrue()
    {
        var wavFile = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/Valid/valid.wav"));
        bool result = AudioValidator.IsValidAudio(wavFile);
        
        Assert.True(result);
    }

    [Fact]
    public void IsValidAudio_FileTypeExcluded_ReturnsTrue()
    {
        var mp3File = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/Valid/valid.mp3"));
        bool result = AudioValidator.IsValidAudio(mp3File, ["mp3"]);

        Assert.False(result);
    }

    [Fact]
    public void IsValidAudio_NoExcludedTypes_ReturnsTrue()
    {
        var wavFile = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/Valid/valid.wav"));
        bool result = AudioValidator.IsValidAudio(wavFile, ["mp3"]);
        Assert.True(result);
    }

    [Fact]
    public void IsInValidAudioWav_NoExcludedTypes_ReturnsFalse()
    {
        var wavFile = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/nvalid/invalid.wav"));
        bool result = AudioValidator.IsValidAudio(wavFile);
        Assert.False(result);
    }
    
    [Fact]
    public void IsInValidAudioMp3_NoExcludedTypes_ReturnsFalse()
    {
        var wavFile = new FileInfo(Path.Combine(_basePath, "Assets/AudioValidator/Invalid/invalid.mp3"));
        bool result = AudioValidator.IsValidAudio(wavFile);
        Assert.False(result);
    }
}