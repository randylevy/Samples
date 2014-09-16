Description
===========
This sample project shows a sample of a Custom Database Trace Listener that adds an additional column to the out of the box
FormattedDatabaseTraceListener.  The CustomDatabaseTraceListener will check the LogEntry to see if it is a CustomLogEntry 
and if it is then it will add the CustomData information into the CustomData column in the [Log] table.

Requirements
============
This sample application requires:

o Enterprise Library 6 (Assembly version 6.0.0.0).
o Microsoft .NET Framework 4.5
o Microsoft Visual Studio 2012
o A database server running a database that is supported by a .NET Framework 4.5 data provider.  The provided script
  does not work with SQL AZURE.  For SQL Azure support, modify the out of the box CreateLoggingDatabaseObjects.sql to include 
  the CustomData column (as well as in stored procedures).

Installation
============
Run the Script LoggingDatabase.sql against a SQL Server/Express database.
Configure app.config with the correct connection string for your environment.
NuGet package restore must be enabled to download dependent Enterprise Library packages when compiling.

Notes
=====
The CustomDatabaseTraceListener is very similar to the FormattedDatabaseTraceListener
except that the CustomDatabaseTraceListener will write one more column to the 
database in the [Log] table.

This example has been tested with SQL Server however, it should also work with other databases provided
that the stored procedure is modified to function correctly with the appropriate DBMS.

If you are using EnterpriseLibrary.config as the configuration tool (Right click on app.config "Edit configuration file v6")
then you will need to build the solution before opening with the configuration tool.  If you wish to use the standalone 
configuration tool you will need to copy the CustomDatabaseTraceListener.dll into the directory where the configuration
tool is located.