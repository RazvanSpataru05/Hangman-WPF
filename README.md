# Hangman WPF
A Hangman clone built in C# WPF using the MVVM architecture. It features data persistance across sessions, management of multiple users and the possibility of guessing words across multiple categories.

## Preview
### GIF
![hangman_preview](https://github.com/user-attachments/assets/ee7b1f1d-a1cf-4a4f-8b16-b4d1d87858f0)

### Screenshots

| Sign In | Game | Statistics |
|---------|------|------------|
| <img src="https://github.com/user-attachments/assets/e43c311b-2c0b-4f6d-96ed-9e9128745270" width="250"/> | <img src="https://github.com/user-attachments/assets/0b22f2c1-9050-41c7-a90d-fb5589905408" width="250"/> | <img src="https://github.com/user-attachments/assets/91df6bb3-962c-4c10-bee1-b0d98a5811d8" width="250"/> |

## Key Features
* Multiple users managed at once. Upon starting the game, the user can choose who to play as. Each user has an unqiue name and a custom avatar;
* Five word categories to choose from: Mathematics, Programming, Physics, Biology or "All Categories";
* Save and load functionalities. At any point during the game, the user can save his progress or load any of his previous saves;
* Statistics tracking per user and category;
* Timer-based gameplay with three levels per session and thirty seconds per level;
* Hand-drawn visual style.

 ## Tech Stack
* Language: C#
* UI Framework: WPF (Windows Presentation Foundation)
* .NET 8
* Architecture: MVVM
* NuGet Packages: Microsoft.Extensions.Hosting

## How To Run The Application

### Prerequisites
- Windows 10/11
- .NET 8 SDK
- Visual Studio 2022 (or newer)

### Installation
1. Clone the repository
```bash
   git clone https://github.com/RazvanSpataru05/Hangman.git
```
2. Open 'Hangman.sln' in Visual Studio
3. Restore NuGet packages
4. Build and run the project (Press 'F5')

## How to Play
1. Select an user profile or create a new one
2. Choose a word category from the top bar menu
3. Press **File** in the top bar, then **New Game** to start
4. Guess letters by pressing them or by typing on your keyboard. You can make up to seven mistakes before losing. Each correct letter adds three seconds to the time left
5. Guess three consecutive words to win a game.

