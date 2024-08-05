using Microsoft.Extensions.Logging;

namespace MultiformValidator.Files;

public static class FileValidator
{
    private static readonly ILogger? Logger = LoggerSetup.Logger;
    private static readonly string ERROR_WHILE_READING_FILE_MESSAGE = "An error occurred while reading the file: ";
    private static readonly string ILLEGAL_ARGUMENT_MESSAGE = "The input value cannot be null.";


    /// <summary>
    /// Validates whether the specified file is a valid PDF.
    /// </summary>
    /// <param name="file">The file to be validated.</param>
    /// <returns>
    /// <c>true</c> if the file is a valid PDF; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidPdf(FileInfo file)
    {
        try
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);
            return IsPdf(fileBytes);
        }
        catch (IOException exception)
        {
            Logger?.LogError($"{ERROR_WHILE_READING_FILE_MESSAGE} {exception.Message}");
            return false;
        }
    }

    private static bool IsPdf(byte[] fileBytes)
    {
        return fileBytes[0] == 0x25
            && fileBytes[1] == 0x50
            && fileBytes[2] == 0x44
            && fileBytes[3] == 0x46;
    }

    /// <summary>
    /// Validates whether the specified file is a valid text file.
    /// </summary>
    /// <param name="file">The file to be validated.</param>
    /// <returns>
    /// <c>true</c> if the file is a valid text file; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidTxt(FileInfo file)
    {
        try
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);
            return IsTxt(fileBytes);
        }
        catch (IOException exception)
        {
            Logger?.LogError($"{ERROR_WHILE_READING_FILE_MESSAGE} {exception.Message}");
            return false;
        }
    }

    private static bool IsTxt(byte[] fileBytes)
    {
        if (fileBytes.Length < 0) return false;
        foreach (byte b in fileBytes)
        {
            if ((b < 0x20 || b > 0x7e) && b != 0x0a && b != 0x0d) return false;
        }

        return true;
    }
}