Description
===========
This sample project shows a sample of a Custom Exception Handler that uses a custom TraceListener to log
exception details to a database (as extended properties).

Requirements
============
This sample application requires:

o Enterprise Library 5 Optional Update 1 (version 5.0.505.0).
o Microsoft .NET Framework 3.5 with Service Pack 1 or Microsoft .NET Framework 4.0.
o Microsoft Visual Studio 2010
o A database that is supported by a .NET Framework 3.5 with Service Pack 1 or .NET Framework 4.0 data provider. 

Installation
============
Run the Script LoggingDatabase.sql against a SQL Server/Express database.
Configure app.config with the correct connection string for your environment.
NuGet package restore must be enabled to download dependent Enterprise Library packages when compiling.

Notes
=====
The ExtendedFormattedDatabaseTraceListener is very similar to the FormattedDatabaseTraceListener
except that the ExtendedFormattedDatabaseTraceListener will also write ExtendedProperties to the 
database.  ExtendedProperties are stored in the ExtendedProperties database table.

The ExtendedProperties are passed into the stored procedure as an escaped XML string. XML was chosen
so that the design can function with various databases irrespective of the specific features available.

ToString() is called on all objects in the ExtendedProperties before logging to the database so 
if an object is added to the ExtendedProperties it should implement a meaningful ToString() method 
in order to log its data.

The format of the XML string is:

<ExtendedProperties>
  <ExtendedProperty>
    <Key>hello</Key>
    <Value>world</Value>
  </ExtendedProperty
  <ExtendedProperty>
    <Key>world</Key>
    <Value>b free</Value>
  </ExtendedProperty
</ExtendedProperties>

This example has been tested with SQL Server however, it should also work with other databases provided
that the stored procedure is modified to function correctly with the appropriate DBMS.

If you are using EnterpriseLibrary.config as the configuration tool (Right click on app.config "Edit configuration file")
then you will need to build the solution before opening with the configuration tool.