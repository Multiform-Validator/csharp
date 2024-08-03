using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace MultiformValidator.Files;

/// <summary>
/// Validates audio files by checking if they are of type MP3 or WAV.
/// </summary>
public class AudioValidator
{
    private static readonly ILogger? Logger = LoggerSetup.Logger;
    private static readonly string ERROR_WHILE_READING_FILE_MESSAGE = "An error occurred while reading the file: ";
    private static readonly string ILLEGAL_ARGUMENT_MESSAGE = "The input value cannot be null.";
    private static readonly IEnumerable<string> FILE_TYPES = ["mp3", "wav"];

    /// <summary>
    /// Validates whether the provided file is a valid audio file.
    /// </summary>
    /// <param name="file">The file to be validated.</param>
    /// <param name="exclude">An optional list of file types to be excluded from validation.</param>
    /// <returns>Returns <c>true</c> if the file is a valid audio and not in the exclusion list; otherwise, returns <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided file is null.</exception>
    public static bool IsValidAudio(FileInfo file, IEnumerable<string>? exclude)
    {
        if (file is null) throw new InvalidOperationException(ILLEGAL_ARGUMENT_MESSAGE);

        try
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);

            if (exclude is null) return ValidateAllAudiosFileTypes(fileBytes);

            IEnumerable<string> filteredList = exclude.Intersect(FILE_TYPES);
            return filteredList is not null && filteredList.Any() && ValidateAllAudiosFileTypes(fileBytes, filteredList);
        }
        catch (IOException exception)
        {
            Logger?.LogError($"{ERROR_WHILE_READING_FILE_MESSAGE} {exception.Message}");
            return false;
        }
    }

    /// <summary>
    /// Validates whether the provided file is a valid audio file.
    /// </summary>
    /// <param name="file">The file to be validated.</param>
    /// <param name="exclude">An optional list of file types to be excluded from validation. Use a comma-separated list (e.g., "mp3", "wav").</param>
    /// <returns>Returns <c>true</c> if the file is a valid audio and not in the exclusion list; otherwise, returns <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided file is null.</exception>
    /// <exception cref="IOException">Thrown when there is an I/O error reading the file.</exception>
    public static bool IsValidAudio(FileInfo file, params string[]? exclude) => IsValidAudio(file, exclude);

    /// <summary>
    /// Validates the audio file types based on the filtered list.
    /// </summary>
    /// <param name="fileBytes">The bytes of the file to be validated.</param>
    /// <param name="filteredList">The filtered list of allowed file types.</param>
    /// <returns>Returns <c>true</c> if the file matches one of the allowed file types; otherwise, returns <c>false</c>.</returns>
    private static bool ValidateAllAudiosFileTypes(byte[] fileBytes, IEnumerable<string> filteredList)
    {
        bool isMp3Valid = filteredList.Contains("mp3") && IsMp3(fileBytes);
        bool isWavValid = filteredList.Contains("wav") && IsWav(fileBytes);

        return isMp3Valid || isWavValid;
    }

    /// <summary>
    /// Validates the audio file types.
    /// </summary>
    /// <param name="fileByte">The bytes of the file to be validated.</param>
    /// <returns>Returns <c>true</c> if the file matches one of the valid audio types; otherwise, returns <c>false</c>.</returns>
    private static bool ValidateAllAudiosFileTypes(byte[] fileByte)
    {
        return IsMp3(fileByte) || IsWav(fileByte);
    }

    /// <summary>
    /// Checks if the file bytes correspond to the MP3 format.
    /// </summary>
    /// <param name="fileBytes">The bytes of the file to be checked.</param>
    /// <returns>Returns <c>true</c> if the bytes correspond to the MP3 format; otherwise, returns <c>false</c>.</returns>
    private static bool IsMp3(byte[] fileBytes)
    {
        return fileBytes[0] == 0x49 && fileBytes[1] == 0x44 && fileBytes[2] == 0x33;
    }

    /// <summary>
    /// Checks if the file bytes correspond to the WAV format.
    /// </summary>
    /// <param name="fileBytes">The bytes of the file to be checked.</param>
    /// <returns>Returns <c>true</c> if the bytes correspond to the WAV format; otherwise, returns <c>false</c>.</returns>
    private static bool IsWav(byte[] fileBytes)
    {
        return fileBytes[0] == 0x52
            && fileBytes[1] == 0x49
            && fileBytes[2] == 0x46
            && fileBytes[3] == 0x46;
    }
}