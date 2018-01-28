https://github.com/dotnet-architecture/eShopOnContainers

# Prerequisites
Install .NET Core 2.0

## Checklist Web API
# Heartbeat controller
# DI with Autofac
# Logging with Application Insights
# Exception Handling
# 


## Call stack
Program.Main > Startup.ConfigureServices() > Startup.Configure > Controller.ActionMethod()

Startup.ConfigureServices() is optional
When using a third-party DI container, you must change ConfigureServices so that it returns IServiceProvider 
	instead of void.

# AspNet Core has no RouteConfig.cs, no Global.aspx, no Web.config.
Web.config -> appSettings.json
package.config ->  bin//*.deps.json

In Project > Properties, we have launchSettings.json
It contains IIS settings and launch profiles. 

Notice that API project is hosted on one port and IIS Express is launching on another port... ?

## Recommendatioins
Configuration should use the options pattern. 
Avoid static access to services.
Avoid service location in your application code.
Avoid static access to HttpContext.
Remember, dependency injection is an alternative to static/global object access patterns. 
You won't be able to realize the benefits of DI if you mix it with static object access.

In ASPNET Core, MVC and Web API are one thing, unlike ealier where they used to be two different things

# repo structure
root\
    *.sln - Main Solution
    src\
        projects/api
    test\
        apitest

HTTP Middleware Pipeline ---> MVC Filter Pipeline

# Logging
ASP.NET Core doesn't provide async logger methods because logging should be so fast that it isn't worth the cost of using async. 
If your data store is slow, write the log messages to a fast store first, 
then move them to a slow store later.

ASP.NET Core container             -> Autofac
----------------------                -------
// the 3 big ones
services.AddSingleton<IFoo, Foo>() -> builder.RegisterType<Foo>().As<IFoo>().SingleInstance()
services.AddScoped<IFoo, Foo>()    -> builder.RegisterType<Foo>().As<IFoo>().InstancePerLifetimeScope()
services.AddTransient<IFoo, Foo>() -> builder.RegisterType<Foo>().As<IFoo>().InstancePerDependency()

// default
services.AddTransient<IFoo, Foo>() -> builder.RegisterType<Foo>().As<IFoo>()

# Authentication

https://digitalmccullough.com/posts/aspnetcore-auth-system-demystified.html

The default auth handler provided by ASP.NET Core is the Cookies authentication handler

A scheme is just a string that identifies a unique auth handler in a dictionary of auth handlers. 
The default scheme for the Cookies auth handler is “Cookies”, but it can be changed to anything.

A middleware is a module that can be inserted into the startup sequence and is run on every request.

#CSRF/XSRF 
This Cross-Site Request Forgery attack steals uses authentication context such as Cookies, token etc. 
When the user clicks some mailicious site, the browser sends the users authentication context.
https://damienbod.com/2017/05/09/anti-forgery-validation-with-asp-net-core-mvc-and-angular/

Using SSL doesn't prevent a CSRF attack, the malicious site can send an https:// request.
use AutoValidateAntiforgeryToken broadly for non-API scenarios.
For API, we generally don't require this validation as users are aware what site/api endpoint they are accessing.

# Open Redirect Attack
This attack redirects users from trusted safe domain to another un-trusted domain.
Prevent this attack by redirecting to a local domain url.
Controller.IsLocalUrl() and Controller.LocalRedirect() can be helpful

# XSS 
This Cross-Site Scripting attack aims to steal cookies and session tokens, 
change the contents of the web page through DOM manipulation or redirect the browser to another page.
by injecting malicious scripts(example javascript)
This attack generally occur when an application takes user input and outputs it in a page without validating, 
encoding or escaping it.
Prevent this attack by performing validation of input values.
https://www.owasp.org/index.php/XSS_(Cross_Site_Scripting)_Prevention_Cheat_Sheet

# CORS
AJAX requests can be made from the same domain(or origin) only(because of browser security feature).
Two URLs have the same origin if they have identical schemes, hosts, and ports.
http://example.com/ and http://api.example.com/ are considered 2 separate origins
CORS is safer and more flexible than earlier techniques such as JSONP. 
UseCors() middleware to allow cross-origin ajax requests.





