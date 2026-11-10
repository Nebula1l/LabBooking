- Вимоги до середовища:

Для запуску проєкту потрібні:
- .NET SDK 10.0 або новіший;
- Visual Studio Code або інше середовище розробки для C#;
- розширення C# від Microsoft для Visual Studio Code;
- Entity Framework Core CLI (`dotnet-ef`).

Якщо інструмент Entity Framework Core не встановлений, виконайте команду:

dotnet tool install --global dotnet-ef

- Створення бази даних:

Відкрийте термінал у папці проєкту "LabBooking", виконайте такі команди:

dotnet restore

dotnet ef migrations add InitialCreate

Для створення та заповнення бази даних SQLite:

dotnet ef database update

- Запуск програми:

У терміналі в папці проєкту виконайте dotnet run. Після запуску термінал покаже локальну адресу застосунку - відкрийте цю адресу у браузері. Для зупинки програми в терміналі натисніть Ctrl + C.