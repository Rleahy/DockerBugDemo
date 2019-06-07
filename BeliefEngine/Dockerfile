

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BeliefEngine/BeliefEngine.csproj", "BeliefEngine/"]
RUN dotnet restore "BeliefEngine/BeliefEngine.csproj"
COPY . .
WORKDIR "/src/BeliefEngine"
RUN dotnet build "BeliefEngine.csproj" -c Release -o /app


FROM tensorflow/tensorflow
EXPOSE 80
# CNTK EVRO

ENV PATH="/root/anaconda3/envs/cntk-py35/bin:${PATH}"
ENV PATH="/cntk/cntk/bin:${PATH}"


RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install ASP.NET Core


ENV ASPNETCORE_VERSION 2.1.0

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='0f37dc0fabf467c36866ceddd37c938f215c57b10c638d9ee572316a33ae66f7479a1717ab8a5dbba5a8d2661f09c09fcdefe1a3f8ea41aef5db489a921ca6f0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

ENV NUGET_XMLDOC_MODE skip
WORKDIR /vsdbg
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg
    
RUN apt-get update -y;
RUN apt-get install -y python-tk
RUN apt-get install -y openbabel

RUN mkdir /TempFiles
RUN mkdir /BeliefRefinery
RUN mkdir /TensorFlowEventFiles
RUN mkdir /Tools
WORKDIR /BeliefRefinery
RUN mkdir /Lib
RUN mkdir /Packages

COPY ./WebFiles/ /WebFiles/
COPY ./binarys/ /Packages/

WORKDIR /Packages

RUN tar xzvf mgltools_x86_64Linux2_1.5.6.tar.gz -C /Tools
WORKDIR /Tools/mgltools_x86_64Linux2_1.5.6
RUN ./install.sh
ENV PATH="/Tools/mgltools_x86_64Linux2_1.5.6/bin:${PATH}"

RUN mkdir /Tools/smina
RUN mv /Packages/smina /Tools/smina/smina
RUN chmod +x /Tools/smina/smina
ENV PATH="/Tools/smina:${PATH}"

WORKDIR /BeliefRefinery
COPY ./lib/* ./
COPY --from=Build /BeliefRefinery/out* .
COPY ./Python/* ./Lib/

ENTRYPOINT ["dotnet", "BeliefEngine.dll"]
