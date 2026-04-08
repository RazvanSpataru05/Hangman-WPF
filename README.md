# Hangman WPF
A Hangman clone built in C# WPF using the MVVM architecture. Features data persistance across sessions, management of multiple users and the possibility of guessing wrods across different categories.

## Preview
![Hangman Demo]("Media/hangman_preview.gif")

## Features
* Multiple users managed at once. Upon starting the app, the player gets to choose his user before actually playing.
* 5 word categories: Mathematics, Programming, Physics, Biology, All Categories
* Save and load functionalities. At any point during the game, the user and save his progress or load any of his saves
* Statistics tracking per user and category
* Timer-based gamepplay with 3 levels per session and 30 seconds per level
* Hand-drawn visual style

 ## Built With
* Language: C#
* UI Framework: WPF (Windows Presentation Foundation)
* .NET 8

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
3. Build and run the project ('F5')

## How to Play
1. Select an user profile or create a new one
2. Choose a word category from the menu
3. Press **New Game** to start
4. Guess letters before the hangman is complete. You can make up to 7 mistakes before losing. Each correct letter guessed adds another 3 seconds to the left time.
5. Complete 3 consecutive words to win the game 

