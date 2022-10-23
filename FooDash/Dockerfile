FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy csproj and restore as distinct layers

COPY ./ App/


RUN dotnet restore App/src/FooDash.WebApi/FooDash.WebApi.csproj

RUN dotnet publish App/src/FooDash.WebApi -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y newrelic-netcore20-agent

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=eu01xx5d9645615af596505e6300097900b7NRAL \
NEW_RELIC_APP_NAME=devel-backend

WORKDIR /App

COPY --from=build-env /App/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "FooDash.WebApi.dll"]

