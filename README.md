# FreeWave <!-- omit in toc -->

Events Take The Wheel!

- [Requirements](#requirements)
   - [Environment](#environment)
   - [Tools and Frameworks](#tools-and-frameworks)
   - [Windows](#windows)
- [Getting Started](#getting-started)

## Requirements

### Environment

- [VSCode](https://code.visualstudio.com/)
- [Dev Containers VSCode Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)
- [WSL2](https://learn.microsoft.com/en-us/windows/wsl/install) (see [Windows](#windows) section before installing Docker)
- [Docker](https://docs.docker.com/engine/install/)

:warning: If you are on a corporate VPN, you will have to disconnect. :warning:

### Tools and Frameworks

You do **not** need to install any of these. They come pre-configured and installed using the devcontainer.

- [.NET Core 7](https://dotnet.microsoft.com/en-us/download)
- [Redis](https://redis.io/) (requries WSL)
- [MongoDB](https://www.mongodb.com/)
- [gRPC](https://grpc.io/)

### Windows

If you are on Windows you will need to install [WSL2](https://learn.microsoft.com/en-us/windows/wsl/install) before installing Docker Desktop for Windows.

Due to how linux utilizes RAM, WSL2 will attempt to eat up all your memory.
As such, we will need to limit how much RAM WSL2 is allowed to use. I usually set it to half my system RAM.
After installing WSL2, follow these steps:
1. Shutdown WSL in command prompt:
   ```bash
   wsl --shutdown
   ```
1. Open Notepad or preferred text editor
1. Paste in this:
   ```bash
   [wsl2]
   memory=2GB
   ```
1. Save as: "%UserProfile%\.wslconfig"
1. Open Docker and answer yes to restarting WSL2.

## Getting Started

This project has 4 projects to truly let Events Take The Wheel. For each of the following folders, you need to open a new terminal and run `dotnet run` in the order the folders are listed:

1. [./src/EventsTakeTheWheel.ChargeController](./src/EventsTakeTheWheel.ChargeController)
   - This is a console application that represents an IoT device constantly emitting data
1. [./src/EventsTakeTheWheel.EventProcessor](./src/EventsTakeTheWheel.EventProcessor)
   - This is a worker application responsible for processing data events from IoT devices
   - You could use this to trigger events that the BFF could listen and respond to as well
1. [./src/EventsTakeTheWheel.BFF](./src/EventsTakeTheWheel.BFF)
   - Backend For Frontend that services the console UI application
1. [./src/EventsTakeTheWheel.UI](./src/EventsTakeTheWheel.UI)
   - Console application that represents the UI
