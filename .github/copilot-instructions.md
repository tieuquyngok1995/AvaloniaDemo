# Copilot Instructions

## Mục tiêu dự án

Dự án **ConfigGenerator** là ứng dụng sử dụng .NET 9 và Avalonia UI theo mô hình **MVVM**. Mục tiêu của ứng dụng là:

- Tạo file cấu hình (`config.json`) cho các app liên quan (ví dụ: app get thông tin, app liên kết Exchange).
- Cho phép người dùng nhập thông tin cấu hình qua giao diện GUI.
- Cài đặt độc lập, không ảnh hưởng đến hệ thống.
- Tuân theo TDD (Test-Driven Development).

---

## Công nghệ sử dụng

- .NET 9
- Avalonia UI (đa nền tảng)
- MVVM
- ReactiveUI
- XUnit (Unit test)
- JsonSerializer (ghi file .json)

---

## Cấu trúc thư mục

ConfigGenerator/
├── ConfigGenerator/          # Thư mục chứa mã nguồn chính (Model, ViewModel, Services, Logic)
├── ConfigGenerator.Desktop/  # Giao diện Avalonia UI 
├── ConfigGenerator.Tests/    # Dự án test (XUnit)
└── ConfigGenerator.sln       # Solution chính

---

## Quy tắc viết mã

- Tách logic rõ ràng theo mô hình MVVM (Model - View - ViewModel).
- Tất cả các comment đều sử dụng tiếng nhất
- Luôn có comment đầu fuction
- Mỗi ViewModel chỉ xử lý logic giao tiếp UI, không chứa xử lý nặng hoặc truy cập file trực tiếp.
- Luôn dùng `try/catch` khi xử lý I/O hoặc tương tác hệ thống.
- Ưu tiên sử dụng `ReactiveCommand` cho thao tác người dùng.
- Code phải có test đi kèm (theo TDD).

---

## Kiến trúc MVVM

### ConfigGenerator/ (logic):

- Models/SettingModel.cs
- Services/ConfigService.cs
- Interfaces/ (nếu cần tách interface cho DI)

### ConfigGenerator.Desktop/ (UI + ViewModel):

- ViewModels/MainViewModel.cs
- Views/MainWindow.axaml

## Viết Test theo TDD

Ví dụ test cho `ConfigService`:

```csharp
[Fact]
public void SaveSettings_ShouldWriteJsonFile()
{
    var service = new ConfigService();
    var model = new SettingModel { AppName = "Demo", OutputPath = "out" };
    var path = "test.json";

    service.SaveSettings(model, path);

    Assert.True(File.Exists(path));
    File.Delete(path);
}
```

Chạy test:

```bash
dotnet test ConfigGenerator.Tests
```

## Chạy ứng dụng

```bash
dotnet run --project ConfigGenerator.Desktop
```

## Ghi chú

- Node.js hoặc các công cụ bổ trợ có thể được đặt trong `./tools/` và không cài vào hệ thống.
- Các file cấu hình sinh ra nên được ghi vào thư mục con tương ứng với từng app.
- Avalonia UI hỗ trợ build cross-platform (Windows, Linux, macOS).

## Gợi ý phát triển tiếp

- Cho phép chọn app (GetInfo/Exchange) qua GUI để sinh cấu hình tương ứng.
- Hỗ trợ export/import cấu hình.
- Cho phép download Node.js và extract local bằng script.

© 2025 – Phát triển bởi công ty Fujinet.
