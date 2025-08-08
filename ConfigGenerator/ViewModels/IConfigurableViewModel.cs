namespace ConfigGenerator.ViewModels
{
  public interface IConfigurableViewModel
  {
    void GenerateConfig();
    void SaveDraft();
    void Cancel();
  }
}