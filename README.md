# RemoteConfigurator

Attempt at creating an application that can read and manipulate the configs of other applications./websites

Based on using reflection and having each application implement a specific configuration interface

Main points of this approach are 
 * common library with a defined configuration interface 
 * individual applications implement this interface to manage their own configuration
 * Single Administration Application - scans for any assemblies implementing that interface
 * The instantiates that class for the child applications - and used that to read and write settings for that application
