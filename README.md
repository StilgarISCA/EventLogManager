Event Log Manager
===============

C# .NET console application for managing Windows Event Logs.

Abstract
--------------

Having written a number of Windows applications, sites and services, I've found it desireable to have custom application logging, so your log messages don't get lost in generic Windows application logs.

The problem with logging to custom event logs and event log sources is that both need to be created in Windows, and there isn't a built-in tool for doing so.

This command line tool is designed to help facilitate the creation and management of these custom event logs.

_This Project Is Not Complete, but the code in the master branch should be functional_

Commands
===============
**Help** &ndash; Display the help

**CreateLog "NewEventLogName" "DefaultEventSourceName"** &ndash; Creates a new event log with an event source. Enclose event log / source names with spaces in double quotes.

**CreateSource "NewEventSourceName" "TargetEventLogName"** &ndash; Creates a new event source in event log. Enclose event log / source names with spaces in double quotes.

**DeleteSource "EventSourceNameToDelete"** &ndash; Deletes an event source. Enclose source names with spaces in double quotes.

**List** &ndash; Lists all of the event logs by name

**List "EventLogName"** &ndash; Lists all of the event sources for the given event log. Enclose event log names with spaces in double quotes.

Examples
===============


**CreateLog**

*Example*

    EventLogManager CreateLog "United Federation of Planets" "Warp Drive"

*Output*

    Event Log "United Federation of Planets" created with Event Source "Warp Drive".

**CreateSource**

*Example*

    EventLogManager CreateSource "Account Deleter Service" Application

*Output*

    Event Source "Account Deleter Service" created in Application Event Log.


**DeleteSource**

*Example*

    EventLogManager DeleteSource "Account Deleter Service"

*Output*

    Event Source "Account Deleter Service" deleted.

**List**

*Example*

    EventLogManager List

*Output*

    Application
    HardwareEvents
    Key Management Service
    Security
    System
    
**List "EventLogName"**

*Example 1*
    
    EventLogManager List System

*Output*
    
    ACPI
    Application Popup
    AppReadiness
    Microsoft Antimalware
    Microsoft-Windows-Time-Service
    WPC

*Example 2*

    EventLogManager List "Key Management Service"

*Output*

    KmsRequests
    Office Software Protection Platform Service