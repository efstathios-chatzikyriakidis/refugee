front end


Currently we are using yeoman as our main angular generator (routes,views,controllers,services etc).
Also we are using grunt as a strong build/serve/test/* manager, 
bower as our lightweight package manager and finally npm as our hardcore package manager (globaly installed files)

In order to make frontend side code functional follow these steps:
(During the first 3 steps internet connection is needed)

======================STEP 1 =====================================================
-Install npm (the newer the better)
-Install grunt-cli
-Install bower
-Install yo
-Install generator-karma (not mandatory)
-Install generator-angular (not mandatory)

(Just run this command (as root/admin) : npm install -g grunt-cli bower yo generator-karma generator-angular)  :) :) :)


======================STEP 2 ======================================================
Afterwards pull the frontend side code
git pull .....

======================STEP 3 =====================================================
Install all needed dependencies/libraries by typing:

bower install (must be in the root folder -> front-end where bower.json lives in)
npm install (must be in the root folder ->where packages.json lives in )
======================STEP 4 =====================================================
Build project by typing grunt in root folder

======================STEP 5 =====================================================
In order to run the front-end side code just type in root folder:
grunt serve --force 
