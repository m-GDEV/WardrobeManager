# Contributing Document

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.
 
If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

 Don't forget to give the project a star! Thanks again!
 1. Fork the Project on Github
 2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
 3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
 4. Push to the Branch (`git push origin feature/AmazingFeature`)
 5. Open a Pull Request on Github

## How To Setup The Development Environment
1. Install `.NET 8.0 SDK` for your platform
2. Install `Node` and `npm` and make sure that they are in your PATH
4. Install `docker`

## How To Run The Project Locally
1. Clone the repository with `git clone https://github.com/m-GDEV/WardrobeManager` and run `cd ./WardrobeManager`
2. Open two terminals
  * In one, run `cd ./WardrobeManager.Presentation` and `dotnet watch -lp https`
  * In the other, run `cd ./WardrobeManager.Api` and `dotnet watch -lp https`
3. Go to `https://localhost:7026` and see the backend running
3. Go to `https://localhost:7147` and see the frontend running

## How To Run Tests
TODO: actually make tests before you fill this out lol
