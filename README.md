<!-- https://github.com/othneildrew/Best-README-Template -->

<div align="center">

![GitHub commit activity](https://img.shields.io/github/commit-activity/m/m-GDEV/WardrobeManager)
[![Contributors](https://img.shields.io/github/contributors/m-GDEV/WardrobeManager.svg)](https://github.com/m-GDEV/WardrobeManager/graphs/contributors)
[![Forks](https://img.shields.io/github/forks/m-GDEV/WardrobeManager.svg?style=flat)](https://github.com/m-GDEV/WardrobeManager/network/members)
![GitHub Repo stars](https://img.shields.io/github/stars/m-GDEV/WardrobeManager?style=flat&link=https%3A%2F%2Fgithub.com%2Fm-GDEV%2FWardrobeManager%2Fstargazers)
[![Issues](https://img.shields.io/github/issues/m-GDEV/WardrobeManager.svg)](https://github.com/m-GDEV/WardrobeManager/issues)
[![MIT License](https://img.shields.io/github/license/m-GDEV/WardrobeManager.svg)](https://github.com/m-GDEV/WardrobeManager/blob/master/LICENSE.txt)
![Website](https://img.shields.io/website?url=https%3A%2F%2Fwardrobemanager.connectwithmusa.com%2F&up_message=active&down_message=inactive&label=demo&link=https%3A%2F%2Fwardrobemanager.connectwithmusa.com%2F)

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/m-GDEV/WardrobeManager/BuildAndPushApiDockerImage.yml?branch=master&style=flat&label=API%20Docker%20Image%20Build&link=https%3A%2F%2Fgithub.com%2Fm-GDEV%2FWardrobeManager%2Factions%2Fworkflows%2FBuildAndPushApiDockerImage.yml)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/m-GDEV/WardrobeManager/BuildAndPushPresentationDockerImage.yml?branch=master&style=flat&label=Presentation%20Docker%20Image%20Build&link=https%3A%2F%2Fgithub.com%2Fm-GDEV%2FWardrobeManager%2Factions%2Fworkflows%2FBuildAndPushPresentationDockerImage.yml)

![Docker Image Size](https://img.shields.io/docker/image-size/tmclncy/wardrobemanager-api?sort=date&label=API%20Docker%20Image%20Size&link=https%3A%2F%2Fhub.docker.com%2Frepository%2Fdocker%2Ftmclncy%2Fwardrobemanager-api%2Fgeneral)
![Docker Image Size](https://img.shields.io/docker/image-size/tmclncy/wardrobemanager-presentation?sort=date&label=Presentation%20Docker%20Image%20Size&link=https%3A%2F%2Fhub.docker.com%2Frepository%2Fdocker%2Ftmclncy%2Fwardrobemanager-presentation%2Fgeneral)

</div>


 
 <!--
 [![LinkedIn](https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&colorB=555)](https://linkedin.com/in/musa-ahmed)
-->

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/m-GDEV/WardrobeManager">
    <img src="WardrobeManager.Presentation/wwwroot/img/icon.webp" alt="Logo" width="200" height="200">
  </a>

<h3 align="center">WardrobeManager</h3>

  <p align="center">
    Welcome to the world’s first ever all-in-one wardrobe management solution
    <br />
    <a href="https://github.com/m-GDEV/WardrobeManager/docs"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://wardrobemanager.connectwithmusa.com/">View Demo</a>
    ·
    <a href="https://github.com/m-GDEV/WardrobeManager/issues/new">Report Bug</a>
    ·
    <a href="https://github.com/m-GDEV/WardrobeManager/issues/new">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
# Links
* [About The Project](#about-the-project)
* [Built With](#built-with)
* [Installation](#installation)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
  * [Development Environment](#development-environment)
* [License](#license)
* [Contact](#contact)
* [Acknowledgements](#acknowledgements)

<!-- ABOUT THE PROJECT -->
# About The Project

Wardrobe Manager simplifies clothing organization and outfit planning. Add and manage clothing items with images, track wear frequency, and specify preferred combinations. Set wash schedules, mark favorites, and filter by season. This fully open-source application helps optimize wardrobe choices and plan outfits effortlessly. Easily self-hostable, Wardrobe Manager puts you in control of your clothing data and helps you build the perfect look, every time.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

![Demo Video](./docs/demo-video.gif)

## Built With

![](https://img.shields.io/badge/ASP.NET%20Web%20Api-512BD4?logo=.NET&logoColor=white)
![](https://img.shields.io/badge/Blazor%20Web%20Assembly-512BD4?logo=Blazor&logoColor=white)
![](https://img.shields.io/badge/Tailwind%20CSS-06B6D4?logo=tailwindcss?logoColor=white)
![](https://img.shields.io/badge/DaisyUI-1AD1A5?logo=daisyui?logoColor=white)
![](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?logo=bootstrap&logoColor=white)
![](https://img.shields.io/badge/Docker-2496ED&logo=docker?logoColor=white)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
# Installation

### Docker

To deploy this application yourself (self-hosted) you can use **docker compose** with an environment file to configure the app.

`docker-compose.yml` (this is likely out of date, check [here](https://github.com/m-GDEV/WardrobeManager/tree/master/docker-compose.yml) for the most up to date version):

```yml
services:
  wardrobe-manager-api:
    image: tmclncy/wardrobemanager-api:latest
    container_name: wardrobe-manager-api
    env_file:
      ./docker/config.env # change this to the right location
    ports:
      - "9000:8080"
    volumes:
      - ./data:/data # you may use a local bind-mount or an actual docker volume

  wardrobe-manager-presentation:
    image: tmclncy/wardrobemanager-presentation:latest
    container_name: wardrobe-manager-presentation
    env_file:
      ./docker/config.env # change this to the right location
    ports:
      - "9001:80"
```

`config.env` (this is likely out of date, check [here](https://github.com/m-GDEV/WardrobeManager/tree/master/docker-compose.yml) for the most up to date version)
```dotenv
# Wardrobe Manager Example Configuration File

# These settings will afffect the API and Presentation

# Url of WardrobeManager.Presentation
WM_FRONTEND_URL=https://localhost:9001

# Url of WardrobeManager.Api
WM_BACKEND_URL=https://localhost:9000

# Maximum image upload size 
WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB=5

# Persistent storage location (do not change if running in docker, change docker volume mount instead)
WM_DATA_DIRECTORY=/data

# Not prefix with 'WM_' as Blazor WASM applications do not have server logs 
# https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging
# Allowed Levels: Trace, Debug, Information, Warning, Error, Critical, None
Logging__LogLevel__Default=Information
```

**Example setup guide (on UNIX based systems):**
```bash
# Make new directory & go into it:
cd ~
mkdir wardrobe-manager
cd wardrobe-manager

# Download docker-compose.yml:
curl -LO https://raw.githubusercontent.com/m-GDEV/WardrobeManager/master/docker-compose.yml

# Download config.env:
curl -LO https://raw.githubusercontent.com/m-GDEV/WardrobeManager/master/docker/config.env

# Modify docker-compose.yml & config.env as appropriate
vi docker-compose.yml
vi config.env

# Run application with docker compose
docker compose up -d
```


<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
# Roadmap

- [x] Add, view, edit, and delete clothing items
- [x] Login with multiple user accounts
- [x] Customize UI using over 20+ different themes
- [x] Advanced filtering & searching of clothing items
- [x] Ability to deploy application with Docker Compose (update readme)
- [x] Use native authentication, do not rely on Auth0
- [ ] Figure out how to sync client & server versions
- [ ] Responsive UI
- [ ] Write tests
- [ ] Outfits
  - [ ] Create "outfits", collection of multiple clothing items
  - [ ] Generate outfits dynamically
  - [ ] Group outfits together by colour theme (neutral, vibrant, etc)
  - [ ] Dynamically generate outfits based on color theme (neutral, vibrant, etc)
- [ ] Generate thumbnails & use them
- [ ] Image processing
  - [ ] Use image processing to detect most common color in image of clothing item
  - [ ] Use image processing to "magically" remove backound of uploaded images
- [ ] Maybe: user management & admin dashboard

See the [open projects](https://github.com/m-GDEV/WardrobeManager/projects) for a full list of proposed features & a more detailed roadmap.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
# Contributing

## Development Environment

1. Clone the repository: `git clone https://github.com/m-GDEV/WardrobeManager`
2. Open solution in IDE of your choice
3. Make sure you have npm and nodejs installed and that they are in your PATH
4. Run both projects with 'dotnet watch' launch profile
5. Profit

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project on Github
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request on Github

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Top contributors:

<a href="https://github.com/m-GDEV/WardrobeManager/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=m-GDEV/WardrobeManager" alt="contrib.rocks image" />
</a>



<!-- LICENSE -->
# License

Distributed under the GPL 3.0 License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
# Contact

Musa Ahmed - [musaa.ahmed7@gmail.com](mailto:musaa.ahmed7@gmail.com)

Project Link: [https://github.com/m-GDEV/WardrobeManager](https://github.com/m-GDEV/WardrobeManager)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
# Acknowledgments

* [Best-README-Template: for this cool template](https://github.com/othneildrew/Best-README-Template)


<p align="right">(<a href="#readme-top">back to top</a>)</p>

