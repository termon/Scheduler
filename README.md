
DotNet Core 5 Clean Scheduler Solution
=====================================

This .NET project scheduler demonstrates concept of separation of concerns (SOC) and is a simple implementation of the Clean Architecture. When installed and used to create a new project, all references to ```Scheduler``` should be replaced with the name of your project.

## Core Project

The Core project contains all domain entities and has no dependencies.

Password hashing functionality added via the ```Scheduler.Core.Security.Hasher``` class. This is used in the Data project UserService to hash the user password before storing in database.

## Data Project

The Data project encapsulates all data related concerns. Internally it provides two implementations of the ```IUserService```.

1. Using a List that is persisted to a json file.
2. Using the Repository pattern implemented by Entityframework to handle data storage/retrieval. It defaults to using Sqlite for portability across platforms.

The Service is the only element exposed from this project and consumers of this project simply need reference it to access its functionalty. The List implementation of the service is purely to demonstrate multiple service implementations and could be used in the Test project.

## Test Project

The Test project references the Core and Data projects and should implement unit tests to test the functionalty of the IUserService. The tests should be extended to fully exercise the functionality of your Service.

## Web Project

The Web project uses the MVC pattern to implement a web application. It references the Core and Data projects and uses the any exposed services and models to access data management functionality. This allows the Web project to be completely independent of the persistence framework used in the Data project (EntityFrameworkCore or InMemory List in this scheduler).

### Identity

The project provides extension methods to enable:

1. User Identity using cookie authentication. This is enabled without using the boilerplate scheduler used in the Visual Studio Web MVC project. This allows the developer to gain a better appreciation of how Identity is implemented. The data project implements a User model and the UserService provides user management functionality such as Authenticate, Register, Change Password, Update Profile etc. 

    The Web project implements a UserController with actions for Login/Register/NotAuthorized/NotAuthenticated etc. The ```AuthBuilder``` helper class defined in ```Scheduler.Web.Helpers``` provides a ```BuildClaimsPrinciple``` method to build a set of user claims for User Login action when using cookie authentication

2. JWT authentication for WebAPI's. The Web project implements an ApiController with actions for Register, Login. The ```AuthBuilder``` helper class defined in ```Scheduler.Web.Helpers``` provides a ```BuildJwtToken``` method to build a set of user claims for User Login action when using JWT authentication.
    
    JWT settings are configured via a ```JwtConfig``` section in appsettings.json. Settings include ```JwtSecret```, ```JwtIssuer```, ```JwtAudience```, and ```JwtExpiryInDays```. These are already set with default values and can be amended as required. Recommended to change the JwtSecret and ensure this is not committed to git in production.

To configure one of the various authentication scanarios enable one of the following in Startup.cs ```ConfigureServices``` method.

```
services.AddCookieAuthentication();
services.AddJwtAuthentication(Configuration);
services.AddCookieAndJwtAuthentication(Configuration);
```

### Additional Functionality
The scheduler replaces the locally installed Bootstrap 4 with Bootstrap 5.0.1 delivered via CDN link.

1. Any Controller that inherits from the Web project BaseController, can utilise:

    a. The Alert functionality. Alerts can be used to display alert messages following controller actions. Review the UserController for an example using alerts.

    ```Alert("The User Was Registered Successfully", AlertType.info);```

    b. Authentication function helper methods
    * ```Identity()``` - returns Id of current logged in user or null if not logged in
    * ```IsAuthenticated()``` - returns a boolean to indicate if user is logged in

2. Two custom TagHelpers are included that provide

    a. Authentication and authorisation Tags

    * ```<p asp-authorized>Only displayed if the user is authenticated</p>```

    * ```<p asp-roles="Admin,Manager">Only displayed if the user has one of specified roles</p>```

    Note: to use these tag helpers ConfigureServices in Startup.cs needs following service added to DI container
    ```services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();```

    b. Conditional Display Tag

    * ```<p asp-condtion="@some_boolean_expression">Only displayed if the condition is true</p>```

## Install Scheduler

To install this solution as a scheduler (scheduler name is termonclean)

1. Download current version of the scheduler

    ``` $ git clone https://github.com/termon/DotNetScheduler.git```

2. Install the scheduler so it can be used by ```dotnet new``` command. Use the absolute path to the cloned scheduler directory without trailing '/'

    ``` $ dotnet new -i /absolute_path/DotNetScheduler```

3. Once installed you can create a new project using this scheduler

    ``` $ dotnet new termonclean -o SolutionName```