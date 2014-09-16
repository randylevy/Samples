Description
===========
This solution contains a sample of using WCF Exception Shielding as well as using the Logging Exception Handler to log to a flat file.  

To Run, set Service1.svc as the start page (resolve Enterprise Library references if required) and use Ctrl-F5 to start the WCF Test Client and then invoke the GetData method.  The XML response should show MyFaultContract returned and the file trace.log in the web site should contain error information.

Requirements
============
This sample application requires:

o Enterprise Library 5 Optional Update 1 (version 5.0.505.0)
o Microsoft .NET Framework 3.5 with Service Pack 1 or Microsoft .NET Framework 4.0
o Microsoft Visual Studio 2010

NuGet package restore must be enabled to download dependent Enterprise Library packages when compiling.
