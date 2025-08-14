using CommunityToolkit.Mvvm.ComponentModel;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// すべてのViewModelの基底クラスです。MVVMパターンで利用します。
/// ObservableObjectを継承し、プロパティ変更通知機能を提供します。
/// </summary>
public class ViewModelBase : ObservableObject { }
