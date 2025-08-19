using System;

namespace ConfigGenerator.Models;

/// <summary>
/// ナビゲーションメニューの項目を表すモデルクラスです。
/// ModelType（対応するページやモデルの型）、IconKey（アイコンのリソースキー）、Label（表示名）を持ちます。
/// </summary>
public record NavigationMenuItem(Type ModelType, string IconKey, string Label);
