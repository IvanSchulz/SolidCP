In order to debug FuseCP.EnterpriseServer please:

- For net48, use IIS or setup a "fusecp.enterpriseserver" site that can be run on iisexpress with iisexpress /site:fusecp.enterpriseserver 
  and that runs on http or https. http is only supported for localhost and LAN access.

  In order to configure IIS Express for this on my machine I'd have to run the following commands:
  
  IisExpressAdminCmd setupSslUrl -url:https://localhost:9021 -UseSelfSigned
  appcmd add site /name:fusecp.enterpriseserver /bindings:http/*:9020:,https/*:9021: /physicalPath:c:\github\fusecp\fusecp\sources\fusecp.enterpriseserver

  Then run as administrator iisexpress /site:fusecp.enterpriseserver
 
  Unfortunately it's not possible to debug NET Framework projects directrly in Visual Studio for SDK style projects.
  In order to debug, you have to start iisexpress from an administrator command shell and attach to the process
  within Visual Studio.

- For net6 & WSL set the correct certificate settings in launchSettings.json or appsettings.json to configure the
   server certificate WCF should use, or run the server only over http.
