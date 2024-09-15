# HOW TO BUILD:

Visual Studio is the preferred IDE. <br />
Only x86_64 architecture is supported (this is the only architecture Microsoft SQL supports).

Go to visual studio. In the get started tab click "Clone a repository". <br />
Paste the Url of this repository into the "Repository Location" field and save it on your machine. <br />

There should be two .sln files. <br />
"COP4870" is the solution for the Webstore App, this is the client side code. <br />
"eCommerce.API" is the API for the Webstore App, this is the server side code. <br />
Open both solutions in separate instances of Visual Studio. <br />

## Required NuGet Packages: <br />
**Webstore.Library:** Newtonsoft.Json(13.0.3). <br />
**WebStore.Maui:** Microsoft.Extensions.Logging.Debug(8.0.0), Microsoft.NET.ILLink.Tasks(8.0.6), Microsoft.Maui.Controls(8.0.40), Microsoft.Maui.Controls.Compatibility(8.0.40) <br />
**eCommerce.API:** Newtonsoft.Json(13.0.3), Microsoft.Data.SqlClient(5.2.2), Swashbuckle.AspNetCore(6.4.0) <br />

## How to set up the host/port for HTTPS: <br />
In "COP4870" go into the utility file and open "WebRequestHandler.cs."
In the webrequesthandler class, you should see the strings "host" and "port."
Change the port string to the port that appears when you start up the API.
When you start the API, a browser window should open. 
The URL should look something like: "localhost:[Port]/swagger/index.html" 
Copy and paste the port number in the https link into the port string in the webrequesthandler class. 

## How to set up "WebStore.Library" as a Dependency for the API: <br />
In "eCommerce.API" go into the "Dependencies" file, then into "Assemblies," and delete "WebStore.Library."
In "COP4870" click "Build," and then "Build WebStore.Library" or "Build Solution."
Also in "COP4870" right click on the "WebStore.Library" file, then click "Open Folder In File Explorer."
Click "bin," "Debug," and "net8.0."
Copy the path to this folder.
In "eCommerce.API," right click on the Dependencies file and click "Add Project Reference."
Click "Browse" in the bottom right.
Paste the path to the "net8.0" file.
Select the dll file (if the file is empty make sure you have built WebStore.Library)

## How to set up the database: <br />
Firstly, if you do not want to use a database there is an option to create and store files on your local machine.
To use this you would need to create a file on your (C:) drive called "temp", and inside that file create a file called "Items".
This is where the items will be stored.
Now in the "InventoryEC.cs" file you will need to uncomment the code which calls Filebase and Comment out the code which calls MSSQLContext.
**This only works for inventory, not shop.**<br />

To set up the SQL database, download Microsoft SQL Server Management Studio.
Then you want to create a new SQL server instance, I reccomend using SQL Server 2022 Developer Edition (because it's free).
Run the SQL Server Developer Edition installer and create a Basic server.
Go into the MSSQL Server Manager. 
For the server type select "Database Engine." 
For the server name click on the dropdown menu and select the Database Engine you created (should be the name of your computer).
If it doesn't show up there, go into "Services" from the Windows Start menu and check that your SQL server is running.
Set the authentication to "Windows Authentication" and encryption to "Mandatory."
Click "Connect".
In "eCommerce.API," right click on the solution and select "Open Folder In File Explorer." 
Click on the "eCommerce.API" folder, and then the "Database" folder.
Copy the path from the API's Database folder. 
In MSSQL Server Manager, click on "File," "Open," and then "File." 
Paste the copied path into the "open" path for the MSSQL Server Manager
Here, you can find the Querys for Item and Cart.
Open them and follow the instructions from Item and Cart respectively (Make sure that youre adding the Tables, Schemas, and Procedures in the eCommerce Database and not master).


