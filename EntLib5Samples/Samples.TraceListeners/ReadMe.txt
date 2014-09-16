Rolling XML Trace Listener 
==========================

Description
===========
This solution contains a sample of a rolling XML Trace Listener.  
The functionality is based on (and leverages some of the functionality of) the Rolling Flat File Trace Listener 
so it should share the same behavior as that trace listener.  The only difference is that the XML Trace Listener 
does not support a Header, Footer, or Formatter as the Rolling Flat File Trace Listener does.  

The Rolling XML Trace Listener also support full design time integration with the Enterprise Library configuration tool.  Simply copy the assembly into the config tool working folder (e.g. C:\Program Files (x86)\Microsoft Enterprise Library 5.0\Bin) so it can load the trace listener type.
If you are using EnterpriseLibrary.config as the configuration tool (Right click on app.config "Edit configuration file")
then you will need to build the solution before opening with the configuration tool.

Requirements
============
This sample application requires:

o Enterprise Library 5 Optional Update 1 (version 5.0.505.0)
o Microsoft .NET Framework 3.5 with Service Pack 1 or Microsoft .NET Framework 4.0
o Microsoft Visual Studio 2010

Rolling Flat File Trace Listener, Flat File Trace Listener
==========================================================

Description
===========
This solution contains a Rolling XML Trace Listener as well as Flat File Trace Listener.    
The functionality is based on the Rolling Flat File Trace Listener and Flat File Trace Listener
so it should share the same behavior as that trace listener.  The only difference is that these new implementations
resolve an issue where an error occurs during initialization preventing any logging from working by throwing an
exception back to the caller when attempting to resolve a LogWriter from the container.

These Trace Listeners also support full design time integration with the Enterprise Library configuration tool.  Simply copy the assembly into the config tool working folder (e.g. C:\Program Files (x86)\Microsoft Enterprise Library 5.0\Bin) so it can load the trace listener type.
If you are using EnterpriseLibrary.config as the configuration tool (Right click on app.config "Edit configuration file")
then you will need to build the solution before opening with the configuration tool.  They should appear as 
"Custom Rolling Flat File Trace Listener" and "Custom Flat File Trace Listener" in the configuration tool.

Requirements
============
This sample application requires:

o Enterprise Library 5 Optional Update 1 (version 5.0.505.0)
o Microsoft .NET Framework 3.5 with Service Pack 1 or Microsoft .NET Framework 4.0
o Microsoft Visual Studio 2010

NuGet package restore must be enabled to download dependent Enterprise Library packages when compiling.
