Follow the steps to setup the back end:

1) Download and install the latest version of neo4j (community version) from http://neo4j.com/download/

2) After the installation, start the neo4j service from the neo4j desktop application

3) Go to http://localhost:7474/ and login with the default credentials

4) From http://localhost:7474/ execute the following command in order to change the password:

      :server change-password

   Change the password to: neo4j (both username and password should be neo4j)

5) Download back end code (clone the whole repository to get it)

6) Download and install the latest version of postgreSQL from http://www.enterprisedb.com/products-services-training/pgdownload/

   While installing, set both username and password to: postgres

7) After the installation, open pgAdminIII and create a database with name: Refugee

8) Install Visual Studio 2015

9) Open the Refugee.DataAccess.Relational.sln, manage NuGet packages and build

10) Debug the project Refugee.DataAccess.Relational.Creation in order to create the schema for Refugee database and add initial test data

11) Do the following in right order:

      Open the Refugee.Common.sln, manage NuGet packages and build

      Open the Refugee.DataAccess.Graph.sln, manage NuGet packages and build

12) Install IIS with ASP.NET 4.5 support

13) Open IIS, go to Default Web Site and add https binding in port 443 with IIS Express Development Certificate (or any other certificate)

14) Open as administrator the Refugee.Server.sln, manage NuGet packages and build

15) Debug Refugee.Server to start the server