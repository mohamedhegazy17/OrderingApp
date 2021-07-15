# Start with Windows Server Core image
FROM microsoft/windowsservercore

RUN powershell -Command Add-WindowsFeature NET-WCF-TCP-Activation45

# Creates a directory for the Host
WORKDIR app

# Listen on port 83.
EXPOSE 83

# Copy the WCF host into the container.
COPY Ordering.WindowsService/bin/Debug .

Ordering.WindowsService\bin\Debug

ENTRYPOINT Ordering.WindowsService.exe