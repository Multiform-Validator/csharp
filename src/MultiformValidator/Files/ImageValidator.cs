using Microsoft.Extensions.Logging;

namespace MultiformValidator.Files;

public static class ImageValidator
{
    private static readonly ILogger? Logger = LoggerSetup.Logger;
    private static readonly string ERROR_WHILE_READING_FILE_MESSAGE = "An error occurred while reading the file: ";
    private static readonly string ILLEGAL_ARGUMENT_MESSAGE = "The input value cannot be null.";
    private static readonly string[] FILES_TYPES = ["gif", "ico", "png", "jpeg"];

    public static bool IsValidImage(FileInfo file, params string[] exclude)
    {
        if (file is null) throw new InvalidOperationException(ILLEGAL_ARGUMENT_MESSAGE);

        try
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);

            if (exclude.Length == 0) return ValidateAllImageFileTypes(fileBytes);

            var filteredList = FILES_TYPES.Except(exclude).ToArray();
            return filteredList.Length != 0 && ValidateAllImageFileTypes(fileBytes, filteredList);
        }
        catch (IOException exception)
        {
            Logger?.LogError($"{ERROR_WHILE_READING_FILE_MESSAGE} {exception.Message}");
            return false;
        }
    }

    #region [private methods]

    private static bool ValidateAllImageFileTypes(byte[] fileBytes, string[] filteredList)
    {
        var isPng = filteredList.Contains("png") && IsPng(fileBytes);
        var isIco = filteredList.Contains("ico") && IsIco(fileBytes);
        var isJpeg = filteredList.Contains("jpeg") && IsJpeg(fileBytes);
        var isGif = filteredList.Contains("gif") && IsGif(fileBytes);

        return isPng || isIco || isJpeg || isGif;
    }

    private static bool ValidateAllImageFileTypes(byte[] fileBytes)
    {
        return IsGif(fileBytes) || IsIco(fileBytes) || IsJpeg(fileBytes) || IsPng(fileBytes);
    }

    private static bool IsPng(byte[] fileBytes)
    {
        return fileBytes[0] == 0x89
            && fileBytes[1] == 0x50
            && fileBytes[2] == 0x4E
            && fileBytes[3] == 0x47;
    }


    private static bool IsJpeg(byte[] fileBytes)
    {
        return fileBytes[0] == 0xFF
            && fileBytes[1] == 0xD8
            && fileBytes[2] == 0xFF;
    }

    private static bool IsIco(byte[] fileBytes)
    {
        return fileBytes[0] == 0x00
            && fileBytes[1] == 0x00
            && fileBytes[2] == 0x01;
    }

    private static bool IsGif(byte[] fileBytes)
    {
        return fileBytes[0] == 0x47
            && fileBytes[1] == 0x49
            && fileBytes[2] == 0x46
            && fileBytes[3] == 0x38
            && fileBytes[1] == 0x49
            && fileBytes[2] == 0x46
            && fileBytes[3] == 0x38;
    }

    #endregion
}