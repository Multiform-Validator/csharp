using Microsoft.Extensions.Logging;

namespace MultiformValidator.Files;

public class AudioValidator
{
    private static readonly ILogger? Logger = LoggerSetup.Logger;
    private static readonly string ERROR_WHILE_READING_FILE_MESSAGE = "An error occurred while reading the file: ";
    private static readonly string ILLEGAL_ARGUMENT_MESSAGE = "The input value cannot be null.";
    private static readonly string[] FILE_TYPES = ["mp3", "wav"];

    /// <summary>
    /// Validates whether the provided file is a valid audio file.
    /// </summary>
    /// <param name="file">The file to be validated.</param>
    /// <param name="exclude">An optional list of file types to be excluded from validation.</param>
    /// <returns>Returns <c>true</c> if the file is a valid audio and not in the exclusion list; otherwise, returns <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided file is null.</exception>
    public static bool IsValidAudio(FileInfo file, params string[] exclude)
    {
        if (file is null) throw new InvalidOperationException(ILLEGAL_ARGUMENT_MESSAGE);

        try
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);

            if (exclude.Length == 0) return ValidateAllAudiosFileTypes(fileBytes);

            var filteredList = FILE_TYPES.Except(exclude).ToArray();
            return filteredList.Length != 0 && ValidateAllAudiosFileTypes(fileBytes, filteredList);
        }
        catch (IOException exception)
        {
            Logger?.LogError($"{ERROR_WHILE_READING_FILE_MESSAGE} {exception.Message}");
            return false;
        }
    }
    
    #region [private methods]
    private static bool ValidateAllAudiosFileTypes(byte[] fileBytes, string[] filteredList)
    {
        bool isMp3Valid = filteredList.Contains("mp3") && IsMp3(fileBytes);
        bool isWavValid = filteredList.Contains("wav") && IsWav(fileBytes);

        return isMp3Valid || isWavValid;
    }

    private static bool ValidateAllAudiosFileTypes(byte[] fileByte)
    {
        return IsMp3(fileByte) || IsWav(fileByte);
    }

    private static bool IsMp3(byte[] fileBytes)
    {
        return fileBytes[0] == 0x49 && fileBytes[1] == 0x44 && fileBytes[2] == 0x33;
    }

    private static bool IsWav(byte[] fileBytes)
    {
        return fileBytes[0] == 0x52
            && fileBytes[1] == 0x49
            && fileBytes[2] == 0x46
            && fileBytes[3] == 0x46;
    }

    #endregion
}