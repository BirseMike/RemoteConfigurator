# RemoteConfigurator

Attempt at creating an application that can read and manipulate the configs of other applications./websites

Based on using reflection and having each application implement a specific configuration interface

Main points of this approach are 
 * common library with a defined configuration interface 
 * individual applications implement this interface to manage their own configuration
 * Single Administration Application - scans for any assemblies implementing that interface
 * The instantiates that class for the child applications - and used that to read and write settings for that application
 
 Advantages
 * Each Application defines how to manage it's own settings
 
 Disadvantages
 * Maintenance Application needs to use the same assemblies as every instantiated class.  (This means that for websites - we either need to instantiate all the website usings - or have separate simpler project that implements the configuration class...
 
 
 Options to Extend:
 * Extend with setting name - so we can distinguish between the instantiated classes
 * Use a settings type file - to expose information about what setting types are and possible values...
 
