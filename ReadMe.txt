This application is a work in progress exercise in showcasing some web development skills
including .NET MVC, C#, HTML, Javascript and CSS. 

It is a simple version of an expenses application that allows users to create and submit 
expenses claims, and allows admins to review and then approve/reject them as they wish. 

The application utilises a code first database creation, so on running it for the first time
it will attempt to create a SQL Server db on your local machine and seed it with some relevant
data. You may need to configure the target local environment in the connection string in the
web.config file. 

When creating the database two users will automatically be generated for you to log in with. 
One represents a typical user, and the other represents an administrator. The log in details 
for each are as follows:

Regular User - 
Username: user@expensemanager.com
Password: Pa$$w0rd

Admin User - 
Username: admin@expensemanager.com
Password: Pa$$w0rd

Some simple rules for each user type:

Regular User - 
A regular user can create claims and add receipts to those claims. They can submit their claims
(moving them from Draft to Submitted) and they can withdraw them back to Draft. These users
will only see their own claims when the log in.

Admin User-
An admin user can do the same as a regular user (in terms of creating their own claims) but
they can also approve or reject other peoples. Admin's cannot edit the receipts of another 
user's claims. Admin's cannot approve or reject their own claims. 

Dev Notes:

Unfortunately I have not had the time to complete this app and have only spent a couple of 
days on it so far. Currently there is no section for 'analyzing' the claims (i.e. seeing 
graphical representation of the data). It's also currently missing some form validation to 
ensure correct data types are submitted, and some of the logic around admins dealing with 
their own claims needs a bit more attention. 

I have also not had a chance to write up some Test Methods for this application as I went. 