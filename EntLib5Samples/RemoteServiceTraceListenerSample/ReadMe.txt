Description
===========
This solution contains a sample using the Silverlight Remote Trace Listener to send messages from a Silverlight client to a WCF service where the message is logged to a file and the event log.  

Requirements
============
This sample application requires:

o Silverlight Integration Pack for Microsoft Enterprise Library 5.0
o Enterprise Library 5 Optional Update 1 (version 5.0.505.0)
o Silverlight 4
o Microsoft .NET Framework 3.5 with Service Pack 1 or Microsoft .NET Framework 4.0
o Microsoft Visual Studio 2010

Notes
=====
Enable NuGet package restore to download required NuGet packages when compiling.

There are 2 projects: a web application and a Silverlight application.  

The Silverlight client reads in an embedded Enterprise Library Silverlight configuration which is configured to send a message to a RemoteServiceTraceListener.  It then logs a message which is sent to a WCF service in the web application.  The message should be logged to a file trace.log in the web application directory as well as the Event Log.

Additional tips:

o F5 and Ctrl-F5 do not work due to cross site scripting protection.  So instead of F5 use View in Browser for RemoteServiceTraceListenerSampleTestPage.aspx.
o The WCF service is one-way so it will (almost) always appear to succeed.  If the service is not logging it is usually permissions.  Try running as Administrator with UAC turned off to check if it is a permission issue.  Also, you can uncomment the system.diagnostics trace section in the web.config to get a WCF trace file -- this will help if an exception is being thrown since it won't make it back to the Silverlight client.

If you need to go cross domain (or get F5 working), you will need to add configuration as per:

http://msdn.microsoft.com/en-us/library/cc197955(v=vs.95).aspx

Add a file called clientaccesspolicy.xml to the root of the target domain and then add a policy to the config.  E.g.:

<?xml version="1.0" encoding="utf-8"?>
<access-policy>
  <cross-domain-access>
    <policy>
      <allow-from http-request-headers="SOAPAction">
        <domain uri="*"/>
      </allow-from>
      <grant-to>
        <resource path="/" include-subpaths="true"/>
      </grant-to>
    </policy>
  </cross-domain-access>
</access-policy>
