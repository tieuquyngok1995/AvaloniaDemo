namespace ConfigGenerator.ViewModels
{
  /// <summary>
  /// Interface for view models that support configuration operations
  /// </summary>
  public interface IConfigurable
  {
    /// <summary>
    /// Generate configuration based on current settings
    /// </summary>
    void GenerateConfig();

    /// <summary>
    /// Save current settings as draft
    /// </summary>
    void SaveDraft();

    /// <summary>
    /// Cancel current operation
    /// </summary>
    void Cancel();
  }
}