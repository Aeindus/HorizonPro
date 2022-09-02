HorizonPro - Tool for System Administrators
-------------------

`This is an unofficial and automated copy of the repository hosted on tfs.`

An efficient solution for remote controlling a Windows computer. 

**Team**:
  - [Reznicencu Bogdan](https://github.com/orgs/Aeindus/people/ReznicencuBogdan)
  - [AntonVonDelta](https://github.com/orgs/Aeindus/people/AntonVonDelta)

Features
---------------------

The C++ clients provide control for: filesystem, regedit, cmd, screen, mouse.
It features:
  - COM hijacking technology for in-system seeding
  - installation and uninstallation controls
  - all in-dll execution
  - efficient and self-repairing TCP protocol

The server features:
  - an extensible MVVM arhitecture written for VB.Net and used for composing the user interfaces
  - asynchronous programming for a better control over all connected components and users
  - security checks for all packets received
  - a mapping of raw in-memory data received from C++ clients to managed types native to VB.Net
  
  
Setup
----------------

Unfortunately property sheets had to be removed from this public repo. Because of this the dependecies between projects should be remade: 
  - **Core** project is used by all components and **Loader**.
  - **Loader** requires **Core** and **LoaderCore**.
  - **ServerCore** is used only by **LumiereSombre**.
  
**Config.h.in** contains basic networking configuration for the components. The extension must be removed and the file edited. IP addresses are supported in tcp code but not in the Config.h file.

Screenshots
----------------

![image](https://user-images.githubusercontent.com/25268629/188138177-4bb447c4-6f1a-44a4-b77e-006a7c85f6d0.png)


Diagrams
----------------
  
- Logic diagram of the entire system:
  
<p align="center">
  <img src="https://user-images.githubusercontent.com/25268629/188135965-f5794021-8445-491c-8748-0633e70d9a49.png" width="600">
</p>

<br>

- Logic diagram for user/component selection implemented in server:

<p align="center">
  <img src="https://user-images.githubusercontent.com/25268629/188223862-f7fd0afd-6ba4-4830-84f9-c28864df0229.png" width="600">
</p>
