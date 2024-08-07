using MultiformValidator.Files;

namespace MultiformValidator.Test.UnitTests.Files;

public class FileValidatorTest
{
    private readonly string _basePath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

    [Fact]
    public void IsValidFile_FileIsNull_ThrowsInvalidOperationException()
    {
        FileInfo? file = null;
        var exception = Assert.Throws<InvalidOperationException>(() => FileValidator.IsValidFile(file!));
        Assert.Equal("The input value cannot be null.", exception.Message);
    }

    [Fact]
    public void IsValidFile_FileReadThrowsIOException_LogsErrorAndReturnsFalse()
    {
        bool result = FileValidator.IsValidFile(new FileInfo("not/valid/path.txt"));
        Assert.False(result);
    }

    [Fact]
    public void IsValidFile_ValidTxtFile_ReturnsTrue()
    {
        var txtFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Valid/valid.txt"));
        bool result = FileValidator.IsValidFile(txtFile);

        Assert.True(result);
    }

    [Fact]
    public void IsValidFile_ValidPdfFile_ReturnsTrue()
    {
        var pdfFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Valid/valid.pdf"));
        bool result = FileValidator.IsValidFile(pdfFile);

        Assert.True(result);
    }

    [Fact]
    public void IsValidFile_FileTypeExcluded_ReturnsTrue()
    {
        var txtFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Valid/valid.txt"));
        bool result = FileValidator.IsValidFile(txtFile, ["pdf"]);

        Assert.True(result);
    }

    [Fact]
    public void IsValidFile_NoExcludedTypes_ReturnsTrue()
    {
        var pdfFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Valid/valid.pdf"));
        bool result = FileValidator.IsValidFile(pdfFile, ["pdf"]);
        Assert.False(result);
    }

    [Fact]
    public void IsInValidFileTxt_NoExcludedTypes_ReturnsFalse()
    {
        var txtFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Invalid/invalid.txt"));
        bool result = FileValidator.IsValidFile(txtFile);
        Assert.False(result);
    }

    [Fact]
    public void IsInValidFilePdf_NoExcludedTypes_ReturnsFalse()
    {
        var pdfFile = new FileInfo(Path.Combine(_basePath, "Assets/FileValidator/Invalid/invalid.pdf"));
        bool result = FileValidator.IsValidFile(pdfFile);
        Assert.False(result);
    }
}