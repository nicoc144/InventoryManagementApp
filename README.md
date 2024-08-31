HOW TO BUILD:

Visual Studio is the preferred IDE.

Push The code to visual studio:
Go to visual studio. In the get started tab click "Clone a repository".
Paste the Url of this repository into the "Repository Location" field and save it on your machine.

There should be two .sln files. 
"COP4870" is the solution for the Webstore App, this is the client side code.
"eCommerce.API" is the API for the Webstore App, this is the server side code.
Open both solutions in separate instances of Visual Studio.

Install NuGet Packages:


In "COP4870" go into the utility file and open "WebRequestHandler.cs." 
In the webrequesthandler class, you should see the strings "host" and "port."
Change the port string to the port that appears when you start up the API.
When you start the API a browser window should open a url which should look something like "localhost:[Port]/swagger/index.html"
Copy and paste the port number in the https link into the port string in the webrequesthandler class.

To set up the SQL database, download Microsoft SQL Server Management Studio.
Then you want to create a new SQL server instance, I reccomend using SQL Server 2022 Developer Edition (because it's free).
Run the SQL Server Developer Edition installer and create a Basic server.
Go into the MSSQL Server Manager. 
For the server type select "Database Engine." 
For the server name click on the dropdown menu and select the Database Engine you created (should be the name of your computer).
If it doesn't show up there, go into "Services" from the Windows Start menu and check that your SQL server is running.
Set the authentication to "Windows Authentication" and encryption to "Mandatory."
Click "Connect".
In "eCommerce.API," right click on the solution and select "Open Folder In File Explorer." Click on the "eCommerce.API" folder, and then the "Database" folder.
In MSSQL Server Manager, click on "File," "Open," and then "File." Copy and paste the path from the API's Database folder. 
Here, you can find the Querys for Item and Cart.
Open them and follow the instructions (Make sure that youre adding the Tables, Schemas, and Procedures in the eCommerce Database after creating it).


