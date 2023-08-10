# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.209.6/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 6.0, 5.0, 3.1, 6.0-bullseye, 5.0-bullseye, 3.1-bullseye, 6.0-focal, 5.0-focal, 3.1-focal
ARG VARIANT="7.0-bullseye-slim"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Optional] Uncomment this section to install additional OS packages.
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends protobuf-compiler libnss3-tools awscli

ENV PATH="$PATH:$HOME/.dotnet/tools"
ENV PATH="$PATH:/home/vscode/.dotnet/tools"
RUN dotnet tool install -g dotnet-reportgenerator-globaltool

# Install MongoDB command line tools if on buster and x86_64 (arm64 not supported)
ARG MongoDbToolsVersion=100.6.1
RUN wget https://fastdl.mongodb.org/tools/db/mongodb-database-tools-debian92-x86_64-$MongoDbToolsVersion.deb && \
    apt install ./mongodb-database-tools-*.deb && \
    rm -f mongodb-database-tools-*.deb
