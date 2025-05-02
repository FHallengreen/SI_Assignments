## Instruction to use AUTH0 yourself

1. Go to https://auth0.com/
2. Sign up for a free account
3. Create a Tenant but choosing a name that is not already taken and server region (Europe)
4. Create a sample app: Choose Regular Web App then ASP.NET Core MVC.
5. Setup your login box but choosing which 3rd party application you want and to be able to sign in with.
   - For example, you can choose Google and Github.
   - You can also choose to use a username and password.
6. press the "try login" to test out the login box you created - then press continue and press download sample app.
   - This will download a zip file with the sample app.
   - The sample app is a .NET Core MVC application that uses Auth0 for authentication.
   - The sample app is already configured to use Auth0 and has the necessary dependencies installed.
7. Unzip the file and open the solution in Visual Studio.
8. Go to settings and make sure the following is set:
   - Allowed Callback URLs: http://localhost:3000/callback
   - Allowed Logout URLs: http://localhost:3000
9.  Run the application from your terminal (while inside the root of the project) and you should be able to login with Auth0.
    ```
    dotnet run
    ```

> Remember to have the right version of .NET Core installed. You can check the version by running the following command:
> ```
> dotnet --version
> ```
> Default version is 6.0.0 for the sample.  
